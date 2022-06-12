using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Theaters;

public class ShowsViewModel : BaseViewModel
{
    public ShowsViewModel()
    {
        IsManager = App.UserType == User.UserType.Manager;
        Shows = new ObservableCollection<Show>();
        Genres = new ObservableCollection<Genre>();
    }

    public ObservableCollection<Show> Shows { get; set; }

    public Show? Show { get; set; }

    public string Name { get; set; } = null!;

    public Genre? Genre { get; set; }

    public ObservableCollection<Genre> Genres { get; set; }

    public bool IsManager { get; set; }

    public event StringMethod? ShowError;

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

    public async Task GetShows()
    {
        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var reply = await client.GetShowsAsync(new GetShowsRequest
        {
            UserId = App.UserId,
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

    public async Task DelShow()
    {
        if (Show == null)
        {
            ShowError?.Invoke("Escolha um espetáculo primeiro.");
        }
        else
        {
            var client = new MgrManager.MgrManagerClient(App.Channel);
            var reply = await client.DelShowAsync(new DelShowRequest
            {
                UserId = App.UserId,
                Id = Show.Id
            });
            if (!reply.Result)
                ShowError?.Invoke(reply.Description);
        }
    }
}