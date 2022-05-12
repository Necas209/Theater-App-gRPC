using System.Threading.Tasks;
using System.Windows;
using Grpc.Net.Client;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task Greet()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7046");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                new HelloRequest { Name = "GreeterClient" }
            );
            Message.Content = $"Greeting: {reply?.Message}";
        }

        private void BtSayHello_OnClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(async () => await Greet());
        }
    }
}