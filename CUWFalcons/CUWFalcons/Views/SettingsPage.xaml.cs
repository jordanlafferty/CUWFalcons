using System;
using System.Collections.Generic;
using System.Diagnostics;
using CUWFalcons.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CUWFalcons.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            this.BindingContext = new SettingsViewModel();
        }

        void websiteBtn_Clicked(System.Object sender, System.EventArgs e)
        {
            var url = "https://www.cuwfalcons.com/landing/index";
            Launcher.OpenAsync(url); 
        }

    }
}

