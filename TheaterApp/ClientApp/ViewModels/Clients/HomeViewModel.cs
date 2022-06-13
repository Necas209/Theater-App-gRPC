using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Clients;

public class HomeViewModel : BaseViewModel
{
    private Client? _client;

    public HomeViewModel()
    {
        Theaters = new ObservableCollection<Theater>();
        Shows = new ObservableCollection<Show>();
        Genres = new ObservableCollection<Genre>();
        Sessions = new ObservableCollection<Session>();
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

    // Theaters Tab
    public ObservableCollection<Theater> Theaters { get; set; }

    public Theater? Theater { get; set; }

    public string? TheaterName { get; set; }

    public string? Location { get; set; }

    // Shows Tab
    public ObservableCollection<Show> Shows { get; set; }

    public ObservableCollection<Genre> Genres { get; set; }

    public Show? Show { get; set; }

    public string? ShowName { get; set; }

    public Genre? Genre { get; set; }

    // Sessions Tab
    public ObservableCollection<Session> Sessions { get; set; }

    public Session? Session { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public event StringMethod? ShowError;

    public async Task GetTheaters()
    {
        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var reply = await client.GetTheatersAsync(new GetTheatersRequest
        {
            UserId = App.UserId,
            Name = TheaterName,
            Location = Location
        });
        var theaters = JsonSerializer.Deserialize<List<Theater>>(reply.Theaters);
        if (theaters != null)
        {
            Theaters.Clear();
            Theater = null;
            theaters.ForEach(show => Theaters.Add(show));
        }
    }

    public async Task GetGenres()
    {
        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var reply = await client.GetGenresAsync(new GetGenresRequest
        {
            UserId = App.UserId
        });
        var genres = JsonSerializer.Deserialize<List<Genre>>(reply.Genres);
        genres?.ForEach(genre => Genres.Add(genre));
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
        }
        else
        {
            var client = new TheaterManager.TheaterManagerClient(App.Channel);
            var reply = await client.GetShowsAsync(new GetShowsRequest
            {
                UserId = App.UserId,
                TheaterId = Theater.Id
            });
            var shows = JsonSerializer.Deserialize<List<Show>>(reply.Shows);
            if (shows != null)
            {
                Shows.Clear();
                Show = null;
                shows.ForEach(show => Shows.Add(show));
            }
        }
    }

    public async Task GetShows()
    {
        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var request = new GetShowsRequest
        {
            UserId = App.UserId,
            Name = string.IsNullOrWhiteSpace(ShowName) ? null : ShowName
        };
        if (Genre != null && Genre.Id != 0)
            request.GenreId = Genre.Id;
        var reply = await client.GetShowsAsync(request);
        var shows = JsonSerializer.Deserialize<List<Show>>(reply.Shows);
        if (shows?.Count != 0)
        {
            Shows.Clear();
            shows?.ForEach(show => Shows.Add(show));
        }
    }

    public async Task GetSessions()
    {
        if (EndDate < StartDate)
        {
            ShowError?.Invoke("Data de início tem de ser anterior a data de fim");
        }
        else
        {
            var client = new TheaterManager.TheaterManagerClient(App.Channel);
            var request = new GetSessionsRequest
            {
                UserId = App.UserId,
                StartDate = Timestamp.FromDateTime(StartDate),
                EndDate = Timestamp.FromDateTime(EndDate)
            };
            if (Theater != null)
                request.TheaterId = Theater.Id;
            if (Show != null)
                request.ShowId = Show.Id;
            var reply = await client.GetSessionsAsync(request);
            var sessions = JsonSerializer.Deserialize<List<Session>>(reply.Sessions);
            Sessions.Clear();
            sessions?.ForEach(session => Sessions.Add(session));
        }
    }
}