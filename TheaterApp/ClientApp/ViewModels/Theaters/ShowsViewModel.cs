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
        Name = "";
        IsManager = App.UserType == User.UserType.Manager;
        Shows = new ObservableCollection<Show>();
        Genres = new ObservableCollection<Genre>();
    }

    public ObservableCollection<Show> Shows { get; }

    public Show? Show { get; set; }

    public string Name { get; set; }

    public Genre? Genre { get; set; }

    public ObservableCollection<Genre> Genres { get; }

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
        genres?.ForEach(genre => Genres.Add(genre));
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
            {
                ShowError?.Invoke(reply.Description);
            }
            else
            {
                ShowMsg?.Invoke(reply.Description);
                Shows.Remove(Show);
                Show = null;
            }
        }
    }
}