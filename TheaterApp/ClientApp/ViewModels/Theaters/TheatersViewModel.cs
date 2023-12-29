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
    }

    public ObservableCollection<Theater> Theaters { get; } = [];

    public Theater? Theater { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public bool IsManager { get; set; }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task GetTheaters()
    {
        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var request = new GetTheatersRequest
        {
            UserId = App.UserId
        };
        if (!string.IsNullOrWhiteSpace(Name))
            request.Name = Name;
        if (!string.IsNullOrWhiteSpace(Location))
            request.Location = Location;
        var reply = await client.GetTheatersAsync(request);
        var theaters = JsonSerializer.Deserialize<List<Theater>>(reply.Theaters);
        if (theaters == null) return;
        Theaters.Clear();
        Theater = null;
        foreach (var show in theaters) Theaters.Add(show);
    }

    public async Task DelTheater()
    {
        if (Theater == null)
        {
            ShowError?.Invoke("Escolha um teatro primeiro.");
            return;
        }

        var client = new MgrManager.MgrManagerClient(App.Channel);
        var reply = await client.DelTheaterAsync(new DelTheaterRequest
        {
            UserId = App.UserId,
            Id = Theater.Id
        });
        if (!reply.Result)
        {
            ShowError?.Invoke(reply.Description);
            return;
        }

        ShowMsg?.Invoke(reply.Description);
        Theaters.Remove(Theater);
        Theater = null;
    }
}