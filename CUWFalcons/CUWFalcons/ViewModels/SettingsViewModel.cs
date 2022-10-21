using System;
using CUWFalcons.Views;
using Xamarin.Forms;

namespace CUWFalcons.ViewModels
{
    public class SettingsViewModel : ContentView
    {
        public Command LogoutCommand { get; }

        public SettingsViewModel()
        {
            Content = new Label { Text = "Hello ContentView" };
            LogoutCommand = new Command(OnLogoutClicked);
        }


        private async void OnLogoutClicked(object obj)
        {
            // logs the user out and goes back to the log in page
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}


