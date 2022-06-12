using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Managers;

public class EditShowViewModel : BaseViewModel
{
    public EditShowViewModel()
    {
        Genres = new ObservableCollection<Genre>();
    }

    public string Name { get; set; } = null!;

    public string Synopsis { get; set; } = null!;

    public TimeSpan Length { get; set; }

    public ObservableCollection<Genre> Genres { get; set; }

    public Genre? Genre { get; set; }

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
    }

    public async Task SaveShow()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            ShowError?.Invoke("Nome em falta.");
        }
        else if (string.IsNullOrWhiteSpace(Synopsis))
        {
            ShowError?.Invoke("Sinopse em falta.");
        }
        else if (Length <= TimeSpan.Zero)
        {
            ShowError?.Invoke($"Duração deverá ser superior a {TimeSpan.Zero}");
        }
        else if (Genre == null)
        {
            ShowError?.Invoke("Deverá selecionar um género.");
        }
        else
        {
            var client = new MgrManager.MgrManagerClient(App.Channel);
            var reply = await client.EditShowAsync(new EditShowRequest
            {
                UserId = App.UserId,
                Name = Name,
                Synopsis = Synopsis,
                Length = Duration.FromTimeSpan(Length),
                GenreId = Genre.Id
            });
            if (!reply.Result) ShowError?.Invoke(reply.Description);
            else ShowMsg?.Invoke(reply.Description);
        }
    }
}