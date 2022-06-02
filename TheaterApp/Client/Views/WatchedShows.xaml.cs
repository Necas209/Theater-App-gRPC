using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Windows;
using Client.ViewModels;
using Greet;
using GrpcLibrary.Models;

namespace Client.Views;

public partial class WatchedShows
{
    private readonly App _app;
    private readonly ShowsViewModel _model;

    public WatchedShows()
    {
        _app = (Application.Current as App)!;
        DataContext = _model = new ShowsViewModel();
        Dispatcher.Invoke(async () =>
        {
            var client = new ClientManager.ClientManagerClient(_app.Channel);
            var reply = await client.WatchedShowsAsync(
                new GetShowsRequest
                {
                    UserId = _app.UserId,
                    EndDate = _model.EndDate.ToString(CultureInfo.InvariantCulture),
                    StartDate = _model.StartDate.ToString(CultureInfo.InvariantCulture)
                }
            );
            var shows = JsonSerializer.Deserialize<List<Show>>(reply.Shows);
            shows?.ForEach(show => _model.Shows.Add(show));
        });
        InitializeComponent();
    }
}