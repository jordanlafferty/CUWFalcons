using System;
using System.Collections.Generic;
using System.Data;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace CUWFalcons.Models
{
    public class AthleteModel
    {

        public int id { get; set; }
        public string sport { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public Nullable<int> number { get; set; }
        public string position { get; set; }
        public string hometown { get; set; }
        public string state { get; set; }
        public string year { get; set; }
        public string major { get; set; }
        public string fullNameDisplay { get; set; } // used to display the athlete's fullname, number & position in the roster page
        public string yearHometownMajorDisplay { get; set; } // used to display the athlete's year, major & hometown in the roster page


        public AthleteModel(int idnum, string sportcode, string fname, string lname, Nullable<int> num, string pos, string yr, string city, string st, string study)
        {
            id = idnum;
            sport = sportcode;
            fName = fname;
            lName = lname;
            number = num;
            position = pos;
            hometown = city;
            state = st;
            year = yr;
            major = study;
            setDisplays();

        }

       
        // adjusts the information that will be displayed for each athlete depending on their information
        public void setDisplays()
        {
            // sets full name display
            if (number == null)
            {
                if (position == null | position == "")
                {
                    fullNameDisplay = fName + " " + lName;
                }
                else
                {
                    fullNameDisplay = fName + " " + lName + "   " + position;
                }
            }
            else
            {
                if (position == null | position == "")
                {
                    fullNameDisplay = number.ToString() + "   " + fName + " " + lName;
                }
                else
                {
                    fullNameDisplay = number.ToString() + "   " + fName + " " + lName + "   " + position;
                }

            }

            // sets hometown, major and year display
            if (hometown == null | hometown == "" | state == null | state == "")
            {

                if (major == null | major == "")
                {
                    yearHometownMajorDisplay = "Year: " + year;
                }
                else
                {
                    yearHometownMajorDisplay = "Year: " + year + "\nMajor: " + major;
                }
            }
            else
            {
                if (major == null | major == "")
                {
                    yearHometownMajorDisplay = "Year: " + year + "\nHometown: " + hometown + ", " + state;
                }
                else
                {
                    yearHometownMajorDisplay = "Year: " + year + "\nMajor: " + major + "\nHometown: " + hometown + ", " + state;
                }
                

            }


        }

    }
}

