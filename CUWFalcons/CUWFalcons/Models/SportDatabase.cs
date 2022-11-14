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
    public class SportDatabase
    {
        public SqlConnection connection;

        private const string TABLE_NAME = "sports";

        public SportDatabase()
        {



        }

        // adds a new sport to the database
        public async Task addNewSportDB(SportModel sport)
        {
            SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            connection.Open();

            string commandText = $"INSERT INTO {TABLE_NAME} VALUES (@sportcode @displayname)";
            using (var cmd = new SqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("sportcode", sport.sportCode);
                cmd.Parameters.AddWithValue("displayname", sport.displayName);

                await cmd.ExecuteNonQueryAsync();
                connection.Close();
            }

        }

        // gets the display name from the given sportcode
        public static string getDisplaySport(string sportCode)
        {
            SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            connection.Open();
            string displayName = "";
            string commandText = $"SELECT * FROM {TABLE_NAME} WHERE sportcode=@val;";

            try
            {
                using (SqlCommand cmd = new SqlCommand(commandText, connection))
                {
                    cmd.Parameters.AddWithValue("@val", sportCode);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            SportModel theSport = readSports(reader);
                            
                            displayName = theSport.displayName;
                        }
                    connection.Close();
                    return displayName;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return "";
        }


        // reads in the sports from the database with a SQLDataReader
        public static SportModel readSports(SqlDataReader reader)
        {
            string code = reader["sportcode"] as string;
            string name = reader["displayname"] as string;


            SportModel sport = new SportModel(code, name)
            {
                sportCode = code,
                displayName = name,

            };

            return sport;
        }


    }
}
