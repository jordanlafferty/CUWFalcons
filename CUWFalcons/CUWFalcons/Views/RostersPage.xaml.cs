using Xamarin.Forms;
using CUWFalcons.ViewModels;
using System;
using Picker = Xamarin.Forms.Picker;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Globalization;
using Xamarin.Essentials;
using CUWFalcons.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Collections;
using System.Data.SqlClient;

namespace CUWFalcons.Views
{
    public partial class RostersPage : ContentPage
    {

        private const string TABLE_NAME = "rosters";

        public RostersPage()
        {
            InitializeComponent();

            sportSelection.Items.Add("Acrobatics and Tumbling");
            sportSelection.Items.Add("Baseball");
            sportSelection.Items.Add("Men's Baskeball");
            sportSelection.Items.Add("Women's Basketball");
            sportSelection.Items.Add("Cheerleading");
            sportSelection.Items.Add("Women's Soccer");
            sportSelection.Items.Add("Softball");
            sportSelection.Items.Add("Men's Tennis");
            sportSelection.Items.Add("Women's Tennis");

            
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                rosterView.ItemsSource = read();
            }
            catch
            {

            }
        }

        private async void addAthleteToolbar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewAthletePage());
        }

        public ArrayList read()
        {

             SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
             connection.Open();
           

            string commandText = $"SELECT * FROM {TABLE_NAME}";
            using (SqlCommand cmd = new SqlCommand(commandText, connection))
            {
                ArrayList cuwathletes = new ArrayList();
                using (SqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        AthleteModel athlete = RosterDatabase.readAthletes(reader);
                        cuwathletes.Add(athlete);
                    }
                return cuwathletes;
            }

        }
    }
}

