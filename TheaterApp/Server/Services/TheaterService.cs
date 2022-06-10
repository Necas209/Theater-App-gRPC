using System.Text.Json;
using Grpc.Core;
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

    public override async Task<GetTheatersReply> GetTheaters(GetTheatersRequest request, ServerCallContext context)
    {
        var theaters = await _context.Theaters
            .Where(x => x.Name.Contains(request.Name) || x.Location.Contains(request.Location))
            .ToListAsync();
        var json = JsonSerializer.Serialize(theaters);
        return await Task.FromResult(new GetTheatersReply
        {
            Theaters = json
        });
    }

    public override async Task<GetShowsReply> GetShows(GetShowsRequest request, ServerCallContext context)
    {
        var shows = await _context.Shows
            .Where(x => x.Name.Contains(request.Name) || x.GenreId == request.GenreId)
            .ToListAsync();
        var json = JsonSerializer.Serialize(shows);
        return await Task.FromResult(new GetShowsReply
        {
            Shows = json
        });
    }

    public override async Task<GetGenresReply> GetGenres(GetGenresRequest request, ServerCallContext context)
    {
        var genres = await _context.Genres
            .ToListAsync();
        var json = JsonSerializer.Serialize(genres);
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
            .Where(x => x.Showtime >= startDate && x.Showtime <= endDate)
            .Include(x => x.Theater)
            .Where(x => x.Theater!.Name.Contains(request.TheaterName) ||
                        x.Theater!.Location.Contains(request.TheaterLocation))
            .Include(x => x.Show)
            .Where(x => x.Show!.Name.Contains(request.ShowName) || x.Show!.GenreId == request.GenreId)
            .ToListAsync();
        var json = JsonSerializer.Serialize(sessions);
        return await Task.FromResult(new GetSessionsReply
        {
            Sessions = json
        });
    }
}