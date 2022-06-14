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
        switch ((User.UserType)request.UserType)
        {
            case User.UserType.Admin:
                user.Admin = new Admin();
                break;
            case User.UserType.Manager:
                user.Manager = new Manager();
                break;
            case User.UserType.Client:
                return await Task.FromResult(new AddUserReply
                {
                    Result = false,
                    Description = "Client type not available."
                });
            default:
                return await Task.FromResult(new AddUserReply
                {
                    Result = false,
                    Description = "Unknown user type."
                });
        }

        await _context.Users.AddAsync(user);
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(AddUser)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new AddUserReply
        {
            Result = true,
            Description = $"{Enum.GetName(typeof(User.UserType), request.UserType)} added successfully."
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
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetPurchases)
        });
        await _context.SaveChangesAsync();
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
        await _context.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetLogs)
        });
        await _context.SaveChangesAsync();
        return await Task.FromResult(new GetLogsReply
        {
            Logs = json
        });
    }
}