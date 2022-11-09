using System;
using SQLite;
using System.Threading.Tasks;
using System.Collections.Generic;
using CUWFalcons.Models;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections;

namespace CUWFalcons
{
    public class RosterDatabase
    {
        // establishes an SQLite connection
       // private readonly SQLiteAsyncConnection db;
        public SqlConnection connection;

        private const string TABLE_NAME = "rosters";

        public RosterDatabase(string dbPath)
        {

            SqlConnection connection = new SqlConnection(dbPath);
            connection.Open();
                
        }

        public async Task addNewAthleteDB(AthleteModel athlete)
        {
            connection.Open();
            string commandText = $"INSERT INTO {TABLE_NAME} (id, fname, lname, number, sport) VALUES (@id, @fname, @lname, @number, @sport)";
            using (var cmd = new SqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", athlete.id);
                cmd.Parameters.AddWithValue("fname", athlete.fName);
                cmd.Parameters.AddWithValue("lname", athlete.lName);
                cmd.Parameters.AddWithValue("number", athlete.number);
                cmd.Parameters.AddWithValue("sport", athlete.sport);

                await cmd.ExecuteNonQueryAsync();
            }
        }



        public ArrayList read()
        {
            try
            {
                connection.Open();
            }
            catch
            {

            }
            
            string commandText = $"SELECT * FROM {TABLE_NAME}";
            using (SqlCommand cmd = new SqlCommand(commandText, connection))
            {
                ArrayList cuwathletes = new ArrayList();
                using (SqlDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        AthleteModel athlete = readAthletes(reader);
                        cuwathletes.Add(athlete);
                    }
                return cuwathletes;
            }

        }


        public static AthleteModel readAthletes(SqlDataReader reader)
        {
            int? id = reader["id"] as int?;
            string fname = reader["fname"] as string;
            string lname = reader["lname"] as string;
            string sport = reader["sport"] as string;
            string number = reader["number"] as string;

            AthleteModel athlete = new AthleteModel
            {
                id = id.Value,
                fName = fname,
                lName = lname,
                sport = sport,
                number = number
            };
            return athlete;
        }



    }
}

