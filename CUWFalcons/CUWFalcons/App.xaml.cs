using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CUWFalcons.Services;
using CUWFalcons.Views;
using System.IO;
using Npgsql;
using System.Data;
using System.Diagnostics;

namespace CUWFalcons
{
    public partial class App : Application
    {
        private static SQLHelper x;
        private static string Host = "localhost";
        private static string User = "newuser";
        private static string DBname = "cuwfalcons";
        private static string Password = "admin";
        private static string Port = "5432";


        public static SQLHelper cuwAthletesDB
        {
            get
            {
                if(x == null)
                {
                    x = new SQLHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "cuwAthletes.db3"));
                }
                return x;
            }

        }


        private static RosterDatabase db;

        public static RosterDatabase rosterDB
        {

            get
            {
                if (db == null)
                {
                    db = new RosterDatabase("@Server=localhost;Port=5432;User Id=postgres;Password=admin; Database=cuwfalcons");
                }
                return db;
            }

        }

        private static void TestConnection()
        {
            try
            {
                var test = "Server=localhost;Port=5432;Database=cuwfalcons;User Id=newuser;Password=password;";
                using (NpgsqlConnection con = new NpgsqlConnection(test))
                {
                    try 
                    {
                        con.OpenAsync();
                        Debug.WriteLine("no error");
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }
            }
            catch
            {
                Debug.WriteLine("error");
            }
            
        }


      

        public App ()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();

        }

        protected override void OnStart ()
        {
            start();
            async void start()
            {
                // switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");

                TestConnection();
            }

            
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

