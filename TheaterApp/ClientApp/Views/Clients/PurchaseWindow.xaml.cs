﻿using System.Windows;

namespace ClientApp.Views.Clients;

public partial class PurchaseWindow
{
    private readonly App _app;

    public PurchaseWindow()
    {
        _app = (Application.Current as App)!;
        InitializeComponent();
    }
}