using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Theaters;

public class ShowsViewModel : BaseViewModel
{
    private bool _isManager;

    public ShowsViewModel()
    {
        Name = "";
        Shows = new ObservableCollection<Show>();
        Genres = new ObservableCollection<Genre>();
    }

    public ObservableCollection<Show> Shows { get; set; }

    public Show? Show { get; set; }

    public string Name { get; set; }

    public Genre? Genre { get; set; }

    public ObservableCollection<Genre> Genres { get; set; }
    
    public bool IsManager
    {
        get => _isManager;
        set
        {
            _isManager = value;
            OnPropertyChanged(nameof(IsManager));
        }
    }

    public event StringMethod? ShowError;

    public async Task GetGenres(App app)
    {
        var client = new TheaterManager.TheaterManagerClient(app.Channel);
        var reply = await client.GetGenresAsync(new GetGenresRequest
        {
            UserId = app.UserId
        });
        var genres = JsonSerializer.Deserialize<List<Genre>>(reply.Genres);
        genres?.ForEach(genre => Genres.Add(genre));
    }
    
    public async Task GetShows(App app)
    {
        var client = new TheaterManager.TheaterManagerClient(app.Channel);
        var reply = await client.GetShowsAsync(new GetShowsRequest
        {
            Name = Name,
            GenreId = Genre?.Id ?? -1
        });
        var shows = JsonSerializer.Deserialize<List<Show>>(reply.Shows);
        if (shows?.Count != 0)
        {
            Shows.Clear();
            shows?.ForEach(show => Shows.Add(show));
        }
    }
    
    public async Task DelShow(App app)
    {
        if (Show == null)
        {
            ShowError?.Invoke("Escolha um espetáculo primeiro.");
        }
        else
        {
            var client = new MgrManager.MgrManagerClient(app.Channel);
            var reply = await client.DelShowAsync(new DelShowRequest
            {
                UserId = app.UserId,
                Id = Show.Id
            });
            if (!reply.Result)
                ShowError?.Invoke(reply.Description);
        }
    }
}