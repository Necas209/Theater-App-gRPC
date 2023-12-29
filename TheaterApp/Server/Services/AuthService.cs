using Grpc.Core;
using GrpcLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Services;

public class AuthService(TheaterDbContext dbContext) : AuthManager.AuthManagerBase
{
    public override async Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(u =>
            u.UserName == request.UserName && u.PasswordHash == request.PasswordHash);
        if (user == null)
            return await Task.FromResult(new LoginReply
            {
                LoginStatus = false
            });
        var userType = await FindUserTypeAsync(user.Id);
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = user.Id,
            Message = nameof(Login)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new LoginReply
        {
            LoginStatus = true,
            UserId = user.Id,
            UserType = (int)userType
        });
    }

    public override async Task<GetUserInfoReply> GetUserInfo(GetUserInfoRequest request, ServerCallContext context)
    {
        var user = await dbContext.Users.FindAsync(request.UserId);
        if (user == null)
            return await Task.FromResult(new GetUserInfoReply
            {
                UserExists = false
            });
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(GetUserInfo)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new GetUserInfoReply
        {
            UserName = user.UserName,
            Name = user.Name,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            UserExists = true
        });
    }

    public override async Task<UpdUserInfoReply> UpdUserInfo(UpdUserInfoRequest request, ServerCallContext context)
    {
        var user = await dbContext.Users.FindAsync(request.UserId);
        if (user == null)
            return await Task.FromResult(new UpdUserInfoReply
            {
                Result = false,
                Description = "User ID not found."
            });
        {
            user.Id = request.UserId;
            user.Name = request.Name;
            user.Email = request.Email;
            user.UserName = request.UserName;
            user.PasswordHash = request.PasswordHash;
        }
        dbContext.Users.Update(user);
        await dbContext.Logs.AddAsync(new Log
        {
            UserId = request.UserId,
            Message = nameof(UpdUserInfo)
        });
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new UpdUserInfoReply
        {
            Result = true,
            Description = "User information updated successfully."
        });
    }

    public override async Task<RegisterReply> Register(RegisterRequest request, ServerCallContext context)
    {
        if (await dbContext.Users.AnyAsync(x => x.UserName == request.UserName))
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
        user.Logs.Add(new Log
        {
            Message = nameof(Register)
        });
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return await Task.FromResult(new RegisterReply
        {
            Result = true,
            Description = "Registration was successful."
        });
    }

    private async Task<User.UserType> FindUserTypeAsync(int userId)
    {
        if (await dbContext.Clients.FindAsync(userId) != null)
            return User.UserType.Client;
        return await dbContext.Admins.FindAsync(userId) != null
            ? User.UserType.Admin
            : User.UserType.Manager;
    }
}