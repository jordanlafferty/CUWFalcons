using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using CUWFalcons.Models;
using CUWFalcons.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CUWFalcons.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewSportPage : ContentPage
    {
        SportModel _Sport;

        // constructor to add a new sport
        public NewSportPage()
        {
            InitializeComponent();
            
        }

        // constructor that takes in a sport to be updates
        public NewSportPage(SportModel theSport)
        {
            InitializeComponent();

            //sets the items in the sport page so it can be edited
            _Sport = theSport;
            sportCodeEntry.Text = theSport.sportCode;
            displayNameEntry.Text = theSport.displayName;

        }


        // check before adding or editing a sport
        async void saveSport_Clicked(Object sender, EventArgs e)
        {

            // makes sure user entered a sportcode and a display name
            if (string.IsNullOrWhiteSpace(sportCodeEntry.Text))
            {
                await DisplayAlert("Invalid", "A sportcode is required.", "OK");
            }
            else if (string.IsNullOrWhiteSpace(displayNameEntry.Text))
            {
                await DisplayAlert("Invalid", "The display name is required", "OK");
            }
            // goes to edit sport
            else if(_Sport != null)
            {
                editSport();
            }
            // goes to add sport
            else
            {
                addNewSport();
            }
            
        }


        // adds new sport to the database
        async void addNewSport()
        {
            string cmdString = $"INSERT INTO sports VALUES (@val1, @val2);";
            string connection = "Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(cmdString, conn))
                {
                    comm.Parameters.AddWithValue("@val1", sportCodeEntry.Text);
                    comm.Parameters.AddWithValue("@val2", displayNameEntry.Text);
                    

                    try
                    {

                        comm.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        await DisplayAlert("Failed", "That Sport Code Already Exists. Please retry.", "OK");
                        Console.WriteLine(e);
                    }
                }
                conn.Close();
            }


            // prompts the user to restart so that the changes can be made
            while (true)
            {
                await DisplayAlert("Restart", "Please restart the App to view changes", "OK");
            }
        }


        // edits the sport and all events and athletes related to that sport
        async void editSport()
        {
            // editing sport
            string cmdString = $"UPDATE sports SET sportcode = @val1, displayname = @val2 WHERE sportcode = @val3;";
            string connection = "Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(cmdString, conn))
                {
                    comm.Parameters.AddWithValue("@val1", sportCodeEntry.Text);
                    comm.Parameters.AddWithValue("@val2", displayNameEntry.Text);
                    comm.Parameters.AddWithValue("@val3", _Sport.sportCode);


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



            // updating events sportcode
            cmdString = $"UPDATE calendar_cuw SET sportcode = @val1 WHERE sportcode = @val2;";
             using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(cmdString, conn))
                {
                    comm.Parameters.AddWithValue("@val1", sportCodeEntry.Text);
                    comm.Parameters.AddWithValue("@val2", _Sport.sportCode);


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


            // updating rosters sport code
            cmdString = $"UPDATE rosters_cuw SET sport_code = @val1 WHERE sport_code = @val2;";
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(cmdString, conn))
                {
                    comm.Parameters.AddWithValue("@val1", sportCodeEntry.Text);
                    comm.Parameters.AddWithValue("@val2", _Sport.sportCode);


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


            // prompts the user to restart so that the changes can be made
            while (true)
            {
                await DisplayAlert("Restart", "Please restart the App to view changes", "OK");
            }
        }
    }
}


