using Grpc.Net.Client;
using GrpcLibrary.Models;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public readonly GrpcChannel Channel;
        public int UserId { get; set; }

        public User.UserType UserType { get; set; }
        
        public App()
        {
            Channel = GrpcChannel.ForAddress("https://localhost:7046");
        }
    }
}