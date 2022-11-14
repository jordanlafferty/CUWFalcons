using System;
using System.Collections.Generic;
using System.Diagnostics;
using CUWFalcons.ViewModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            BindingContext = new SettingsViewModel();
            checkAccount(); // sets the view based on the account type

        }

        protected override void OnAppearing()
        {
            checkAccount();
        }


        // sets the view based on the account type
        private void checkAccount()
        {
            string accountType = App.getAccount();
            if(accountType != "admin")
            {
                newSportBtn.IsVisible = false;
                viewSportBtn.IsVisible = false;
                if (accountType == "guest")
                {
                    logoutBtn.Text = "Go to Login Page";
                }
                else
                {
                    logoutBtn.Text = "Logout";
                }
            }
            else
            {
                logoutBtn.Text = "Logout";
                newSportBtn.IsVisible = true;
                viewSportBtn.IsVisible = true;
            }

        }

        // navigate to cuw falcons website
        void websiteBtn_Clicked(object sender, EventArgs e)
        {
            var url = "https://www.cuwfalcons.com/landing/index";
            Launcher.OpenAsync(url);
        }

        // goes to new sport page
        async void newSportBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewSportPage());
            
        }

        // goes to sports view page
        async void viewSportBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SportCodePage());
        }

        // logs the user out/returns to log in screen
        async void logoutBtn_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}

