using Xamarin.Forms;
using System;
using Picker = Xamarin.Forms.Picker;
using CUWFalcons.Models;
using System.Collections;
using System.Data.SqlClient;

namespace CUWFalcons.Views
{
    public partial class TeamCalendarPage : ContentPage
    {
        
        private const string TABLE_NAME = "calendar_cuw";
        private const string SPORT_TABLE_NAME = "sports";
        public int eventLen; // last id used by an event
        ArrayList theSports = new ArrayList();// list of sport models
        ArrayList pickerSports = new ArrayList(); // list of sport display names to populate the picker
        

        public TeamCalendarPage()
        {
            InitializeComponent();
            

        }

        // updates view based on account
        private void checkAccount()
        {
            string accountType = App.getAccount();
            if (accountType != "admin")
            {
                addEvent_Btn.Text = "";
                addEvent_Btn.IsEnabled = false;
                eventView.IsVisible = true;
                adminEventView.IsVisible = false;
                adminLabel.IsVisible = false;
                refresh_Btn.Text = "";
                refresh_Btn.IsEnabled = false;

            }
            else
            {
                addEvent_Btn.Text = "Add an Event";
                addEvent_Btn.IsEnabled = true;
                eventView.IsVisible = false;
                adminEventView.IsVisible = true;
                adminLabel.IsVisible = true;
                refresh_Btn.Text = "Refresh";
                refresh_Btn.IsEnabled = true;


            }

        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
               
                // creates an arraylist of all the sports and their code for refrence, as well as creates an array list to populate the picker
                setPickerItems();
                checkAccount();
                sportSelection.ItemsSource = pickerSports;

                getLastId(); // gets id of last event
            }
            catch
            {

            }
        }

       
        // sets the sport selection picket
        public void setPickerItems()
        {
            
            SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            try { connection.Open(); }
            catch(Exception e){ Console.WriteLine(e); }

            

            //resets the data in case there has been a change
             theSports.Clear();
             pickerSports.Clear();
       
            
            string commandText = $"SELECT * FROM {SPORT_TABLE_NAME}";
            using (SqlCommand cmd = new SqlCommand(commandText, connection))
            {   
                using (SqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        SportModel sport = SportDatabase.readSports(reader);

                        pickerSports.Add(sport.displayName); 
                        theSports.Add(sport);
                    }

            }

            connection.Close();
                
        }

        // gets the sport code that corresponds with the displayName
        string getSportCode(string displayName)
        {

            for (int i = 0; i < theSports.Count; i++)
            {
                SportModel currSport = (SportModel)theSports[i];
                string currDisplay = currSport.displayName;
                if (displayName == currDisplay)
                {
                    return currSport.sportCode;
                }
            }

            return "";
        }

        // changes the events displayed when the sport chosen changes
        void SportChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (sportSelection.SelectedIndex != -1)
            {
                updateEventsShown();
            }
        }

        // gets the events shown for a sport from the database
        public void updateEventsShown()
        {

            int selectedIndex = sportSelection.SelectedIndex;
            

            if (selectedIndex != -1)
            {
                string displayName = sportSelection.SelectedItem.ToString();
                string sportcode = getSportCode(displayName);
                SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                connection.Open();

                ArrayList eventList = new ArrayList();
                string commandText = $"SELECT * FROM {TABLE_NAME} WHERE sportcode = '{sportcode}'ORDER BY 'date'";
                using (SqlCommand cmd = new SqlCommand(commandText, connection))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            CalendarEventModel theEvent = CalendarDatabase.readEvents(reader);
                            if (theEvent.id != -1)
                            {
                                eventList.Add(theEvent);
                            }
                            
                        }

                }
                eventView.ItemsSource = eventList;
                adminEventView.ItemsSource = eventList;
                connection.Close();

            }
        }

        // takes user to add an athlete page
        private async void addEventToolbar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewCalendarEventPage(eventLen));


        }

        // refreshes the page to see any changes made to a scheduler
        private void refreshToolbar_Clicked(object sender, EventArgs e)
        {
            if (sportSelection.SelectedIndex != -1)
            {
                updateEventsShown(); // shows events for particular sport
            }
            setPickerItems();
            sportSelection.ItemsSource = pickerSports;
            getLastId(); // gets id of last event


        }

        async void editSwipe_Invoked(object sender, EventArgs e)
        {
            // takes the event that was swiped on 
            var item = sender as SwipeItem;
            var theEvent = item.CommandParameter as CalendarEventModel;

            // go to edit event
           await Navigation.PushAsync(new NewCalendarEventPage(theEvent));
            
        }


        public void deleteSwipe_Invoked(object sender, EventArgs e)
        {
            // takes the event that was swiped on 
            var item = sender as SwipeItem;
            var athlete = item.CommandParameter as CalendarEventModel;
            int idnum = athlete.id;


            // deletes the event passed in from the database
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
            if (sportSelection.SelectedIndex != -1)
            {
                updateEventsShown();
            }
            getLastId(); // gets id of last event
        }



        // gets id of last event
        public void getLastId()
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
