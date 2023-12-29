using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Theaters;

public class ShowsViewModel : BaseViewModel
{
    public ShowsViewModel()
    {
        IsManager = App.UserType == User.UserType.Manager;
    }

    public ObservableCollection<Show> Shows { get; } = [];

    public Show? Show { get; set; }

    public string Name { get; set; } = string.Empty;

    public Genre? Genre { get; set; }

    public ObservableCollection<Genre> Genres { get; } = [];

    public bool IsManager { get; set; }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

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

    public async Task GetShows()
    {
        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var request = new GetShowsRequest
        {
            UserId = App.UserId
        };
        if (!string.IsNullOrWhiteSpace(Name))
            request.Name = Name;
        if (Genre is { Id: not 0 })
            request.GenreId = Genre.Id;
        var reply = await client.GetShowsAsync(request);
        var shows = JsonSerializer.Deserialize<List<Show>>(reply.Shows);
        if (shows == null) return;

        Show = null;
        Shows.Clear();
        shows.ForEach(show => Shows.Add(show));
    }

    public async Task DelShow()
    {
        if (Show == null)
        {
            ShowError?.Invoke("Escolha um espetáculo primeiro.");
            return;
        }

        var client = new MgrManager.MgrManagerClient(App.Channel);
        var reply = await client.DelShowAsync(new DelShowRequest
        {
            UserId = App.UserId,
            Id = Show.Id
        });
        if (!reply.Result)
        {
            ShowError?.Invoke(reply.Description);
            return;
        }

        ShowMsg?.Invoke(reply.Description);
        Shows.Remove(Show);
        Show = null;
    }
}