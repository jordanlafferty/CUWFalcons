using System;
using Xamarin.Forms;
using CUWFalcons.Models;
using System.Collections;
using System.Data.SqlClient;


namespace CUWFalcons.Views
{
    public partial class CalendarPage : ContentPage
    {
        private const string TABLE_NAME = "calendar_cuw";
        int eventLen; // holds the last id number for an event, a new event's id would be +1

        public CalendarPage()
        {
            InitializeComponent();

        }

        // disables the ability to add an event if the user is not admin & cleans up view based on account
        private void checkAccount()
        {
            string accountType = App.getAccount();
            if (accountType != "admin")
            {
                addEvent_Btn.Text = "";
                addEvent_Btn.IsEnabled = false;
                eventView.IsVisible = true;
                eventView.IsEnabled = true;
                adminView.IsVisible = false;
                adminView.IsEnabled = false;
                adminLabel.IsVisible = false;
            }
            else
            {
                eventView.IsVisible = false;
                eventView.IsEnabled = false;
                adminView.IsVisible = true;
                adminView.IsEnabled = true;
                addEvent_Btn.Text = "Add Event";
                addEvent_Btn.IsEnabled = true;
                adminLabel.IsVisible = true;

            }

        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                checkAccount(); //sees what account is being used to adjust view
                getLastID(); // gets the last event id used -- so next event gets the next id
            }
            catch
            {

            }
        }


        // moves to a page for the user to be able to add an athlete
        private async void addEventToolbar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewCalendarEventPage(eventLen));
  
        }

        // switches to a page where users can view each team's full schedule
        private async void viewTeamSchedules_Btn_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new TeamCalendarPage());
        }


        // displays the events based on the day in the calendar that is clicked
        void date_Clicked(object sender, EventArgs e)
        {
           // sets the label to a
           DateTime? currDate = theCalendar.SelectedDate;
           DateTime displayDate = currDate ?? DateTime.Now;
           dateLabel.Text = displayDate.ToString("MMMM dd yyyy"); 


           // SQL query to get all the events
           SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
           connection.Open();

                ArrayList dateEvents = new ArrayList();
           string commandText = $"SELECT * FROM {TABLE_NAME} WHERE date = '{currDate}'ORDER BY 'time'";
           using (SqlCommand cmd = new SqlCommand(commandText, connection))
           {

                using (SqlDataReader reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    CalendarEventModel theEvent = CalendarDatabase.readEvents(reader);
                    dateEvents.Add(theEvent);
                }

           }
            connection.Close();

            // sets the view to display all the events on the day selected
            eventView.ItemsSource = dateEvents;
            adminView.ItemsSource = dateEvents;

        }


        // takes the event that needs to be edited to a page that will give the ability to edit
        async void editSwipe_Invoked(object sender, EventArgs e)
        {
            
            var item = sender as SwipeItem;
            var theEvent = item.CommandParameter as CalendarEventModel;

            await Navigation.PushAsync(new NewCalendarEventPage(theEvent));

        }

        // deletes the event that was swiped from the database
        public void deleteSwipe_Invoked(object sender, EventArgs e)
        {
            
            var item = sender as SwipeItem;
            var theEvent = item.CommandParameter as CalendarEventModel;
            int idnum = theEvent.id;
            
            string cmd = $"DELETE FROM calendar_cuw WHERE id = @val;";
            string connection = "Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection conn = new SqlConnection(connection))
            {
                using (SqlCommand comm = new SqlCommand(cmd, conn))
                {
                    try
                    {
                        comm.Parameters.AddWithValue("@val", idnum);
                        conn.Open();
                        comm.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

            getLastID();
        }

        // gets the last events id & sets it to eventLen
        public void getLastID()
        {

            SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            connection.Open();

            string commandText = $"SELECT * FROM {TABLE_NAME} ORDER BY 'id' DESC";
            using (SqlCommand cmd = new SqlCommand(commandText, connection))
            {
                eventLen = 0;

                using (SqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        CalendarEventModel theEvent = CalendarDatabase.readEvents(reader);
                        eventLen = theEvent.id;
                        connection.Close();
                        break;
                        
                    }

            }

        }
    }
}

