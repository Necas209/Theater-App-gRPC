using System.Windows;

namespace ClientApp.Views.Clients;

public partial class BuyTicketsWindow
{
    private readonly App _app;

    public BuyTicketsWindow()
    {
        _app = (Application.Current as App)!;
        InitializeComponent();
    }
}