using Grpc.Net.Client;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public readonly GrpcChannel Channel;
        public int UserId { get; set; }

        public App()
        {
            Channel = GrpcChannel.ForAddress("https://localhost:7046");
        }
    }
}