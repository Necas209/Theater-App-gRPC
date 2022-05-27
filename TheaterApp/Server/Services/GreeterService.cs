using Microsoft.EntityFrameworkCore;
using Greet;
using Grpc.Core;
using Server.Data;

namespace Server.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly TheaterDbContext _context;

    public GreeterService(TheaterDbContext context, ILogger<GreeterService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Message
        });
    }
    
    public override Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == request.UserName 
                                                      && u.PasswordHash == request.PasswordHash);
        var loginStatus = user != null;
        return Task.FromResult(new LoginReply
        {
            LoginStatus = loginStatus
        });
    }
}