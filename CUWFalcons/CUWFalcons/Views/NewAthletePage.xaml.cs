using CUWFalcons.Models;
using CUWFalcons.ViewModels;
using Xamarin.Forms;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms.Xaml;

namespace CUWFalcons.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class NewAthletePage : ContentPage
    {
        public int athleteNum;
        public NewAthletePage()
        {
            InitializeComponent();
        }

        async void saveAthlete_Clicked(object sender, EventArgs e)
        {
            // checks to make sure a new athlete has at least a name and a sport
            if(string.IsNullOrWhiteSpace(fNameEntry.Text))
            {
                await DisplayAlert("Invalid", "The athlete's first name is required", "OK");
            }
            else if (string.IsNullOrWhiteSpace(sportEntry.Text))
            {
                await DisplayAlert("Invalid", "The athlete must have a sport", "OK");
            }
            else
            {
                addNewAthlete();
            }
        }

        async void addNewAthlete()
        {
            athleteNum += 1;
            // creates a new athlete with what data was given
            AthleteModel newAthlete = new AthleteModel(athleteNum, sportEntry.Text, fNameEntry.Text, lNameEntry.Text, numberEntry.Text);
            // adds athlete to the database
            await App.db.addNewAthleteDB(newAthlete);

            await Navigation.PopAsync();
        }
    }
}

