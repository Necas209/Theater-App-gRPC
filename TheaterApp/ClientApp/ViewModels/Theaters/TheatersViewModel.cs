using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Theaters;

public class TheatersViewModel : BaseViewModel
{
    private bool _isManager;

    public TheatersViewModel()
    {
        Name = Location = "";
        Theaters = new ObservableCollection<Theater>();
    }

    public ObservableCollection<Theater> Theaters { get; set; }

    public Theater? Theater { get; set; }

    public string Name { get; set; }

    public string Location { get; set; }
    
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

    public async Task GetTheaters(App app)
    {
        var client = new TheaterManager.TheaterManagerClient(app.Channel);
        var reply = await client.GetTheatersAsync(new GetTheatersRequest
        {
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

    public async Task DelTheater(App app)
    {
        if (Theater == null)
        {
            ShowError?.Invoke("Escolha um teatro primeiro.");
        }
        else
        {
            var client = new MgrManager.MgrManagerClient(app.Channel);
            var reply = await client.DelTheaterAsync(new DelTheaterRequest
            {
                UserId = app.UserId,
                Id = Theater.Id
            });
            if (!reply.Result)
                ShowError?.Invoke(reply.Description);
        }
    }
}