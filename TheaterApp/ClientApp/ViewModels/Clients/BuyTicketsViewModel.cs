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

    public Client? Client
    {
        get => _client;
        private set
        {
            _client = value;
            OnPropertyChanged();
        }
    }

    public Session? Session
    {
        get => _session;
        private set
        {
            _session = value;
            OnPropertyChanged();
        }
    }

    public int NoTickets
    {
        get => _noTickets;
        set
        {
            _noTickets = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Total));
        }
    }

    public IEnumerable<int> Tickets { get; } = Enumerable.Range(1, 6);

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
            OnPropertyChanged();
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
            UserId = App.UserId,
            Id = sessionId
        });
        if (!reply.Result)
        {
            ShowError?.Invoke(reply.Description);
            return;
        }

        Session = JsonSerializer.Deserialize<Session>(reply.Session);
    }

    public async Task ButTickets()
    {
        if (NoTickets > Session!.AvailableSeats)
        {
            ShowError?.Invoke($"Só há {Session!.AvailableSeats} lugares disponíveis.");
            return;
        }

        if (Total > Client!.Funds)
        {
            ShowError?.Invoke("Não tem fundos suficientes.");
            return;
        }

        var client = new ClientManager.ClientManagerClient(App.Channel);
        var reply = await client.BuyTicketsAsync(new BuyTicketsRequest
        {
            ClientId = App.UserId,
            NoTickets = NoTickets,
            TimeOfPurchase = Timestamp.FromDateTime(DateTime.UtcNow),
            SessionId = Session!.Id
        });
        var eventCalled = reply.Result ? ShowMsg : ShowError;
        eventCalled?.Invoke(reply.Description);
    }
}