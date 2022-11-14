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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CUWFalcons.Models;

namespace CUWFalcons
{
    public partial class App : Application
    {

        public static string currAccount; // current account type
        public static string currUser; // current username
        
      
        // sets the user and what type of account they have
        public static void setUser(string account, string username)
        {
            currAccount = account;
            currUser = username;

        }

        // returns the current username
        public static string getUser()
        {
            return currUser;
        }

        // returns the current account type
        public static string getAccount()
        {
            return currAccount;
        }

        public App ()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzczMzI0QDMyMzAyZTMzMmUzMG54TEFwUzNQN2RYVXNsVUtyQk9Xb2FabjJHSGNwTlpjelBQNk1TNDRMOUk9");
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

