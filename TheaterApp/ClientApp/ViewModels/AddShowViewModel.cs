using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels;

public class AddShowViewModel : BaseViewModel
{
    public AddShowViewModel()
    {
        Name = Synopsis = "";
        Genres = new ObservableCollection<Genre>();
    }

    public string Name { get; set; }

    public string Synopsis { get; set; }

    public TimeSpan Length { get; set; }

    public ObservableCollection<Genre> Genres { get; set; }

    public Genre? Genre { get; set; }

    public event StringMethod ShowError;
    
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

    public async Task AddShow(App app)
    {
        if (string.IsNullOrWhiteSpace(Name))
            ShowError("Nome em falta.");
        else if (string.IsNullOrWhiteSpace(Synopsis))
            ShowError("Sinopse em falta.");
        else if (Length <= TimeSpan.Zero)
            ShowError($"Duração deverá ser superior a {TimeSpan.Zero}");
        else if (Genre == null)
            ShowError("Deverá selecionar um género.");
        else
        {
            var client = new MgrManager.MgrManagerClient(app.Channel);
            var reply = await client.AddShowAsync(new AddShowRequest
            {
                UserId = app.UserId,
                Name = Name,
                Synopsis = Synopsis,
                Length = Duration.FromTimeSpan(Length),
                GenreId = Genre.Id
            });
            if (!reply.Result)
                ShowError(reply.Description);
        }
    }
}