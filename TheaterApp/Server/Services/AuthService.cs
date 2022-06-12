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

    public override async Task<GetUserInfoReply> GetUserInfo(GetUserInfoRequest request, ServerCallContext context)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        if (user != null)
            return await Task.FromResult(new GetUserInfoReply
            {
                UserName = user.UserName,
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                UserExists = true
            });

        return await Task.FromResult(new GetUserInfoReply
        {
            UserExists = false
        });
    }

    public override async Task<UpdUserInfoReply> UpdUserInfo(UpdUserInfoRequest request, ServerCallContext context)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
            return await Task.FromResult(new UpdUserInfoReply
            {
                Result = false,
                Description = "User ID not found."
            });
        user.Name = request.Name;
        user.Email = request.Email;
        user.UserName = request.UserName;
        user.PasswordHash = request.PasswordHash;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return await Task.FromResult(new UpdUserInfoReply
        {
            Result = true,
            Description = "User information updated successfully."
        });
    }
    
    public override async Task<RegisterReply> Register(RegisterRequest request, ServerCallContext context)
    {
        if (await _context.Users.AnyAsync(x => x.UserName == request.UserName))
            return await Task.FromResult(new RegisterReply
            {
                Result = false,
                Description = "UserName already taken."
            });
        var user = new User
        {
            Name = request.Name,
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = request.PasswordHash,
            Client = new Client()
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return await Task.FromResult(new RegisterReply
        {
            Result = true,
            Description = "Registration was successful."
        });
    }

    private async Task<User.UserType> FindUserTypeAsync(int userId)
    {
        if (await _context.Clients.FindAsync(userId) != null)
            return User.UserType.Client;
        return await _context.Managers.FindAsync(userId) != null ? User.UserType.Manager : User.UserType.Admin;
    }
}