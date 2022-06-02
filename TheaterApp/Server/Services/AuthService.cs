using Greet;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Services;

public class AuthService : AuthManager.AuthManagerBase
{
    private readonly ILogger<AuthService> _logger;
    private readonly TheaterDbContext _context;

    public AuthService(TheaterDbContext context, ILogger<AuthService> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public override async Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName 
                                                      && u.PasswordHash == request.PasswordHash);
        var loginStatus = user != null;
        return await Task.FromResult(new LoginReply
        {
            LoginStatus = loginStatus
        });
    }
}