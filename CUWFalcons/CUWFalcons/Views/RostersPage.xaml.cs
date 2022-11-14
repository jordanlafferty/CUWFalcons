using Xamarin.Forms;
using System;
using Picker = Xamarin.Forms.Picker;
using CUWFalcons.Models;
using System.Collections;
using System.Data.SqlClient;

namespace CUWFalcons.Views
{
    public partial class RostersPage : ContentPage
    {
        private const string TABLE_NAME = "rosters_cuw";
        private const string SPORT_TABLE_NAME = "sports";
        public int athleteLen;
        ArrayList theSports = new ArrayList();// list of sport models
        ArrayList pickerSports = new ArrayList(); // list of sport display names to populate the picker
        

        public RostersPage()
        {
            InitializeComponent();
        }


        // changes what will appear on the screen so that non-admin users can not edit data
        private void checkAccount()
        {
            string accountType = App.getAccount();
            if (accountType != "admin")
            {
                addAthlete_Btn.Text = "";
                addAthlete_Btn.IsEnabled = false;
                rosterView.IsVisible = true;
                adminRosterView.IsVisible = false;
                refresh_Btn.Text = "";
                refresh_Btn.IsEnabled = false;
            }
            else
            {
                addAthlete_Btn.Text = "Add an Athlete";
                addAthlete_Btn.IsEnabled = true;
                rosterView.IsVisible = false;
                adminRosterView.IsVisible = true;
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

                getLastID(); // gets the id of the last athlete
            }
            catch
            {

            }
        }



        // gets the sports that will display in the SportSelection picker from the database
        public void setPickerItems()
        {

            SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            try { connection.Open(); }
            catch (Exception e) { Console.WriteLine(e); }



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


        // gets the sportcode that corresponds to the displayname
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

        // updates the athletes shown when the sport in the picker is changed
        void SportChanged(object sender, EventArgs e)
        {
            if (sportSelection.SelectedIndex != -1)
            {
                updateAthletesShown();
            }
        }


        // updates the athletes shown based on the sport selected
        public void updateAthletesShown()
        {

            int selectedIndex = sportSelection.SelectedIndex;
            

            if (selectedIndex != -1) // makes sure a sport is selected
            {
                string displayName = sportSelection.SelectedItem.ToString(); // gets the display name of the sport selected
                string sportcode = getSportCode(displayName); //returns the sport code of the selected one in the picker


                // gets the athletes corresponding to a sport
                SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                connection.Open();

                ArrayList cuwathletes = new ArrayList();
                string commandText = $"SELECT * FROM {TABLE_NAME} WHERE sport_code = '{sportcode}'ORDER BY 'number';";
                using (SqlCommand cmd = new SqlCommand(commandText, connection))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            AthleteModel athlete = RosterDatabase.readAthletes(reader);
                            cuwathletes.Add(athlete);
                        }

                }

                // changes the views to reflect the correct athletes
                rosterView.ItemsSource = cuwathletes;
                adminRosterView.ItemsSource = cuwathletes;
                connection.Close();

            }
        }


        // takes the user to add an athlete page
        private async void addAthleteToolbar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ModifyAthletePage(athleteLen));

            
        }

        // updates the view to reflect any changes made to athlete(s)
        private void refreshToolbar_Clicked(object sender, EventArgs e)
        {
            if(sportSelection.SelectedIndex != -1)
            {
                updateAthletesShown();
            }
            setPickerItems(); // updates the sports picket
            sportSelection.ItemsSource = pickerSports;
            getLastID(); // gets the id of the last athlete


        }

        // takes the athlete swiped on to be edited
        async void editSwipe_Invoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var athlete = item.CommandParameter as AthleteModel;

           await Navigation.PushAsync(new ModifyAthletePage(athlete));
            
        }

        // deletes the athlete from the database
        public void deleteSwipe_Invoked(object sender, EventArgs e)
        {
            // gets athlete swiped
            var item = sender as SwipeItem;
            var athlete = item.CommandParameter as AthleteModel;
            int idnum = athlete.id;


            // deletes the athlete from the database
            string cmd = $"DELETE FROM rosters_cuw WHERE id = @val;";
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
                updateAthletesShown(); // updates the view
            }
            getLastID(); // gets the id of the last athlete
        }

        // gets the id of the last athlete
        public void getLastID()
        {

            SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            connection.Open();

            ArrayList cuwathletes = new ArrayList();
            string commandText = $"SELECT * FROM {TABLE_NAME} ORDER BY 'id' DESC";
            using (SqlCommand cmd = new SqlCommand(commandText, connection))
            {
                athleteLen = 0;

                using (SqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        AthleteModel athlete = RosterDatabase.readAthletes(reader);
                        athleteLen = athlete.id;
                        connection.Close();
                        break;
                    }
                
            }
        }
    }
}
