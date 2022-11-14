using CUWFalcons.Models;
using Xamarin.Forms;
using System;
using Xamarin.Forms.Xaml;
using System.Data.SqlClient;
using System.Collections;

namespace CUWFalcons.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class ModifyAthletePage : ContentPage
    {
        ArrayList currSports = new ArrayList(); // list of all SportModels
        ArrayList sportCodes = new ArrayList(); // list of all the sport codes to make sure that user-entered sportcode exists
        AthleteModel _athlete; // this will hold the athlete that will potentially be edited
        public int athleteNum; // the last id used for an athlete
        
        // this will add a new athlete taking in the last id
        public ModifyAthletePage(int num)
        {
            InitializeComponent();

            // sets the sportcode picker and the year abreviation picker
            setSports();
            setYearPicker();
            athleteNum = num;
        }

        // constructor if the user wants to edit an athlete
        public ModifyAthletePage(AthleteModel athlete)
        {
            
            InitializeComponent();

            // sets the sportcode picker and the year abreviation picker
            setSports();
            setYearPicker();


            Title = "Edit Athlete Information"; //changes display Title


            // inserts all the information from the athlete passed into the form so it can be edited
            _athlete = athlete; 
            fNameEntry.Text = athlete.fName;
            lNameEntry.Text = athlete.lName;
            numberEntry.Text = athlete.number.ToString();
            sportCodeSelection.SelectedIndex = checkSportCode(athlete);
            positionEntry.Text = athlete.position;
            yearSelection.SelectedIndex = retrieveAthleteYear(athlete);
            cityEntry.Text = athlete.hometown;
            stateEntry.Text = athlete.state;
            majorEntry.Text = athlete.major;
        }


        // sets the options for the picker determining what year the athlete is
        void setYearPicker()
        {
            yearSelection.Items.Add("Fr.");
            yearSelection.Items.Add("So.");
            yearSelection.Items.Add("Jr.");
            yearSelection.Items.Add("Sr.");
        }

        // if conditions are met the athlete is saved in the database
        async void saveAthlete_Clicked(object sender, EventArgs e)
        {
            // checks to make sure a new athlete has at least a name, year and sport
            // also makes sure that rhe sports code is valid
            if (string.IsNullOrWhiteSpace(fNameEntry.Text))
            {
                await DisplayAlert("Invalid", "The athlete's first name is required.", "OK");
            }
            else if (sportCodeSelection.SelectedIndex == -1)
            {
                await DisplayAlert("Invalid", "The athlete must have a sport.", "OK");
            }
            else if (yearSelection.SelectedIndex == -1)
            {
                await DisplayAlert("Invalid", "The athlete must have a class.\nEither Fr., So., Jr., or Sr.", "OK");
            }

            // the athlete will be edited
            else if (_athlete != null)
            {
                // determines if the athlete has a number so it can be formatted in the database
                Nullable<int> num;
                if (string.IsNullOrWhiteSpace(numberEntry.Text))
                {
                    num = null;
                }
                else
                {
                    string theNum = numberEntry.Text;
                    try { num = Convert.ToInt16(theNum); }
                    catch { num = null; }
                }
                checkNULLs(); // checks to see if there are any NULL values so they can be correctly inputted into db
                editAthlete(_athlete, num); // edits the athlete in the database

            }
            // a new athlete will be added
            else
            {
                // determines if the athlete has a number so it can be formatted in the database
                Nullable<int> num;
                if (string.IsNullOrWhiteSpace(numberEntry.Text))
                {
                    num = null;
                }
                else
                {
                    string theNum = numberEntry.Text;
                    try { num = Convert.ToInt16(theNum); }
                    catch { num = null; }
                }
                
                checkNULLs();// checks to see if there are any NULL values so they can be correctly inputted into db
                addNewAthlete(athleteNum, num); // addss the athlete in the database
            }
        }

        // gets the index of what year was selected for a particular athlete so the picker can reflect that when making edits
        int retrieveAthleteYear(AthleteModel athlete)
        {
            if (athlete.year == "Fr.")
            {
                return 0;
            }
            else if (athlete.year == "So.")
            {
                return 1;
            }
            else if (athlete.year == "Jr.")
            {
                return 2;
            }
            else if (athlete.year == "Sr.")
            {
                return 3;
            }
            return -1;
        }


        // checks to see if there are any NULL values so they can be correctly inputted into db
        void checkNULLs()
        {
            if (lNameEntry.Text == null)
            {
                lNameEntry.Text = "";
            }
            if (positionEntry.Text == null)
            {
                positionEntry.Text = "";
            }
            if (cityEntry.Text == null)
            {
                cityEntry.Text = "";
            }
            if (stateEntry.Text == null)
            {
                stateEntry.Text = "";
            }
            if (majorEntry.Text == null)
            {
                majorEntry.Text = "";
            }
        }


        // goes to a page where all the sport codes are shown for the user
        async void sportCodes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SportCodePage());
        }



        //makes sure that the user enters a correct sport code when entering or editing an athlete
        int checkSportCode(AthleteModel athlete)
        {

            for (int i = 0; i < sportCodes.Count; i++)
            {
                if (String.Equals(sportCodes[i], athlete.sport))
                {
                    return i;
                }
            }

            return -1;
        }

        // passes in the sports that display on the sportcode picker
        public void setSports()
        {
            ArrayList theSports = SportCodePage.read();
            for (int i = 0; i < theSports.Count - 1; i++)
            {
                SportModel sport = (SportModel)theSports[i];
                sportCodes.Add(sport.sportCode);
                sportCodeSelection.Items.Add(sport.sportCode);
                currSports.Add(sport);
            }

        }





        // takes the next int available for the id and creates a new athlete based on the parameters added by the user
        // the new athlete is then added to the database
        async void addNewAthlete(int athleteNum, Nullable<int> num)
        {

            string connection = "Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            
            bool checkNum = true;


            // changes the  sql command based on if the number is null or not
            string cmdString = $"INSERT INTO rosters_cuw VALUES (@val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9, @val10);";
            if (num == null)
            {
                cmdString = $"INSERT INTO rosters_cuw VALUES (@val1, @val2, @val3, NULL, @val5, @val6, @val7, @val8, @val9, @val10);";
                checkNum = false;
            }
         

            // adds the athlete to the database based if the number is NULL
            if (checkNum == false)
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(cmdString, conn))
                    {
                        comm.Parameters.AddWithValue("@val1", athleteNum + 1);
                        comm.Parameters.AddWithValue("@val3", lNameEntry.Text);
                        comm.Parameters.AddWithValue("@val2", fNameEntry.Text);
                        comm.Parameters.AddWithValue("@val5", sportCodeSelection.Items[sportCodeSelection.SelectedIndex].ToString());
                        comm.Parameters.AddWithValue("@val6", positionEntry.Text);
                        comm.Parameters.AddWithValue("@val7", yearSelection.Items[yearSelection.SelectedIndex].ToString());
                        comm.Parameters.AddWithValue("@val8", cityEntry.Text);
                        comm.Parameters.AddWithValue("@val9", stateEntry.Text);
                        comm.Parameters.AddWithValue("@val10", majorEntry.Text);

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
                        comm.Parameters.AddWithValue("@val1", athleteNum + 1);
                        comm.Parameters.AddWithValue("@val3", lNameEntry.Text);
                        comm.Parameters.AddWithValue("@val2", fNameEntry.Text);
                        comm.Parameters.AddWithValue("@val4", num);
                        comm.Parameters.AddWithValue("@val5", sportCodeSelection.Items[sportCodeSelection.SelectedIndex].ToString());
                        comm.Parameters.AddWithValue("@val6", positionEntry.Text);
                        comm.Parameters.AddWithValue("@val7", yearSelection.Items[yearSelection.SelectedIndex].ToString());
                        comm.Parameters.AddWithValue("@val8", cityEntry.Text);
                        comm.Parameters.AddWithValue("@val9", stateEntry.Text);
                        comm.Parameters.AddWithValue("@val10", majorEntry.Text);

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


        // this takes in an athlete the user choose to edit and uses the SQL command update to change the athlete info in the database
        async void editAthlete(AthleteModel _athlete, Nullable<int> num)
        {
            string connection = "Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            bool checkNum = true;
            int _id = _athlete.id;



            // changes the  sql command based on if the number is null or not
            string cmdString = $"UPDATE rosters_cuw SET lname = @val1, fname = @val2, number = @val3, sport_code = @val4, position = @val6, year = @val7, hometown = @val8, state = @val9, major = @val10   WHERE id = @val5;";
            if (num == null)
            {
                cmdString = $"UPDATE rosters_cuw SET lname = @val1, fname = @val2, sport_code = @val4, position = @val6, year = @val7, hometown = @val8, state = @val9, major = @val10   WHERE id = @val5;";
                checkNum = false;

            }



            // edits the athlete to the database based if the number is NULL
            if (checkNum == false)
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    using (SqlCommand comm = new SqlCommand(cmdString, conn))
                    {
                        comm.Parameters.AddWithValue("@val5", _id); 
                        comm.Parameters.AddWithValue("@val2", fNameEntry.Text);
                        comm.Parameters.AddWithValue("@val1", lNameEntry.Text);
                        comm.Parameters.AddWithValue("@val4", sportCodeSelection.Items[sportCodeSelection.SelectedIndex].ToString());
                        comm.Parameters.AddWithValue("@val6", positionEntry.Text);
                        comm.Parameters.AddWithValue("@val7", yearSelection.Items[yearSelection.SelectedIndex].ToString());
                        comm.Parameters.AddWithValue("@val8", cityEntry.Text);
                        comm.Parameters.AddWithValue("@val9", stateEntry.Text);
                        comm.Parameters.AddWithValue("@val10", majorEntry.Text);
                        try
                        {
                            conn.Open();
                            comm.ExecuteNonQuery();

                        }
                        catch (SqlException e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    using (SqlCommand comm = new SqlCommand(cmdString, conn))
                    {
                        comm.Parameters.AddWithValue("@val5", _id);
                        comm.Parameters.AddWithValue("@val2", fNameEntry.Text);
                        comm.Parameters.AddWithValue("@val1", lNameEntry.Text);
                        comm.Parameters.AddWithValue("@val3", num);
                        comm.Parameters.AddWithValue("@val4", sportCodeSelection.Items[sportCodeSelection.SelectedIndex].ToString());
                        comm.Parameters.AddWithValue("@val6", positionEntry.Text);
                        comm.Parameters.AddWithValue("@val7", yearSelection.Items[yearSelection.SelectedIndex].ToString());
                        comm.Parameters.AddWithValue("@val8", cityEntry.Text);
                        comm.Parameters.AddWithValue("@val9", stateEntry.Text);
                        comm.Parameters.AddWithValue("@val10", majorEntry.Text);
                        try
                        {
                            conn.Open();
                            comm.ExecuteNonQuery();

                        }
                        catch (SqlException e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }


            //closes the page
            await Navigation.PopAsync();

        }
    }
}
