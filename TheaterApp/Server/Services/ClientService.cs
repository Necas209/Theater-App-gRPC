using System.Text.Json;
using System.Text.Json.Serialization;
using Grpc.Core;
using GrpcLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Services;

public class ClientService : ClientManager.ClientManagerBase
{
    private readonly TheaterDbContext _context;

    public ClientService(TheaterDbContext context)
    {
        _context = context;
    }

    public override async Task<GetClientInfoReply> GetClientInfo(GetClientInfoRequest request,
        ServerCallContext context)
    {
        var client = await _context.Clients
            .Where(x => x.Id == request.UserId)
            .Include(x => x.User)
            .FirstAsync();
        var clientInfo = JsonSerializer.Serialize(client, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetClientInfo)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new GetClientInfoReply
        {
            ClientInfo = clientInfo
        });
    }

    public override async Task<BuyTicketsReply> BuyTickets(BuyTicketsRequest request, ServerCallContext context)
    {
        var client = await _context.Clients.FindAsync(request.ClientId);
        if (client == null)
            return await Task.FromResult(new BuyTicketsReply
            {
                Result = false,
                Description = "Client ID not found."
            });
        var session = await _context.Sessions.FindAsync(request.SessionId);
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
        _context.Sessions.Update(session);
        client.Funds -= purchaseTotal;
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
        // Add reservation, movement and log
        await _context.Reservations.AddAsync(new Reservation
        {
            ClientId = request.ClientId,
            SessionId = request.SessionId,
            NoTickets = request.NoTickets,
            TimeOfPurchase = request.TimeOfPurchase.ToDateTime(),
            Total = purchaseTotal
        });
        await _context.Movements.AddAsync(new Movement
        {
            ClientId = request.ClientId,
            Description = "Reservation",
            Value = -purchaseTotal
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.ClientId,
            Message = nameof(BuyTickets)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new BuyTicketsReply
        {
            Result = true,
            Description = "Operation successful."
        });
    }

    public override async Task<MarkAsWatchedReply> MarkAsWatched(MarkAsWatchedRequest request,
        ServerCallContext context)
    {
        if (await _context.Watched.FindAsync(request.UserId, request.ShowId) != null)
            return await Task.FromResult(new MarkAsWatchedReply
            {
                Result = false,
                Description = "Show already marked as watched."
            });
        await _context.Watched.AddAsync(new Watched
        {
            ClientId = request.UserId,
            ShowId = request.ShowId
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(MarkAsWatched)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new MarkAsWatchedReply
        {
            Result = true,
            Description = "Operation successful."
        });
    }

    public override async Task<AddFundsReply> AddFunds(AddFundsRequest request, ServerCallContext context)
    {
        var client = await _context.Clients.FindAsync(request.UserId);
        if (client == null)
            return await Task.FromResult(new AddFundsReply
            {
                Result = false,
                Description = "Client ID not found."
            });
        client.Funds += request.Funds;
        _context.Clients.Update(client);
        await _context.Movements.AddAsync(new Movement
        {
            ClientId = client.Id,
            Description = $"Funds;{request.PaymentMethod}",
            Value = request.Funds
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(AddFunds)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new AddFundsReply
        {
            Result = true,
            Description = "Funds added successfully.",
            TotalFunds = client.Funds
        });
    }

    public override async Task<RefundReply> Refund(RefundRequest request, ServerCallContext context)
    {
        var reservation = await _context.Reservations.FindAsync(request.ReservationId);
        if (reservation == null)
            return await Task.FromResult(new RefundReply
            {
                Result = false,
                Description = "Reservation ID not found.",
                Funds = -1
            });
        var client = await _context.Clients.FindAsync(request.UserId);
        if (client == null)
            return await Task.FromResult(new RefundReply
            {
                Result = false,
                Description = "Client ID not found",
                Funds = -1
            });
        var session = await _context.Sessions.FindAsync(reservation.SessionId);
        session!.AvailableSeats += reservation.NoTickets;
        _context.Sessions.Update(session);
        client.Funds += reservation.Total;
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
        _context.Reservations.Remove(reservation);
        await _context.Movements.AddAsync(new Movement
        {
            ClientId = request.UserId,
            Description = "Refund",
            Value = reservation.Total
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(Refund)
        });
        await _context.SaveChangesAsync();
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
        var reservations = await _context.Reservations
            .Where(x => x.Id == request.UserId
                        && x.TimeOfPurchase <= endDate
                        && x.TimeOfPurchase >= startDate)
            .Include(x => x.Session).ThenInclude(x => x!.Show)
            .Include(x => x.Session).ThenInclude(x => x!.Theater)
            .ToListAsync();
        var json = JsonSerializer.Serialize(reservations);
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetReservations)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new GetReservationsReply
        {
            Reservations = json
        });
    }
}