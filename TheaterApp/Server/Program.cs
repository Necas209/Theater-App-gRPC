using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddDbContext<TheaterDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TheaterDbConnection")
    )
);

var app = builder.Build();

app.MapGrpcService<AuthService>();
app.MapGrpcService<ClientService>();
app.MapGrpcService<MgrService>();
app.MapGrpcService<AdminService>();
app.MapGrpcService<TheaterService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run();