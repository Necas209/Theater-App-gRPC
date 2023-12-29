using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Managers;

public class AddShowViewModel : BaseViewModel
{
    public string Name { get; set; } = string.Empty;

    public string Synopsis { get; set; } = string.Empty;

    public TimeSpan Length { get; set; }

    public ObservableCollection<Genre> Genres { get; } = [];

    public Genre? Genre { get; set; }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task GetGenres(App app)
    {
        var client = new TheaterManager.TheaterManagerClient(app.Channel);
        var reply = await client.GetGenresAsync(new GetGenresRequest
        {
            UserId = app.UserId
        });
        var genres = JsonSerializer.Deserialize<List<Genre>>(reply.Genres);
        foreach (var genre in genres ?? Enumerable.Empty<Genre>()) Genres.Add(genre);
    }

    public async Task AddShow(App app)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            ShowError?.Invoke("Nome em falta.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Synopsis))
        {
            ShowError?.Invoke("Sinopse em falta.");
            return;
        }

        if (Length <= TimeSpan.Zero)
        {
            ShowError?.Invoke($"Duração deverá ser superior a {TimeSpan.Zero}");
            return;
        }

        if (Genre == null)
        {
            ShowError?.Invoke("Deverá selecionar um género.");
            return;
        }

        var client = new MgrManager.MgrManagerClient(app.Channel);
        var reply = await client.AddShowAsync(new AddShowRequest
        {
            UserId = app.UserId,
            Name = Name,
            Synopsis = Synopsis,
            Length = Duration.FromTimeSpan(Length),
            GenreId = Genre.Id
        });
        var eventHandler = reply.Result ? ShowMsg : ShowError;
        eventHandler?.Invoke(reply.Description);
    }
}