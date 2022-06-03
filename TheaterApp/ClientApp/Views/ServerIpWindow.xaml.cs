using System.Windows;

namespace ClientApp.Views;

public partial class ServerIpWindow
{
    private readonly App _app;
    
    public ServerIpWindow()
    {
        _app = (Application.Current as App)!;
        InitializeComponent();
    }
}