using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Clients;

public class BuyTicketsViewModel : BaseViewModel
{
    private Client? _client;
    private int _noTickets;
    private Session? _session;
    private decimal _total;

    public BuyTicketsViewModel()
    {
        Tickets = Enumerable.Range(1, 6);
    }

    public Client? Client
    {
        get => _client;
        set
        {
            _client = value;
            OnPropertyChanged(nameof(Client));
        }
    }

    public Session? Session
    {
        get => _session;
        set
        {
            _session = value;
            OnPropertyChanged(nameof(Session));
        }
    }

    public int NoTickets
    {
        get => _noTickets;
        set
        {
            _noTickets = value;
            OnPropertyChanged(nameof(NoTickets));
            OnPropertyChanged(nameof(Total));
        }
    }

    public IEnumerable<int> Tickets { get; set; }

    public decimal Total
    {
        get
        {
            _total = NoTickets * Session!.TicketPrice;
            return _total;
        }
        set
        {
            _total = value;
            OnPropertyChanged(nameof(Total));
        }
    }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task GetClientInfo()
    {
        var clientManager = new ClientManager.ClientManagerClient(App.Channel);
        var reply = await clientManager.GetClientInfoAsync(new GetClientInfoRequest
        {
            UserId = App.UserId
        });
        Client = JsonSerializer.Deserialize<Client>(reply.ClientInfo);
    }

    public async Task GetSession(int sessionId)
    {
        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var reply = await client.GetSessionAsync(new GetSessionRequest
        {
            Id = sessionId
        });
        if (!reply.Result)
            ShowError?.Invoke(reply.Description);
        else
            Session = JsonSerializer.Deserialize<Session>(reply.Session);
    }

    public async Task ButTickets()
    {
        if (NoTickets > Session!.AvailableSeats)
        {
            ShowError?.Invoke($"Só há {Session!.AvailableSeats} lugares disponíveis.");
        }
        else if (Total > Client!.Funds)
        {
            ShowError?.Invoke("Não tem fundos suficientes.");
        }
        else
        {
            var client = new ClientManager.ClientManagerClient(App.Channel);
            var reply = await client.BuyTicketsAsync(new BuyTicketsRequest
            {
                ClientId = App.UserId,
                NoTickets = NoTickets,
                TimeOfPurchase = Timestamp.FromDateTime(DateTime.Now),
                SessionId = Session!.Id
            });
            if (!reply.Result)
                ShowError?.Invoke(reply.Description);
            else
                ShowMsg?.Invoke(reply.Description);
        }
    }
}