using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CUWFalcons.Models;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections;
using static Azure.Core.HttpHeader;
using Xamarin.Forms;

namespace CUWFalcons
{
    public class CalendarDatabase
    {
        
        public SqlConnection connection;
        private const string TABLE_NAME = "sports";

        public CalendarDatabase()
        {

        }

        // gets all the event information for the SQLDataReader and then creates a new CalendarEventModel with the information
        public static CalendarEventModel readEvents(SqlDataReader reader)
        {
            
            try {
                int? id = reader["id"] as int?;
                string sport = reader["sportcode"] as string;
                DateTime? date = reader["date"] as DateTime?;
                TimeSpan? time = reader["time"] as TimeSpan?;
                string away = reader["away"] as string;
                string home = reader["home"] as string;
                string type = reader["type"] as string;
                string site = reader["site"] as string;
                string notes = reader["notes"] as string;
                string attendance = reader["attendance"] as string;


                // new event that will be added to an array on the page that called this method
                CalendarEventModel theEvent = new CalendarEventModel(id.Value, sport, date.Value, time, away, home, type, site, notes, attendance)
                {
                    id = id.Value,
                    sport = sport,
                    date = date.Value,
                    time = time,
                    awayteam = away,
                    hometeam = home,
                    gametype = type,
                    site = site,
                    notes = notes,
                    attendance = attendance

                };
                return theEvent;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // should not get to this code but needs a returned CalendarEventModel
            return new CalendarEventModel(-1, "", DateTime.Today, TimeSpan.Zero, "", "", "", "", "", ""); 
            
        }


    }
}

