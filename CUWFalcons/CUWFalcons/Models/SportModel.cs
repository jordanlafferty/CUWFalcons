using System;
using System.Collections.Generic;
using System.Data;
using Xamarin.Forms;

namespace CUWFalcons.Models
{
    public class SportModel
    {

        public string sportCode { get; set; }
        public string displayName { get; set; }


        public SportModel(string code, string name)
        {
            sportCode = code;
            displayName = name;
        }
    }
}

