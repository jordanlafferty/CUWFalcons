using System;
using System.Collections.Generic;
using System.Data;
using SQLite;

namespace CUWFalcons.Models
{
    public class CalendarEventModel
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [MaxLength(50)]

        public string sport { get; set; }
        public string location { get; set; }
        public string awayteam { get; set; }
        public string hometeam { get; set; }
        public string date { get; set; }

        public CalendarEventModel()
        {

        }
    }
}

