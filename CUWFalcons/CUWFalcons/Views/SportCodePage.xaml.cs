using Xamarin.Forms;
using System;
using CUWFalcons.Models;
using System.Collections;
using System.Data.SqlClient;

namespace CUWFalcons.Views
{
    public partial class SportCodePage : ContentPage
    {
        private static string theConnectionString= "Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


        public SportCodePage()
        {
            InitializeComponent();

            // gets all the sports to be displayed
            ArrayList sports = read();
            sportCodesView.ItemsSource = sports;
        }

        // reads all the data from sports table to display content
        public static ArrayList read()
        {

            SqlConnection connection = new SqlConnection(theConnectionString);
            connection.Open();

            ArrayList sportList = new ArrayList();
            string commandText = $"SELECT * FROM sports";
            using (SqlCommand cmd = new SqlCommand(commandText, connection))
            {


                using (SqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        SportModel sport = SportDatabase.readSports(reader);
                        sportList.Add(sport);
                    }
                connection.Close();
            }
            
            return sportList;
        }


        
        // deletes a sport from the database
        public async void deleteSwipe_Invoked(object sender, EventArgs e)
        {
            // sport that was swiped on
            var item = sender as SwipeItem;
            var theSport = item.CommandParameter as SportModel;
            string sportCode = theSport.sportCode;

            // makes sure user wants to delete the sport
            bool answer = await DisplayAlert("Delete Sport", "Are you sure you want to delete this sport?\n All athletes and events with that sport will be deleted too.", "Yes", "No");

            // user deletes sport
            if (answer == true)
            {
                delete_Sport(sportCode); // deletes sport from database
            }
            // user does not delete sport
            else
            {
                await DisplayAlert("Notice", " Sport was not deleted", "OK");

            }
        }

        // deletes sport from database and all athletes and events for that sport
        async public void delete_Sport(string sportCode)
        {


            // deletes sport
            string cmd = $"DELETE FROM sports WHERE sportcode = @val;";

            using (SqlConnection conn = new SqlConnection(theConnectionString))
            {
                using (SqlCommand comm = new SqlCommand(cmd, conn))
                {
                    try
                    {
                        comm.Parameters.AddWithValue("@val", sportCode);
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


            // deletes events from the sport
            cmd = $"DELETE FROM calendar_cuw WHERE sportcode = @val;";

            using (SqlConnection conn = new SqlConnection(theConnectionString))
            {
                using (SqlCommand comm = new SqlCommand(cmd, conn))
                {
                    try
                    {
                        comm.Parameters.AddWithValue("@val", sportCode);
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


            // deletes athletes that play that sport
            cmd = $"DELETE FROM rosters_cuw WHERE sport_code = @val;";

            using (SqlConnection conn = new SqlConnection(theConnectionString))
            {
                using (SqlCommand comm = new SqlCommand(cmd, conn))
                {
                    try
                    {
                        comm.Parameters.AddWithValue("@val", sportCode);
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


            ArrayList sports = read();
            sportCodesView.ItemsSource = sports;

            // makes the user have to restart the app tp continue to use it
            while (true)
            {
                await DisplayAlert("Restart", "Please restart the App to view changes", "OK");
            }
        }


        // sends sport to be edited
        async public void editSwipe_Invoked(object sender, EventArgs e)
        {
            // sport swiped on
            var item = sender as SwipeItem;
            var theSport = item.CommandParameter as SportModel;

            // option to edit sport
            await Navigation.PushAsync(new NewSportPage(theSport));

            // updates view
            ArrayList sports = read();
            sportCodesView.ItemsSource = sports;
        }

        // goes back to settings page
        async void closeClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }



    }
            
}

