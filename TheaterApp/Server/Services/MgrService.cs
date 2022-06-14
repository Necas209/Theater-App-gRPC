using Grpc.Core;
using GrpcLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Services;

public class MgrService : MgrManager.MgrManagerBase
{
    private readonly TheaterDbContext _context;

    public MgrService(TheaterDbContext context)
    {
        _context = context;
    }

    public override async Task<AddTheaterReply> AddTheater(AddTheaterRequest request, ServerCallContext context)
    {
        if (await _context.Theaters.AnyAsync(x => x.Name == request.Name))
            return await Task.FromResult(new AddTheaterReply
            {
                Result = false,
                Description = "Theater with given name already exists."
            });
        await _context.Theaters.AddAsync(new Theater
        {
            Name = request.Name,
            Location = request.Location,
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(AddTheater)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new AddTheaterReply
        {
            Result = true,
            Description = "Theater added successfully."
        });
    }

    public override async Task<EditTheaterReply> EditTheater(EditTheaterRequest request, ServerCallContext context)
    {
        if (await _context.Theaters.FindAsync(request.Id) == null)
            return await Task.FromResult(new EditTheaterReply
            {
                Result = false,
                Description = "Theater ID not found."
            });
        _context.Theaters.Update(new Theater
        {
            Id = request.Id,
            Name = request.Name,
            Location = request.Location,
            Address = request.Address,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(EditTheater)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new EditTheaterReply
        {
            Result = true,
            Description = "Theater updated successfully."
        });
    }

    public override async Task<DelTheaterReply> DelTheater(DelTheaterRequest request, ServerCallContext context)
    {
        if (await _context.Sessions.AnyAsync(x => x.TheaterId == request.Id))
            return await Task.FromResult(new DelTheaterReply
            {
                Result = false,
                Description = "Theater already has sessions."
            });
        var theater = await _context.Theaters.FindAsync(request.Id);
        if (theater == null)
            return await Task.FromResult(new DelTheaterReply
            {
                Result = false,
                Description = "Theater ID not found."
            });
        _context.Theaters.Remove(theater);
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(DelTheater)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new DelTheaterReply
        {
            Result = true,
            Description = "Theater removed successfully."
        });
    }

    public override async Task<AddShowReply> AddShow(AddShowRequest request, ServerCallContext context)
    {
        if (await _context.Shows.AnyAsync(x => x.Name == request.Name))
            return await Task.FromResult(new AddShowReply
            {
                Result = false,
                Description = "Show with given name already exists."
            });
        await _context.Shows.AddAsync(new Show
        {
            Name = request.Name,
            Synopsis = request.Synopsis,
            Length = request.Length.ToTimeSpan(),
            GenreId = request.GenreId
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(AddShow)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new AddShowReply
        {
            Result = true,
            Description = "Show added successfully."
        });
    }

    public override async Task<EditShowReply> EditShow(EditShowRequest request, ServerCallContext context)
    {
        if (await _context.Shows.FindAsync(request.Id) == null)
            return await Task.FromResult(new EditShowReply
            {
                Result = false,
                Description = "Show ID not found."
            });
        _context.Shows.Update(new Show
        {
            Id = request.Id,
            Name = request.Name,
            Synopsis = request.Synopsis,
            Length = request.Length.ToTimeSpan(),
            GenreId = request.GenreId
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(EditShow)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new EditShowReply
        {
            Result = true,
            Description = "Show updated successfully."
        });
    }

    public override async Task<DelShowReply> DelShow(DelShowRequest request, ServerCallContext context)
    {
        if (await _context.Sessions.AnyAsync(x => x.ShowId == request.Id))
            return await Task.FromResult(new DelShowReply
            {
                Result = false,
                Description = "Show already has sessions."
            });
        var show = await _context.Shows.FindAsync(request.Id);
        if (show == null)
            return await Task.FromResult(new DelShowReply
            {
                Result = false,
                Description = "Show ID not found."
            });
        _context.Shows.Remove(show);
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(DelShow)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new DelShowReply
        {
            Result = true,
            Description = "Show removed successfully."
        });
    }

    public override async Task<AddSessionReply> AddSession(AddSessionRequest request, ServerCallContext context)
    {
        if (await _context.Sessions.AnyAsync(x =>
                x.ShowId == request.ShowId && x.TheaterId == request.TheaterId &&
                x.Showtime == request.Showtime.ToDateTime()))
            return await Task.FromResult(new AddSessionReply
            {
                Result = false,
                Description = "Session already exists."
            });
        await _context.Sessions.AddAsync(new Session
        {
            ShowId = request.ShowId,
            TheaterId = request.TheaterId,
            Showtime = request.Showtime.ToDateTime(),
            TotalSeats = request.TotalSeats,
            AvailableSeats = request.TotalSeats,
            TicketPrice = request.TicketPrice
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(AddSession)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new AddSessionReply
        {
            Result = true,
            Description = "Session added successfully."
        });
    }

    public override async Task<DelSessionReply> DelSession(DelSessionRequest request, ServerCallContext context)
    {
        if (await _context.Reservations.AnyAsync(x => x.SessionId == request.Id))
            return await Task.FromResult(new DelSessionReply
            {
                Result = false,
                Description = "Session already reserved."
            });
        var session = await _context.Sessions.FindAsync(request.Id);
        if (session == null)
            return await Task.FromResult(new DelSessionReply
            {
                Result = false,
                Description = "Session ID not found."
            });
        _context.Sessions.Remove(session);
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(DelSession)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new DelSessionReply
        {
            Result = true,
            Description = "Session removed successfully."
        });
    }
}