using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
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

    public event StringMethod? ShowMsg;

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
        if (theaters?.Count != 0)
        {
            Theaters.Clear();
            theaters?.ForEach(show => Theaters.Add(show));
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
    }

    public async Task GetClientInfo()
    {
        var clientManager = new ClientManager.ClientManagerClient(App.Channel);
        var reply = await clientManager.GetClientInfoAsync(new GetClientInfoRequest
        {
            UserId = App.UserId
        });
        Client = JsonSerializer.Deserialize<Client>(reply.ClientInfo);
    }
}