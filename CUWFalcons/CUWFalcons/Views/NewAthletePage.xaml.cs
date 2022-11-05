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
       
        public NewAthletePage()
        {
            InitializeComponent();
        }

        async void saveAthlete_Clicked(object sender, EventArgs e)
        {
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
            // creates a new athlete with what data was given
            AthleteModel newAthlete = new Models.AthleteModel
            {
                fName = fNameEntry.Text,
                lName = lNameEntry.Text,
                sport = sportEntry.Text,
                number = numberEntry.Text
            };
            await App.rosterDB.addNewAthleteDB(newAthlete);

            await Navigation.PopAsync();
        }
    }
}

