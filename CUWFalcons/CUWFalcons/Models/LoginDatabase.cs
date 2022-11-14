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

namespace CUWFalcons
{
    public class LoginDatabase
    {
        
        public SqlConnection connection;
        private const string TABLE_NAME = "login_cuw";

        public LoginDatabase()
        {

        }

        // checks to see if username and password match and returns what kind of account they have (admin, guest, student)
        public static string checkLogin(string username, string password)
        {
            SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            string commandText = $"SELECT * FROM {TABLE_NAME} WHERE username=@value;";

            connection.Open();

            try
            {
                using SqlCommand cmd = new SqlCommand(commandText, connection);
                cmd.Parameters.AddWithValue("@value", username);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    LoginModel theUser = readUser(reader); // gets the the user who is attempting to log in
                    connection.Close();
                    if (password == theUser.password) // correct login
                    {
                        return theUser.accountType;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return "fail";
        }


        // reads the information and creates a User (LoginModel)
        public static LoginModel readUser(SqlDataReader reader)
        {
            string username = reader["username"] as string;
            string password = reader["password"] as string;
            string accountType = reader["account"] as string;


            LoginModel user = new LoginModel(username, password, accountType)
            {
                username = username,
                password = password,
                accountType = accountType,

            };

            return user;
        }


    }
}
