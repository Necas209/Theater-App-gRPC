using Grpc.Core;
using GrpcLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Services;

public class AuthService : AuthManager.AuthManagerBase
{
    private readonly TheaterDbContext _context;
    
    public AuthService(TheaterDbContext context)
    {
        _context = context;
    }

    public override async Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == request.UserName && u.PasswordHash == request.PasswordHash);
        if (user == null)
            return await Task.FromResult(new LoginReply
            {
                LoginStatus = user != null
            });
        var userType = await FindUserTypeAsync(user.Id);
        return await Task.FromResult(new LoginReply
        {
            LoginStatus = true,
            UserId = user.Id,
            UserType = (int)userType
        });
    }

    private async Task<User.UserType> FindUserTypeAsync(int userId)
    {
        if (await _context.Clients.FindAsync(userId) != null)
            return User.UserType.Client;
        return await _context.Managers.FindAsync(userId) != null ? User.UserType.Manager : User.UserType.Admin;
    }
}