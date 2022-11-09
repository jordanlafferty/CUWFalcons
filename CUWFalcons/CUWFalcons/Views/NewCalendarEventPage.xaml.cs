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

    public partial class NewCalendarEventPage : ContentPage
    {
       
        public NewCalendarEventPage()
        {
            InitializeComponent();
        }

        async void saveEvent_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(hometeamEntry.Text))
            {
                await DisplayAlert("Invalid", "A home team is required", "OK");
            }
            else if (string.IsNullOrWhiteSpace(sportEntry.Text))
            {
                await DisplayAlert("Invalid", "A sport is required", "OK");
            }
         
            else
            {
                addNewEvent();
            }
        }

        async void addNewEvent()
        {
            // creates a new athlete with what data was given
            CalendarEventModel newEvent = new CalendarEventModel
            {
                sport = sportEntry.Text,
                hometeam = hometeamEntry.Text,
                awayteam = awayteamEntry.Text,
                date = dateEntry.ToString()
            };


            //make sure to add this to a newCalendarDB
            //await App.rosterDB.addNewAthleteDB(newEvent);

            await Navigation.PopAsync();
        }
    }
}

