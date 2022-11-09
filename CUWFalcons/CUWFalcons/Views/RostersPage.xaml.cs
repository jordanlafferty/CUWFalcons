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



            /*players = new ObservableCollection<RosterModel>
            {
                new RosterModel { fName= "Jordan", lName = "Lafferty", number = 9, year = "Senior", hometown= "Surprise, Arizona", position= "M"},
                new RosterModel { fName = "Isabel", lName = "Downs", number = 21, year = "Senior", hometown = "Menomonee Falls, Wisconsin", position = "F"}
            };
            rosterView.ItemsSource = players;*/

            
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

            //getFile();




            /*void getFile()
            {
                string filePath = @"/Users/jordanlafferty/Projects/CUWFalcons/CUWFalcons/CUWFalcons/WSOCRoster.csv";
                StreamReader reader = null;
                if (File.Exists(filePath))
                {
                    reader = new StreamReader(File.OpenRead(filePath));
                    List<string> listA = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        foreach (var item in values)
                        {
                            listA.Add(item);
                        }
                        foreach (var coloumn1 in listA)
                        {
                            .SetBinding(ItemsView.ItemsSourceProperty, coloumn1);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("File doesn't exist");
                }
                Console.ReadLine();
            }

            var file = await FilePicker.PickAsync();
            var stream = await file.OpenReadAsync();
            using (var reader = new System.IO.StreamReader(stream))
            {
                if (reader != null)
                {
                    using (var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture))
                    {
                        while (csvReader.Read())
                        {
                            list.Add(new TestModel
                            {
                                ID = csvReader.GetField<string>(0),
                                content = csvReader.GetField<string>(1)
                            });
                        }
                    }
                }
            }
            listview.ItemsSource = list;*/
        
