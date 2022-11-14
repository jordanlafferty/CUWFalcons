using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Xamarin.Forms;


namespace CUWFalcons.Models
{
    public class CalendarEventModel
    {
        
        public int id { get; set; }
        
        public string sport { get; set; }
        public string displaySport{ get; set; } // display name for the spirt
        public string location { get; set; }
        public string awayteam { get; set; }
        public string hometeam { get; set; }
        public Nullable <TimeSpan> time { get; set; }
        public string displayTime { get; set; } // reformats the timespan
        public string displayDate { get; set; } // reformats the datetime
        public string displayTeams { get; set; } // string of the team(s) competing 
        public DateTime date { get; set; }
        public string gametype { get; set; }
        public string site { get; set; } 
        public string notes { get; set; }
        public string attendance { get; set; }


        public CalendarEventModel(int idnum, string sportcode, DateTime theDate, Nullable<TimeSpan> theTime, string away, string home, string type, string site, string theNotes, string attendList)
        {
            id = idnum;
            sport = sportcode;
            location = site;
            date = theDate;
            time = theTime;
            hometeam = home;
            awayteam = away;
            gametype = type;
            notes = theNotes;
            attendance = attendList;
            displayDate = theDate.ToString("MMMM dd yyyy");

            convertDisplays();

        }


        public void convertDisplays()
        {
            
            DateTime thetime = DateTime.Today.Add((TimeSpan)time); //readjusts time so it can be formatted
            displayTime = thetime.ToString("hh:mm tt"); // fixes time format

            if(displayTime == "12:00 AM") // user is prompted to but 12:00 AM if no time is set, so becomes TBD
            {
                displayTime = "TBD";
            }

            displaySport = SportDatabase.getDisplaySport(sport); // gets the display name of the sport based on the 


            // displays the teams based on input (adds 'vs.' if both home & away are entered)
            displayTeams = "";
            if (hometeam == "Concordia Wisconsin" | hometeam == "Concordia University Wisconsin")
            {
                hometeam = "CUW";
            }
            if (awayteam == "Concordia Wisconsin" | awayteam == "Concordia University Wisconsin")
            {
                awayteam = "CUW";
            }

            if (hometeam != null & hometeam != " ")
            {
                displayTeams = hometeam;
                if (awayteam != null & awayteam != " ")
                {
                    displayTeams = hometeam + " vs. " + awayteam;
                }
            }


        }
       

    }
}

