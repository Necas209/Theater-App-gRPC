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
    public SessionsViewModel()
    {
        StartDate = DateTime.Today;
        EndDate = DateTime.Today.AddDays(7);
        IsManager = App.UserType == User.UserType.Manager;
        Sessions = new ObservableCollection<Session>();
    }

    public ObservableCollection<Session> Sessions { get; set; }

    public bool IsManager { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Session? Session { get; set; }

    public event StringMethod? ShowError;

    public event StringMethod? ShowMsg;

    public async Task DelSession()
    {
        if (Session == null)
        {
            ShowError?.Invoke("Selecione uma sessão primeiro.");
        }
        else
        {
            var client = new MgrManager.MgrManagerClient(App.Channel);
            var reply = await client.DelSessionAsync(new DelSessionRequest
            {
                UserId = App.UserId,
                Id = Session.Id
            });
            if (!reply.Result)
            {
                ShowError?.Invoke(reply.Description);
            }
            else
            {
                ShowMsg?.Invoke(reply.Description);
                Sessions.Remove(Session);
                Session = null;
            }
        }
    }

    public async Task GetSessions()
    {
        if (EndDate < StartDate)
        {
            ShowError?.Invoke("Data de início tem de ser anterior a data de fim");
        }
        else
        {
            var client = new TheaterManager.TheaterManagerClient(App.Channel);
            var reply = await client.GetSessionsAsync(new GetSessionsRequest
            {
                UserId = App.UserId,
                StartDate = Timestamp.FromDateTime(StartDate),
                EndDate = Timestamp.FromDateTime(EndDate)
            });
            var sessions = JsonSerializer.Deserialize<List<Session>>(reply.Sessions);
            Sessions.Clear();
            sessions?.ForEach(session => Sessions.Add(session));
        }
    }
}