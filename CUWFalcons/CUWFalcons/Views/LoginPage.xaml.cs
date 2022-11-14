using System;
using CUWFalcons.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CUWFalcons.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
            
        }

        // sees if the user is able to log in
        // successful login takes the user to the check in page
        async void login_Clicked(object sender, EventArgs e)
        {

            // makes sure the username is entered
            if (string.IsNullOrWhiteSpace(usernameEntry.Text))
            {
                await DisplayAlert("Invalid", "Enter a username", "OK");
            }
            // makes sure the password is entered
            else if (string.IsNullOrWhiteSpace(passwordEntry.Text))
            {

                await DisplayAlert("Invalid", "Enter a password", "OK");
            }
            else
            {
                // sees if the username and password match a user
                // returns "fail" if unsuccessful otherwise gets the users loginmodel
                string account = LoginDatabase.checkLogin(usernameEntry.Text, passwordEntry.Text);
                
                if (account == "fail")
                {
                    await DisplayAlert("Invalid", "Wrong username or password", "OK");
                }
                else
                {
                    // sets the account type and username to the app to be used later
                    App.setUser(account, usernameEntry.Text);

                    // resets the username and password for next login
                    usernameEntry.Text = ""; 
                    passwordEntry.Text = "";


                    BindingContext = new LoginViewModel();
                    await Shell.Current.GoToAsync($"//{nameof(CheckInPage)}");
                }

            }
        }

        // user will view the app as a guest
        async void guest_Clicked(object sender, EventArgs e)
        {
            App.setUser("guest", ""); //sets the type of account to guest and no  username
            BindingContext = new LoginViewModel();
            await Shell.Current.GoToAsync($"//{nameof(CheckInPage)}");

        }
    }
}