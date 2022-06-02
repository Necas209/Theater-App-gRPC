using System.Text.Json;
using Greet;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Services;

public class ClientService : ClientManager.ClientManagerBase
{
    private readonly ILogger<AuthService> _logger;
    private readonly TheaterDbContext _context;

    public ClientService(TheaterDbContext context, ILogger<AuthService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task<GetReservationsReply> GetReservations(GetReservationsRequest request, ServerCallContext context)
    {
        var startDate = DateTime.Parse(request.StartDate);
        var endDate = DateTime.Parse(request.EndDate);
        var reservations = await _context.Reservations
            .Where(x => x.Id == request.UserId
                        && x.TimeOfPurchase <= endDate
                        && x.TimeOfPurchase >= startDate)
            .Include(x => x.Session).ThenInclude(x => x.Show)
            .Include(x => x.Session).ThenInclude(x => x.Theater)
            .ToListAsync();
        var json = JsonSerializer.Serialize(reservations);
        return await Task.FromResult(new GetReservationsReply
        {
            Reservations = json
        });
    }
}