using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Theaters;

public class SessionsViewModel : BaseViewModel
{
    private bool _isManager;

    public SessionsViewModel()
    {
        Sessions = new ObservableCollection<Session>();
    }

    public ObservableCollection<Session> Sessions { get; set; }

    public bool IsManager
    {
        get => _isManager;
        set
        {
            _isManager = value;
            OnPropertyChanged(nameof(IsManager));
        }
    }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Session? Session { get; set; }

    public event StringMethod? ShowError;

    public async Task DelSession(App app)
    {
        if (Session == null)
        {
            ShowError?.Invoke("Selecione uma sessão primeiro.");
        }
        else
        {
            var client = new MgrManager.MgrManagerClient(app.Channel);
            var reply = await client.DelSessionAsync(new DelSessionRequest
            {
                UserId = app.UserId,
                Id = Session.Id
            });
            if (!reply.Result)
                ShowError?.Invoke(reply.Description);
        }
    }

    public async Task GetSessions(App app)
    {
        if (EndDate < StartDate)
        {
            ShowError?.Invoke("Data de início tem de ser anterior a data de fim");
        }
        else
        {
            var client = new TheaterManager.TheaterManagerClient(app.Channel);
            var reply = await client.GetSessionsAsync(new GetSessionsRequest
            {
                UserId = app.UserId,
                StartDate = Timestamp.FromDateTime(StartDate),
                EndDate = Timestamp.FromDateTime(EndDate)
            });
            var sessions = JsonSerializer.Deserialize<List<Session>>(reply.Sessions);
            Sessions.Clear();
            sessions?.ForEach(session => Sessions.Add(session));
        }
    }
}