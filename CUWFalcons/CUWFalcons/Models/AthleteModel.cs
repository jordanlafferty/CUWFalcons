using System;
using System.Collections.Generic;
using System.Data;
using SQLite;

namespace CUWFalcons.Models
{
    public class AthleteModel
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [MaxLength(50)]

        public string sport { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string number { get; set; }
        public string position { get; set; }
        public string hometown { get; set; }
        public string hschool { get; set; }
        //public string major { get; set; }
        public string year { get; set; }


        public AthleteModel()
        {
           
        }
    }
}

