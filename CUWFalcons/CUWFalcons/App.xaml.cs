using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CUWFalcons.Services;
using CUWFalcons.Views;
using System.IO;
using Npgsql;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using static System.Net.WebRequestMethods;

namespace CUWFalcons
{
    public partial class App : Application
    {


        public static RosterDatabase db;

        public static RosterDatabase rosterDB
        {

            get
            {
                if (db == null)
                {
                    db = new RosterDatabase("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                }
                return db;
            }

        }

        private static void TestConnection()
        {
            try
            {
                var connectionstring = "Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    try
                    {
                        connection.Open();
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

