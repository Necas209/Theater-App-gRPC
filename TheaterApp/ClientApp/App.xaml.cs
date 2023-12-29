using Grpc.Net.Client;
using GrpcLibrary.Models;

namespace ClientApp;

public partial class App
{
    public int UserId { get; set; }

    public User.UserType UserType { get; set; }

    public GrpcChannel? Channel { get; set; }
}