using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Managers;

public class EditShowViewModel : BaseViewModel
{
    private Genre? _genre;
    private string? _name;
    private string? _synopsis;

    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public string? Synopsis
    {
        get => _synopsis;
        set
        {
            _synopsis = value;
            OnPropertyChanged();
        }
    }

    public TimeSpan Length { get; set; }

    public ObservableCollection<Genre> Genres { get; } = [];

    public Genre? Genre
    {
        get => _genre;
        set
        {
            _genre = value;
            OnPropertyChanged();
        }
    }

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
    }

    public async Task SaveShow()
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

        var client = new MgrManager.MgrManagerClient(App.Channel);
        var reply = await client.EditShowAsync(new EditShowRequest
        {
            UserId = App.UserId,
            Name = Name,
            Synopsis = Synopsis,
            Length = Duration.FromTimeSpan(Length),
            GenreId = Genre.Id
        });
        var eventHandler = reply.Result ? ShowMsg : ShowError;
        eventHandler?.Invoke(reply.Description);
    }
}