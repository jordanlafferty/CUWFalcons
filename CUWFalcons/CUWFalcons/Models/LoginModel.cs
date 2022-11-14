using System;
using System.Collections.Generic;
using System.Data;
using Xamarin.Forms;

namespace CUWFalcons.Models
{
    public class LoginModel
    {

        public string username{ get; set; }
        public string password { get; set; }
        public string accountType { get; set; }

        // creates a user
        public LoginModel(string user, string pass, string accType)
        {
            username = user;
            password = pass;
            accountType = accType;
        }
    }
}

