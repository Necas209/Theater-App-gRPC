using System.Net;
using System.Threading.Tasks;
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

    public async Task<bool> Connect()
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
            if (await Task.FromResult(true))
            {
                ShowMsg?.Invoke("Conexão ao servidor com sucesso");
                return true;
            }

            ShowError?.Invoke("O canal gRPC não existe.");
        }

        return false;
    }

    private async Task<bool> IsReadyAsync()
    {
        try
        {
            await App.Channel!.ConnectAsync();
        }
        catch (TaskCanceledException)
        {
            return false;
        }

        return App.Channel.State == ConnectivityState.Ready;
    }
}