using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CUWFalcons.Services;
using CUWFalcons.Views;
using System.IO;

namespace CUWFalcons
{
    public partial class App : Application
    {
        private static SQLHelper db;

        public static SQLHelper cuwAthletesDB
        {
            get
            {
                if(db == null)
                {
                    db = new SQLHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "cuwAthletes.db3"));
                }
                return db;
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
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
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

