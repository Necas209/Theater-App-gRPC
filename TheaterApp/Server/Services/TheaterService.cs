using System.Text.Json;
using Grpc.Core;
using GrpcLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Services;

public class TheaterService(TheaterDbContext dbContext) : TheaterManager.TheaterManagerBase
{
    public override async Task<GetSessionReply> GetSession(GetSessionRequest request, ServerCallContext context)
    {
        var session = await dbContext.Sessions
            .Include(x => x.Show).ThenInclude(x => x!.Genre)
            .Include(x => x.Theater)
            .SingleOrDefaultAsync(x => x.Id == request.Id);
        if (session == null)
            return await Task.FromResult(new GetSessionReply
            {
                Result = false,
                Description = "Session ID not found."
            });
        var json = JsonSerializer.Serialize(session);
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetSession)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new GetSessionReply
        {
            Result = true,
            Description = "Session found.",
            Session = json
        });
    }

    public override async Task<GetTheatersReply> GetTheaters(GetTheatersRequest request, ServerCallContext context)
    {
        var theaters = await dbContext.Theaters
            .Where(x => (!request.HasName || x.Name.Contains(request.Name)) &&
                        (!request.HasLocation || x.Location.Contains(request.Location)))
            .ToListAsync();
        var json = JsonSerializer.Serialize(theaters);
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetTheaters)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new GetTheatersReply
        {
            Theaters = json
        });
    }

    public override async Task<GetShowsReply> GetShows(GetShowsRequest request, ServerCallContext context)
    {
        List<Show> shows;
        if (request.HasTheaterId)
            shows = await dbContext.Shows
                .Join(dbContext.Sessions.Where(x => x.TheaterId == request.TheaterId), show => show.Id,
                    session => session.ShowId, (show, session) => new { show })
                .Select(x => x.show)
                .Include(x => x.Genre)
                .Distinct()
                .ToListAsync();
        else
            shows = await dbContext.Shows
                .Where(x => (!request.HasName || x.Name.Contains(request.Name)) &&
                            (!request.HasGenreId || x.GenreId == request.GenreId))
                .Include(x => x.Genre)
                .ToListAsync();
        var json = JsonSerializer.Serialize(shows);
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetShows)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new GetShowsReply
        {
            Shows = json
        });
    }

    public override async Task<GetGenresReply> GetGenres(GetGenresRequest request, ServerCallContext context)
    {
        var genres = await dbContext.Genres.ToListAsync();
        var json = JsonSerializer.Serialize(genres);
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetGenres)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new GetGenresReply
        {
            Genres = json
        });
    }

    public override async Task<GetSessionsReply> GetSessions(GetSessionsRequest request, ServerCallContext context)
    {
        var startDate = request.StartDate.ToDateTime();
        var endDate = request.EndDate.ToDateTime();
        List<Session> sessions;
        if (request.HasShowId)
            sessions = await dbContext.Sessions
                .Where(x => x.ShowId == request.ShowId && x.TheaterId == request.TheaterId)
                .Include(x => x.Theater)
                .Include(x => x.Show)
                .ToListAsync();
        else
            sessions = await dbContext.Sessions
                .Where(x => x.Showtime >= startDate && x.Showtime.Date <= endDate)
                .Include(x => x.Theater)
                .Include(x => x.Show)
                .ToListAsync();
        var json = JsonSerializer.Serialize(sessions);
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetSessions)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new GetSessionsReply
        {
            Sessions = json
        });
    }
}