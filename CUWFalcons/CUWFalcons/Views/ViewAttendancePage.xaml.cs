using System;
using System.Collections;
using System.Data.SqlClient;
using CUWFalcons.Models;
using Xamarin.Forms;

namespace CUWFalcons.Views
{
    public partial class ViewAttendancePage : ContentPage
    {
        CalendarEventModel theEvent; // event that attendance is being viewed
        ArrayList theAttendance; // all the checked in users

        public ViewAttendancePage(CalendarEventModel anEvent)
        {
            InitializeComponent();
            theEvent = anEvent; // event that attendance is being viewed
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                // update the view based on who is checked in
                theAttendance = setAttendance(theEvent.attendance);
                attendanceView.ItemsSource = theAttendance;
            }
            catch
            {

            }
        }


        // gets the attendance string for the event
        public void displayAttendance()
        {
            SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            connection.Open();
            int id = theEvent.id;
            string attendees = "";
            string commandText = $"SELECT * FROM calendar_cuw WHERE id = @val;";
            using (SqlCommand cmd = new SqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("@val", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        CalendarEventModel theEvent = CalendarDatabase.readEvents(reader);
                        attendees = theEvent.attendance;
                    }

            }

            theAttendance = setAttendance(attendees); // updates the view

            connection.Close();
        }


        //generates a random user that checked into the event
        async void chooseUser_Clicked(object sender, EventArgs e)
        {
            int len = 0;
            if(theAttendance != null) // makes sure someone is checked in
            {
                len = theAttendance.Count;
            }

            // chooses a user
            if(len != 0)
            {
                Random rnd = new Random();
                int randomindex = rnd.Next(len);
                string name = theAttendance[randomindex].ToString();
                await DisplayAlert("Random Attendee", name, "OK");
            }

            // no one is checked in
            else
            {
                await DisplayAlert("Random Attendee", "No one is checked in to this event", "OK");
            }
            

            
        }

        // separates the list of names that attended the event & returns an arraylist
        public ArrayList setAttendance(string listOfPeople)
        {
            string name = "";
            ArrayList attendances = new ArrayList();

            for (int i = 0; i < listOfPeople.Length; i++)
            {
                if (listOfPeople[i].ToString() != ",")
                {
                    name = name + listOfPeople[i].ToString();
                }
                else
                {
                    attendances.Add(name);
                    name = "";
                }
            }
            attendances.Add(name);
            return attendances;

        }
    }
}


