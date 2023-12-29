using System.Text.Json;
using Grpc.Core;
using GrpcLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Services;

public class ClientService(TheaterDbContext dbContext) : ClientManager.ClientManagerBase
{
    public override async Task<GetClientInfoReply> GetClientInfo(GetClientInfoRequest request,
        ServerCallContext context)
    {
        var client = await dbContext.Clients
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == request.UserId);
        var clientInfo = JsonSerializer.Serialize(client);
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetClientInfo)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new GetClientInfoReply
        {
            ClientInfo = clientInfo
        });
    }

    public override async Task<BuyTicketsReply> BuyTickets(BuyTicketsRequest request, ServerCallContext context)
    {
        var client = await dbContext.Clients.FindAsync(request.ClientId);
        if (client == null)
            return await Task.FromResult(new BuyTicketsReply
            {
                Result = false,
                Description = "Client ID not found."
            });
        var session = await dbContext.Sessions.FindAsync(request.SessionId);
        if (session == null)
            return await Task.FromResult(new BuyTicketsReply
            {
                Result = false,
                Description = "Session ID not found."
            });
        if (request.NoTickets > session.AvailableSeats)
            return await Task.FromResult(new BuyTicketsReply
            {
                Result = false,
                Description = "Not enough tickets available."
            });
        var purchaseTotal = request.NoTickets * session.TicketPrice;
        if (purchaseTotal > client.Funds)
            return await Task.FromResult(new BuyTicketsReply
            {
                Result = false,
                Description = "Not enough funds available."
            });
        // Update available seats and funds
        session.AvailableSeats -= request.NoTickets;
        dbContext.Sessions.Update(session);
        client.Funds -= purchaseTotal;
        dbContext.Clients.Update(client);
        await dbContext.SaveChangesAsync();
        // Add reservation, movement and log
        await dbContext.Reservations.AddAsync(new Reservation
        {
            ClientId = request.ClientId,
            SessionId = request.SessionId,
            NoTickets = request.NoTickets,
            TimeOfPurchase = request.TimeOfPurchase.ToDateTime(),
            Total = purchaseTotal
        });
        await dbContext.Movements.AddAsync(new Movement
        {
            ClientId = request.ClientId,
            Description = "Reservation",
            Value = -purchaseTotal
        });
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.ClientId,
            Message = nameof(BuyTickets)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new BuyTicketsReply
        {
            Result = true,
            Description = "Operation successful."
        });
    }

    public override async Task<MarkAsWatchedReply> MarkAsWatched(MarkAsWatchedRequest request,
        ServerCallContext context)
    {
        if (await dbContext.Watched.FindAsync(request.UserId, request.ShowId) != null)
            return await Task.FromResult(new MarkAsWatchedReply
            {
                Result = false,
                Description = "Show already marked as watched."
            });
        await dbContext.Watched.AddAsync(new Watched
        {
            ClientId = request.UserId,
            ShowId = request.ShowId
        });
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(MarkAsWatched)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new MarkAsWatchedReply
        {
            Result = true,
            Description = "Operation successful."
        });
    }

    public override async Task<AddFundsReply> AddFunds(AddFundsRequest request, ServerCallContext context)
    {
        var client = await dbContext.Clients.FindAsync(request.UserId);
        if (client == null)
            return await Task.FromResult(new AddFundsReply
            {
                Result = false,
                Description = "Client ID not found."
            });
        client.Funds += request.Funds;
        dbContext.Clients.Update(client);
        await dbContext.Movements.AddAsync(new Movement
        {
            ClientId = client.Id,
            Description = $"Funds;{request.PaymentMethod}",
            Value = request.Funds
        });
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(AddFunds)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new AddFundsReply
        {
            Result = true,
            Description = "Funds added successfully.",
            TotalFunds = client.Funds
        });
    }

    public override async Task<RefundReply> Refund(RefundRequest request, ServerCallContext context)
    {
        var reservation = await dbContext.Reservations.FindAsync(request.ReservationId);
        if (reservation == null)
            return await Task.FromResult(new RefundReply
            {
                Result = false,
                Description = "Reservation ID not found.",
                Funds = -1
            });
        var client = await dbContext.Clients.FindAsync(request.UserId);
        if (client == null)
            return await Task.FromResult(new RefundReply
            {
                Result = false,
                Description = "Client ID not found",
                Funds = -1
            });
        var session = await dbContext.Sessions.FindAsync(reservation.SessionId);
        session!.AvailableSeats += reservation.NoTickets;
        dbContext.Sessions.Update(session);
        client.Funds += reservation.Total;
        dbContext.Clients.Update(client);
        await dbContext.SaveChangesAsync();
        dbContext.Reservations.Remove(reservation);
        await dbContext.Movements.AddAsync(new Movement
        {
            ClientId = request.UserId,
            Description = "Refund",
            Value = reservation.Total
        });
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(Refund)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new RefundReply
        {
            Result = true,
            Description = "Refund successful.",
            Funds = reservation.Total
        });
    }

    public override async Task<GetReservationsReply> GetReservations(GetReservationsRequest request,
        ServerCallContext context)
    {
        var startDate = request.StartDate.ToDateTime();
        var endDate = request.EndDate.ToDateTime();
        var reservations = await dbContext.Reservations
            .Where(x => x.ClientId == request.UserId && x.TimeOfPurchase <= endDate && x.TimeOfPurchase >= startDate)
            .Include(x => x.Session).ThenInclude(x => x!.Show)
            .Include(x => x.Session).ThenInclude(x => x!.Theater)
            .OrderByDescending(x => x.TimeOfPurchase)
            .ToListAsync();
        var json = JsonSerializer.Serialize(reservations);
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetReservations)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new GetReservationsReply
        {
            Reservations = json
        });
    }
}