using System.Windows;

namespace ClientApp.Views;

public partial class HomeWindow
{
    private readonly App _app;

    public HomeWindow()
    {
        _app = (Application.Current as App)!;
        InitializeComponent();
    }
}