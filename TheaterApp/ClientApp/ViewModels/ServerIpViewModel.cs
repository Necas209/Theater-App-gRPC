using System.Net;
using Grpc.Core;
using Grpc.Net.Client;

namespace ClientApp.ViewModels;

public class ServerIpViewModel : BaseViewModel
{
    public ServerIpViewModel()
    {
        IpAddress = "127.0.0.1";
    }

    public string IpAddress { get; set; }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public bool Connect()
    {
        if (string.IsNullOrWhiteSpace(IpAddress))
        {
            ShowError?.Invoke("Endereço IP em falta.");
        }
        else if (!IPAddress.TryParse(IpAddress, out var address))
        {
            ShowError?.Invoke("Endereço IP não é válido.");
        }
        else
        {
            App.Channel = GrpcChannel.ForAddress(address.Equals(IPAddress.Loopback)
                ? "https://localhost:7046"
                : $"https://{address}:7046");
            if (App.Channel.State == ConnectivityState.Ready)
            {
                ShowMsg?.Invoke("Conexão ao servidor com sucesso");
                return true;
            }

            ShowError?.Invoke("O canal gRPC não existe.");
        }

        return false;
    }
}