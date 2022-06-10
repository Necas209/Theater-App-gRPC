using System.Text.Json;
using Grpc.Core;
using GrpcLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Services;

public class AdminService : AdminManager.AdminManagerBase
{
    private readonly TheaterDbContext _context;

    public AdminService(TheaterDbContext context)
    {
        _context = context;
    }

    public override async Task<AddUserReply> AddUser(AddUserRequest request, ServerCallContext context)
    {
        if (await _context.Users.AnyAsync(x => x.UserName == request.UserName))
            return await Task.FromResult(new AddUserReply
            {
                Result = false,
                Description = "UserName already taken."
            });
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            UserName = request.UserName,
            PasswordHash = request.PasswordHash
        };
        switch (request.UserType)
        {
            case "Admin":
                user.Admin = new Admin();
                break;
            case "Manager":
                user.Manager = new Manager();
                break;
        }

        await _context.Users.AddAsync(user);
        return await Task.FromResult(new AddUserReply
        {
            Result = true,
            Description = $"{request.UserType} added successfully."
        });
    }

    public override async Task<GetPurchasesReply> GetPurchases(GetPurchasesRequest request, ServerCallContext context)
    {
        var startDate = request.StartDate.ToDateTime();
        var endDate = request.EndDate.ToDateTime();
        var purchases = await _context.Reservations
            .Where(x => x.TimeOfPurchase >= startDate && x.TimeOfPurchase <= endDate)
            .Include(x => x.Session).ThenInclude(x => x!.Theater)
            .Include(x => x.Session).ThenInclude(x => x!.Show)
            .ToListAsync();
        var json = JsonSerializer.Serialize(purchases);
        return await Task.FromResult(new GetPurchasesReply
        {
            Purchases = json
        });
    }

    public override async Task<GetLogsReply> GetLogs(GetLogsRequest request, ServerCallContext context)
    {
        var startDate = request.StartDate.ToDateTime();
        var endDate = request.EndDate.ToDateTime();
        var logs = await _context.Logs
            .Where(x => x.Stamp >= startDate && x.Stamp <= endDate)
            .Include(x => x.User)
            .ToListAsync();
        var json = JsonSerializer.Serialize(logs);
        return await Task.FromResult(new GetLogsReply
        {
            Logs = json
        });
    }
}