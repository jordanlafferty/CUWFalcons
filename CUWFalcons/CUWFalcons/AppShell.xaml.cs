using System;
using System.Collections.Generic;
using CUWFalcons.ViewModels;
using CUWFalcons.Views;
using Xamarin.Forms;

namespace CUWFalcons
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RostersPage), typeof(RostersPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));

        }

    }
}

