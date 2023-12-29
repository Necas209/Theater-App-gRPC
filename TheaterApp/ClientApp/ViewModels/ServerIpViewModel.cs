using System.Net;
using Grpc.Core;
using Grpc.Net.Client;

namespace ClientApp.ViewModels;

public class ServerIpViewModel : BaseViewModel
{
    public string IpAddress { get; set; } = "127.0.0.1";

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public bool Connect()
    {
        if (string.IsNullOrWhiteSpace(IpAddress))
        {
            ShowError?.Invoke("Endereço IP em falta.");
            return false;
        }

        if (!IPAddress.TryParse(IpAddress, out var address))
        {
            ShowError?.Invoke("Endereço IP não é válido.");
            return false;
        }

        App.Channel = GrpcChannel.ForAddress(address.Equals(IPAddress.Loopback)
            ? "https://localhost:7046"
            : $"https://{address}:7046");
        if (App.Channel.State == ConnectivityState.TransientFailure)
        {
            ShowError?.Invoke("O canal gRPC não existe.");
            return false;
        }

        ShowMsg?.Invoke("Conexão ao servidor com sucesso");
        return true;
    }
}