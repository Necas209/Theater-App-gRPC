using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Theaters;

public class SessionsViewModel : BaseViewModel
{
    public SessionsViewModel()
    {
        IsManager = App.UserType == User.UserType.Manager;
    }

    public ObservableCollection<Session> Sessions { get; } = [];

    public bool IsManager { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Today;

    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);

    public Session? Session { get; set; }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task DelSession()
    {
        if (Session == null)
        {
            ShowError?.Invoke("Selecione uma sessão primeiro.");
            return;
        }

        var client = new MgrManager.MgrManagerClient(App.Channel);
        var reply = await client.DelSessionAsync(new DelSessionRequest
        {
            UserId = App.UserId,
            Id = Session.Id
        });
        if (!reply.Result)
        {
            ShowError?.Invoke(reply.Description);
            return;
        }

        ShowMsg?.Invoke(reply.Description);
        Sessions.Remove(Session);
        Session = null;
    }

    public async Task GetSessions()
    {
        if (EndDate < StartDate)
        {
            ShowError?.Invoke("Data de início tem de ser anterior a data de fim");
            return;
        }

        var client = new TheaterManager.TheaterManagerClient(App.Channel);
        var reply = await client.GetSessionsAsync(new GetSessionsRequest
        {
            UserId = App.UserId,
            StartDate = Timestamp.FromDateTime(StartDate.ToUniversalTime()),
            EndDate = Timestamp.FromDateTime(EndDate.ToUniversalTime())
        });
        var sessions = JsonSerializer.Deserialize<List<Session>>(reply.Sessions);
        Sessions.Clear();
        foreach (var session in sessions ?? Enumerable.Empty<Session>()) Sessions.Add(session);
    }
}