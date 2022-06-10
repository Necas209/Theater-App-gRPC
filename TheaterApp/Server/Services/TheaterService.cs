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
            .Where(x => x.Name.Contains(request.Name) && x.GenreId == request.GenreId)
            .ToListAsync();
        var json = JsonSerializer.Serialize(shows);
        return await Task.FromResult(new GetShowsReply
        {
            Shows = json
        });
    }
}