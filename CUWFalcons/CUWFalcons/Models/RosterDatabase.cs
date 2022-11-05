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

namespace CUWFalcons
{
    public class RosterDatabase
    {
        // establishes an SQLite connection
       // private readonly SQLiteAsyncConnection db;
        private NpgsqlConnection connection;

        private const string TABLE_NAME = "rosters";

        public RosterDatabase(string dbPath)
        {


            connection = new NpgsqlConnection(dbPath);
            //using var con = new NpgsqlConnection(dbPath);

            try
            {

                connection.Open();
                ConnectionState conState = connection.State;

                if (conState == ConnectionState.Closed || conState == ConnectionState.Broken)
                {
                    Debug.WriteLine("error");
                }
                else
                {
                    Debug.WriteLine("no error");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("<<< catch : " + ex.ToString());
            }
                
        }

        public async Task addNewAthleteDB(AthleteModel athlete)
        {
            string commandText = $"INSERT INTO {TABLE_NAME} (id, fname, lname, number, sport) VALUES (@id, @fname, @lname, @number, @sport)";
            using (var cmd = new NpgsqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", athlete.id);
                cmd.Parameters.AddWithValue("fname", athlete.fName);
                cmd.Parameters.AddWithValue("lname", athlete.lName);
                cmd.Parameters.AddWithValue("number", athlete.number);
                cmd.Parameters.AddWithValue("sport", athlete.sport);

                await cmd.ExecuteNonQueryAsync();
            }
        }



        public async Task<AthleteModel> read()
        {
            string commandText = $"SELECT * FROM {TABLE_NAME}";
            using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, connection))
            {

                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    while (await reader.ReadAsync())
                    {
                        AthleteModel athlete = readAthletes(reader);
                        return athlete;
                    }
            }
            return null;
        }


        public static AthleteModel readAthletes(NpgsqlDataReader reader)
        {
            int? id = reader["id"] as int?;
            string fname = reader["name"] as string;
            string lname = reader["minplayers"] as string;
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

