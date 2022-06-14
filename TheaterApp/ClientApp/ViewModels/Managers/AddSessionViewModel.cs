using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Managers;

public class AddSessionViewModel : BaseViewModel
{
    public AddSessionViewModel()
    {
        Showtime = DateTime.Now;
        Shows = new ObservableCollection<Show>();
        Theaters = new ObservableCollection<Theater>();
    }

    public ObservableCollection<Show> Shows { get; set; }

    public ObservableCollection<Theater> Theaters { get; set; }

    public Show? Show { get; set; }

    public Theater? Theater { get; set; }

    public DateTime Showtime { get; set; }

    [DataType(DataType.Currency)] public decimal TicketPrice { get; set; }

    public int TotalSeats { get; set; }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task AddSession()
    {
        if (Show == null)
        {
            ShowError?.Invoke("Selecione um espetáculo");
        }
        else if (Theater == null)
        {
            ShowError?.Invoke("Selecione um teatro.");
        }
        else if (Showtime <= DateTime.Now)
        {
            ShowError?.Invoke("Data deverá ser futura.");
        }
        else if (TicketPrice <= 0)
        {
            ShowError?.Invoke($"Preço do bilhete deverá ser superior a {0:C}");
        }
        else if (TotalSeats <= 0)
        {
            ShowError?.Invoke("Número de lugares deverá ser superior a 0.");
        }
        else
        {
            var client = new MgrManager.MgrManagerClient(App.Channel);
            var reply = await client.AddSessionAsync(new AddSessionRequest
            {
                UserId = App.UserId,
                ShowId = Show.Id,
                TheaterId = Theater.Id,
                Showtime = Timestamp.FromDateTime(Showtime.ToUniversalTime()),
                TotalSeats = TotalSeats,
                TicketPrice = TicketPrice
            });
            if (!reply.Result)
                ShowError?.Invoke(reply.Description);
            else
                ShowMsg?.Invoke(reply.Description);
        }
    }

    public async Task GetShows(string textInput)
    {
        if (textInput.Length == 0)
        {
            ShowError?.Invoke("Espetáculo em falta.");
        }
        else
        {
            var client = new TheaterManager.TheaterManagerClient(App.Channel);
            var reply = await client.GetShowsAsync(new GetShowsRequest
            {
                Name = textInput
            });
            var shows = JsonSerializer.Deserialize<List<Show>>(reply.Shows);
            if (shows != null)
            {
                Shows.Clear();
                shows.ForEach(show => Shows.Add(show));
            }
        }
    }

    public async Task GetTheaters(string textInput)
    {
        if (textInput.Length == 0)
        {
            ShowError?.Invoke("Teatro em falta.");
        }
        else
        {
            var client = new TheaterManager.TheaterManagerClient(App.Channel);
            var reply = await client.GetTheatersAsync(new GetTheatersRequest
            {
                Name = textInput
            });
            var theaters = JsonSerializer.Deserialize<List<Theater>>(reply.Theaters);
            if (theaters != null)
            {
                Theaters.Clear();
                theaters.ForEach(theater => Theaters.Add(theater));
            }
        }
    }
}