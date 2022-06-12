using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Theaters;

public class TheatersViewModel : BaseViewModel
{
    public TheatersViewModel()
    {
        IsManager = App.UserType == User.UserType.Manager;
        Theaters = new ObservableCollection<Theater>();
    }

    public ObservableCollection<Theater> Theaters { get; set; }

    public Theater? Theater { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public bool IsManager { get; set; }

    public event StringMethod? ShowError;

    public async Task GetTheaters()
    {
        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var reply = await client.GetTheatersAsync(new GetTheatersRequest
        {
            UserId = App.UserId,
            Name = Name,
            Location = Location
        });
        var theaters = JsonSerializer.Deserialize<List<Theater>>(reply.Theaters);
        if (theaters?.Count != 0)
        {
            Theaters.Clear();
            theaters?.ForEach(show => Theaters.Add(show));
        }
    }

    public async Task DelTheater()
    {
        if (Theater == null)
        {
            ShowError?.Invoke("Escolha um teatro primeiro.");
        }
        else
        {
            var client = new MgrManager.MgrManagerClient(App.Channel);
            var reply = await client.DelTheaterAsync(new DelTheaterRequest
            {
                UserId = App.UserId,
                Id = Theater.Id
            });
            if (!reply.Result)
                ShowError?.Invoke(reply.Description);
        }
    }
}