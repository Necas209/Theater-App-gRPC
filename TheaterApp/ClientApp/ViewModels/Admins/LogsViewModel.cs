using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Admins;

public class LogsViewModel : BaseViewModel
{
    public ObservableCollection<Log> Logs { get; set; }

    public LogsViewModel()
    {
        StartDate = DateTime.Today.AddDays(-7);
        EndDate = DateTime.Now;
        Logs = new ObservableCollection<Log>();
    }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public event StringMethod? ShowError;

    public async Task GetLogs()
    {
        if (EndDate < StartDate)
        {
            ShowError?.Invoke("Data de início tem de ser anterior a data de fim");
        }
        else
        {
            var client = new AdminManager.AdminManagerClient(App.Channel);
            var reply = await client.GetLogsAsync(new GetLogsRequest
            {
                UserId = App.UserId,
                StartDate = Timestamp.FromDateTime(StartDate.ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(EndDate.AddDays(1).ToUniversalTime())
            });
            var logs = JsonSerializer.Deserialize<List<Log>>(reply.Logs);
            if (logs != null)
            {
                Logs.Clear();
                logs.ForEach(log => Logs.Add(log));
            }
        }
    }
}