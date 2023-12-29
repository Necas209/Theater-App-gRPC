using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Clients;

public class HomeViewModel : BaseViewModel
{
    private Client? _client;

    public Client? Client
    {
        get => _client;
        private set
        {
            _client = value;
            OnPropertyChanged();
        }
    }

    // Theaters Tab
    public ObservableCollection<Theater> Theaters { get; } = [];

    public Theater? Theater { get; set; }

    public string TheaterName { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    // Shows Tab
    public ObservableCollection<Show> Shows { get; } = [];

    public ObservableCollection<Genre> Genres { get; } = [];

    public Show? Show { get; set; }

    public string ShowName { get; set; } = string.Empty;

    public Genre? Genre { get; set; }

    // Sessions Tab
    public ObservableCollection<Session> Sessions { get; } = [];

    public Session? Session { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Today;

    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task GetTheaters()
    {
        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var request = new GetTheatersRequest
        {
            UserId = App.UserId
        };
        if (!string.IsNullOrWhiteSpace(TheaterName))
            request.Name = TheaterName;
        if (!string.IsNullOrWhiteSpace(Location))
            request.Location = Location;
        var reply = await client.GetTheatersAsync(request);
        var theaters = JsonSerializer.Deserialize<List<Theater>>(reply.Theaters);
        if (theaters == null) return;

        Theaters.Clear();
        Theater = null;
        foreach (var theater in theaters) Theaters.Add(theater);
    }

    public async Task GetGenres()
    {
        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var reply = await client.GetGenresAsync(new GetGenresRequest
        {
            UserId = App.UserId
        });
        var genres = JsonSerializer.Deserialize<List<Genre>>(reply.Genres);
        foreach (var genre in genres ?? Enumerable.Empty<Genre>()) Genres.Add(genre);
        Genres.Add(new Genre
        {
            Id = 0,
            Name = "Todos"
        });
    }

    public async Task GetClientInfo()
    {
        var client = new ClientManager.ClientManagerClient(App.Channel);
        var reply = await client.GetClientInfoAsync(new GetClientInfoRequest
        {
            UserId = App.UserId
        });
        Client = JsonSerializer.Deserialize<Client>(reply.ClientInfo);
    }

    public async Task GetShowsByTheater()
    {
        if (Theater == null)
        {
            ShowError?.Invoke("Selecione um teatro.");
            return;
        }

        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var reply = await client.GetShowsAsync(new GetShowsRequest
        {
            UserId = App.UserId,
            TheaterId = Theater.Id
        });
        var shows = JsonSerializer.Deserialize<List<Show>>(reply.Shows);
        if (shows == null) return;

        Shows.Clear();
        Show = null;
        foreach (var show in shows) Shows.Add(show);
    }

    public async Task GetShows()
    {
        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var request = new GetShowsRequest
        {
            UserId = App.UserId
        };
        if (!string.IsNullOrWhiteSpace(ShowName))
            request.Name = ShowName;
        if (Genre is { Id: not 0 })
            request.GenreId = Genre.Id;
        var reply = await client.GetShowsAsync(request);
        var shows = JsonSerializer.Deserialize<List<Show>>(reply.Shows);
        if (shows is not { Count: not 0 }) return;

        Shows.Clear();
        foreach (var show in shows) Shows.Add(show);
    }

    public async Task GetSessions()
    {
        if (EndDate < StartDate)
        {
            ShowError?.Invoke("Data de início tem de ser anterior a data de fim");
            return;
        }

        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var request = new GetSessionsRequest
        {
            UserId = App.UserId,
            StartDate = Timestamp.FromDateTime(StartDate.ToUniversalTime()),
            EndDate = Timestamp.FromDateTime(EndDate.ToUniversalTime())
        };
        if (Theater != null)
            request.TheaterId = Theater.Id;
        if (Show != null)
            request.ShowId = Show.Id;
        var reply = await client.GetSessionsAsync(request);
        var sessions = JsonSerializer.Deserialize<List<Session>>(reply.Sessions);
        if (sessions == null) return;

        Sessions.Clear();
        foreach (var session in sessions) Sessions.Add(session);
    }

    public async Task MarkAsWatched()
    {
        if (Show == null)
        {
            ShowError?.Invoke("Selecione um espetáculo primeiro.");
            return;
        }

        var client = new ClientManager.ClientManagerClient(App.Channel);
        var reply = await client.MarkAsWatchedAsync(new MarkAsWatchedRequest
        {
            UserId = App.UserId,
            ShowId = Show.Id
        });
        var eventCalled = reply.Result ? ShowMsg : ShowError;
        eventCalled?.Invoke(reply.Description);
    }
}