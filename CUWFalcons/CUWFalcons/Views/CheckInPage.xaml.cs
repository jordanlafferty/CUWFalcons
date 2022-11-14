using System;
using System.Collections;
using System.ComponentModel;
using CUWFalcons.Models;
using System.Data.SqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CUWFalcons.Views
{
    public partial class CheckInPage : ContentPage
    {
        public CheckInPage()
        {
            InitializeComponent();
            displayTodaysEvents(); // shows the home events for today

        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                setContent(); // changes what the page looks like depending on the account
                displayTodaysEvents(); // shows the home events for today
            }
            catch
            {

            }
        }

        public void setContent()
        {
            // changes what the page looks like depending on the account
            string accountType = App.getAccount();
            if (accountType == "guest")
            {
                nonGuestScreen.IsVisible = false;
                guestScreen.IsVisible = true;
                qrManager_Btn.Text = "";
                qrManager_Btn.IsEnabled = false;
            }
            else
            {
                nonGuestScreen.IsVisible = true;
                guestScreen.IsVisible = false;
                if (accountType == "admin")
                {
                    qrManager_Btn.Text = "Manage QR Codes";
                    qrManager_Btn.IsEnabled = true;
                }
                else
                {
                    qrManager_Btn.Text = "";
                    qrManager_Btn.IsEnabled = false;
                }
            }
            
        }

        // shows the available home games that can be checked in to
        public void displayTodaysEvents()
        {
            SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            connection.Open();



            // database is 6 hours off of Wisconsin, so the commandText is different based on the time of day
            DateTime theDate = DateTime.Now;
            DateTime switchTime = DateTime.Parse("2022/01/01 18:00:00.000");
            string commandText;

            
            if (theDate.TimeOfDay >= switchTime.TimeOfDay)
            {
                commandText = $"SELECT * FROM calendar_cuw WHERE date = cast(getdate()-1 as date) AND (home='Concordia Wisconsin' OR home='CUW' or home='cuw' or home='Concordia University Wisconsin') ORDER BY time;";

            }
            else
            {
                commandText = $"SELECT * FROM calendar_cuw WHERE date = cast(getdate() as date) AND (home='Concordia Wisconsin' OR home='CUW' or home='cuw' or home='Concordia University Wisconsin') ORDER BY time;";

            }


            // gets home events today from database
            ArrayList homeEvents = new ArrayList();
            using (SqlCommand cmd = new SqlCommand(commandText, connection))
            {

                using (SqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        CalendarEventModel theEvent = CalendarDatabase.readEvents(reader);
                        homeEvents.Add(theEvent);
                    }

            }
            todayGameView.ItemsSource = homeEvents;
        }


        // switches to view qrcode and check ins
        private async void qrManagerToolbar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GenerateQRPage());

        }

        
        // takes the user to the QR Code Scanner while passing the event being checked into
        async void checkInBtn_Clicked(object sender, EventArgs e)
        {
            var item = sender as Button;
            var theEvent = item.CommandParameter as CalendarEventModel;
            await Navigation.PushAsync(new QRScannerPage(theEvent));

            
        }

        // button is only visible to guests
        // returns guest to the log in screen
        async void loginBtn_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
