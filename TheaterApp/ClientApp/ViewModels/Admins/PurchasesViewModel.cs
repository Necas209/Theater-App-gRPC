using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using GrpcLibrary.Models;

namespace ClientApp.ViewModels.Admins;

public class PurchasesViewModel : BaseViewModel
{
    public ObservableCollection<Reservation> Purchases { get; } = [];

    public DateTime StartDate { get; set; } = DateTime.Today.AddMonths(-3);

    public DateTime EndDate { get; set; } = DateTime.Now;

    public event StringMethod? ShowError;

    public async Task GetPurchases()
    {
        if (EndDate > DateTime.Today)
        {
            ShowError?.Invoke("Data de fim deve ser atual!");
            return;
        }

        if (EndDate < StartDate)
        {
            ShowError?.Invoke("Data de início tem de ser anterior a data de fim");
            return;
        }

        var client = new AdminManager.AdminManagerClient(App.Channel);
        var reply = await client.GetPurchasesAsync(new GetPurchasesRequest
            {
                UserId = App.UserId,
                StartDate = Timestamp.FromDateTime(StartDate.ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(EndDate.ToUniversalTime())
            }
        );
        var purchases = JsonSerializer.Deserialize<List<Reservation>>(reply.Purchases);
        if (purchases != null)
        {
            Purchases.Clear();
            purchases.ForEach(purchase => Purchases.Add(purchase));
        }
    }
}