using CUWFalcons.Models;
using Xamarin.Forms;
using System;
using Xamarin.Forms.Xaml;
using System.Collections;
using System.Data.SqlClient;

namespace CUWFalcons.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class NewCalendarEventPage : ContentPage
    {
        public int id;
        ArrayList sportCodes = new ArrayList(); // list of all sport codes
        CalendarEventModel _theEvent;  // this will hold the event that will potentially be edited


        // this will add a new event taking in the last id
        public NewCalendarEventPage(int idNum)
        {
            InitializeComponent();
            id = idNum;

            // sets the sportcode picker and the game type picker
            setGameTypes();
            setSports();
        }

        // constructor that takes in an existing event and allows the user to edit it
        public NewCalendarEventPage(CalendarEventModel theEvent)
        {
            
            InitializeComponent();

            // sets the sportcode picker and the year abreviation picker
            setSports();
            setGameTypes();

            Title = "Edit Event Information"; //changes display Title


            // inserts all the information from the event passed into the form so it can be edited
            _theEvent = theEvent;
            homeEntry.Text = theEvent.hometeam;
            awayEntry.Text = theEvent.awayteam;
            siteEntry.Text = theEvent.location;
            noteEntry.Text = theEvent.notes;
            sportCodeSelection.SelectedIndex = checkSportCode(theEvent);
            typeSelection.SelectedIndex = retrieveGameType(theEvent);
            if (theEvent.time != null)
            {
                timeEntry.Time = theEvent.time.Value;
            }
            dateEntry.Date = theEvent.date;
        }


        // sets the options for the picker determining what game type the event is
        public void setGameTypes()
        {
            typeSelection.Items.Add("Regular");
            typeSelection.Items.Add("Exhibition");
            typeSelection.Items.Add("Conference");
            typeSelection.Items.Add("Post Season");
        }


        // gets the index of what gametype was selected for a particular event so the picker can reflect that when making edits
        int retrieveGameType(CalendarEventModel theEvent)
        {
            if (theEvent.gametype == "")
            {
                return 0;
            }
            else if (theEvent.gametype == "#")
            {
                return 1;
            }
            else if (theEvent.gametype == "*")
            {
                return 2;
            }
            else if (theEvent.gametype == "%")
            {
                return 3;
            }
            return -1;
        }


        // gets the index of what sport was selected for an event so the picker can reflect that when making edits
        int checkSportCode(CalendarEventModel theEvent)
        {

            for (int i = 0; i < sportCodes.Count; i++)
            {
                if (String.Equals(sportCodes[i], theEvent.sport))
                {
                    return i;
                }
            }

            return -1;
        }

        // gets the symbol that is going to be displayed based on the game type
        public string getGameTypeSymbol()
        {

            if (typeSelection.SelectedIndex == 1) //exhibition
            {
                return "#";
            }
            else if (typeSelection.SelectedIndex == 2) // conference
            {
                return "*";
            }
            else if (typeSelection.SelectedIndex == 3) // post season
            {
                return "%";
            }

            return ""; //regular

        }


        // if conditions are met the evemnt is saved in the database
        async void saveEvent_Clicked(object sender, EventArgs e)
        {
            // checks to make sure an event has at least a hometeam, gametype and sport
            // also makes sure that rhe sports code is valid
            if (string.IsNullOrWhiteSpace(homeEntry.Text))
            {
                await DisplayAlert("Invalid", "A home team or event name is required", "OK");
            }
            else if (sportCodeSelection.SelectedIndex == -1)
            {
                await DisplayAlert("Invalid", "The game/event must have a sport.", "OK");
            }
            else if (typeSelection.SelectedIndex == -1)
            {
                await DisplayAlert("Invalid", "A game type must be choosen", "OK");
            }
            else if (_theEvent != null)
            {

                bool isTime = checkNULLs(); //fixes all NULL values and returns true if a time was selected
                _theEvent.gametype = getGameTypeSymbol(); // gets what symbol represents the game type
                editEvent(_theEvent, isTime); // adds new event to the database

            }
            else
            {
                bool isTime = checkNULLs(); //fixes all NULL values and returns true if a time was selected
                addNewEvent(id, isTime); // adds new event to database
            }


            
            
        }


        // checks all the data for an easier sql statement
        bool checkNULLs()
        {
           
            if (awayEntry.Text == null)
            {
                awayEntry.Text = " ";
            }
            if (siteEntry.Text == null)
            {
                siteEntry.Text = "";
            }
            if (noteEntry.Text == null)
            {
                noteEntry.Text = "";
            }

            if (timeEntry.Time == null)
            {
                return false;
            }
            return true;
        }


        // passes in the sports that display on the picker
        public void setSports()
        {
            ArrayList theSports = SportCodePage.read();
            for (int i = 0; i < theSports.Count - 1; i++)
            {
                SportModel sport = (SportModel)theSports[i];
                sportCodes.Add(sport.sportCode);
                sportCodeSelection.Items.Add(sport.sportCode);
            }

        }


        //adds a new event into the database
        async void addNewEvent(int eventNum, bool timeCheck)
        {
            // takes the next int available for the id and creates a new event based on the parameters added by the user
            // the new event is then added to the database
            string cmdString = $"INSERT INTO calendar_cuw VALUES (@id, @sport, @date, @time, @away, @home, @type, @site, @notes, NULL);";
            string connection = "Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            string gameSymbol = getGameTypeSymbol(); // gets the symbol that represents the game type


            // sees if a time was entered, if not will add event with time as NULL
            if (timeCheck == false) 
            {
                // update command string since a time was not selected
                cmdString = $"INSERT INTO calendar_cuw VALUES (@id, @sport, @date, NULL, @away, @home, @type, @site, @notes, NULL);";
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(cmdString, conn))
                    {
                        comm.Parameters.AddWithValue("@id", eventNum + 1);
                        comm.Parameters.AddWithValue("@sport", sportCodeSelection.Items[sportCodeSelection.SelectedIndex].ToString());
                        comm.Parameters.AddWithValue("@date", dateEntry.Date);
                        comm.Parameters.AddWithValue("@away", awayEntry.Text);
                        comm.Parameters.AddWithValue("@home", homeEntry.Text);
                        comm.Parameters.AddWithValue("@type", gameSymbol);
                        comm.Parameters.AddWithValue("@site", siteEntry.Text);
                        comm.Parameters.AddWithValue("@notes", noteEntry.Text);

                        try
                        {
                            comm.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    conn.Close();
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(cmdString, conn))
                    {
                        comm.Parameters.AddWithValue("@id", eventNum+ 1);
                        comm.Parameters.AddWithValue("@sport", sportCodeSelection.Items[sportCodeSelection.SelectedIndex].ToString());
                        comm.Parameters.AddWithValue("@date", dateEntry.Date);
                        comm.Parameters.AddWithValue("@time", timeEntry.Time);
                        comm.Parameters.AddWithValue("@away", awayEntry.Text);
                        comm.Parameters.AddWithValue("@home", homeEntry.Text);
                        comm.Parameters.AddWithValue("@type", gameSymbol);
                        comm.Parameters.AddWithValue("@site", siteEntry.Text);
                        comm.Parameters.AddWithValue("@notes", noteEntry.Text);

                        try
                        {

                            comm.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    conn.Close();
                }
            }

            await Navigation.PopAsync();
        }

        // this takes in an event the user choose to edit and uses the SQL command update to change the athlete info in the database
        async void editEvent(CalendarEventModel _Event, bool timeCheck)
        {

            int _id = _Event.id;
            string cmdString = $"UPDATE calendar_cuw SET id = @id, sportcode = @sport, date = @date, time = @time, away = @away, home = @home, type = @type, site = @site, notes = @notes WHERE id = @val ;";
            

            string connection = "Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            // sees if a time was entered, if not will add event with time as NULL
            if (timeCheck == false)
            {
                // update command string since a time was not selected
                cmdString = $"UPDATE calendar_cuw SET id = @id, sportcode = @sport, date = @date, away = @away, home = @home, type = @type, site = @site, notes = @notes WHERE id = @val;";
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(cmdString, conn))
                    {
                        
                        comm.Parameters.AddWithValue("@id", _id);
                        comm.Parameters.AddWithValue("@sport", sportCodeSelection.Items[sportCodeSelection.SelectedIndex].ToString());
                        comm.Parameters.AddWithValue("@date", dateEntry.Date);
                        comm.Parameters.AddWithValue("@away", awayEntry.Text);
                        comm.Parameters.AddWithValue("@home", homeEntry.Text);
                        comm.Parameters.AddWithValue("@type", _Event.gametype);
                        comm.Parameters.AddWithValue("@site", siteEntry.Text);
                        comm.Parameters.AddWithValue("@notes", noteEntry.Text);
                        comm.Parameters.AddWithValue("@val", _id);

                        try
                        {
                            comm.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    conn.Close();
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(cmdString, conn))
                    {
                        comm.Parameters.AddWithValue("@id", _id);
                        comm.Parameters.AddWithValue("@sport", sportCodeSelection.Items[sportCodeSelection.SelectedIndex].ToString());
                        comm.Parameters.AddWithValue("@date", dateEntry.Date);
                        comm.Parameters.AddWithValue("@time", timeEntry.Time);
                        comm.Parameters.AddWithValue("@away", awayEntry.Text);
                        comm.Parameters.AddWithValue("@home", homeEntry.Text);
                        comm.Parameters.AddWithValue("@type", _Event.gametype);
                        comm.Parameters.AddWithValue("@site", siteEntry.Text);
                        comm.Parameters.AddWithValue("@notes", noteEntry.Text);
                        comm.Parameters.AddWithValue("@val", _id);

                        try
                        {

                            comm.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    conn.Close();
                }
            }

            await Navigation.PopAsync();


        }

    }
}

