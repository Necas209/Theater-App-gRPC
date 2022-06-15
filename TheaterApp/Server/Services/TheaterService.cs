using System.Text.Json;
using System.Text.Json.Serialization;
using Grpc.Core;
using GrpcLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Services;

public class TheaterService : TheaterManager.TheaterManagerBase
{
    private readonly TheaterDbContext _context;

    public TheaterService(TheaterDbContext context)
    {
        _context = context;
    }

    public override async Task<GetSessionReply> GetSession(GetSessionRequest request, ServerCallContext context)
    {
        var session = await _context.Sessions
            .Where(x => x.Id == request.Id)
            .Include(x => x.Show).ThenInclude(x => x!.Genre)
            .Include(x => x.Theater)
            .FirstOrDefaultAsync();
        if (session == null)
            return await Task.FromResult(new GetSessionReply
            {
                Result = false,
                Description = "Session ID not found."
            });
        var json = JsonSerializer.Serialize(session, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetSession)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new GetSessionReply
        {
            Result = true,
            Description = "Session found.",
            Session = json
        });
    }

    public override async Task<GetTheatersReply> GetTheaters(GetTheatersRequest request, ServerCallContext context)
    {
        var theaters = await _context.Theaters
            .Where(x => (!request.HasName || x.Name.Contains(request.Name)) &&
                        (!request.HasLocation || x.Location.Contains(request.Location)))
            .ToListAsync();
        var json = JsonSerializer.Serialize(theaters, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetTheaters)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new GetTheatersReply
        {
            Theaters = json
        });
    }

    public override async Task<GetShowsReply> GetShows(GetShowsRequest request, ServerCallContext context)
    {
        List<Show> shows;
        if (request.HasTheaterId)
            shows = await _context.Shows
                .Join(_context.Sessions.Where(x => x.TheaterId == request.TheaterId), show => show.Id,
                    session => session.ShowId, (show, session) => new { show })
                .Select(x => x.show)
                .ToListAsync();
        else
            shows = await _context.Shows
                .Where(x => (!request.HasName || x.Name.Contains(request.Name)) &&
                            (!request.HasGenreId || x.GenreId == request.GenreId))
                .ToListAsync();
        var json = JsonSerializer.Serialize(shows, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetShows)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new GetShowsReply
        {
            Shows = json
        });
    }

    public override async Task<GetGenresReply> GetGenres(GetGenresRequest request, ServerCallContext context)
    {
        var genres = await _context.Genres.ToListAsync();
        var json = JsonSerializer.Serialize(genres);
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetGenres)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new GetGenresReply
        {
            Genres = json
        });
    }

    public override async Task<GetSessionsReply> GetSessions(GetSessionsRequest request, ServerCallContext context)
    {
        var startDate = request.StartDate.ToDateTime();
        var endDate = request.EndDate.ToDateTime();
        var sessions = await _context.Sessions
            .Where(x => x.Showtime >= startDate && x.Showtime.Date <= endDate &&
                        (!request.HasShowId || x.ShowId == request.ShowId) &&
                        (!request.HasTheaterId || x.TheaterId == request.TheaterId))
            .Include(x => x.Theater)
            .Include(x => x.Show)
            .ToListAsync();
        var json = JsonSerializer.Serialize(sessions, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetSessions)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new GetSessionsReply
        {
            Sessions = json
        });
    }
}