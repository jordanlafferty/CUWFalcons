using System;
using System.Threading.Tasks;
using CUWFalcons.Models;
using System.Data.SqlClient;

namespace CUWFalcons
{
    public class RosterDatabase
    {

        public SqlConnection connection;
        private const string TABLE_NAME = "rosters_cuw";

        public RosterDatabase()
        {


        }

        // adds a new athlete to the database
        public async Task addNewAthleteDB(AthleteModel athlete)
        {
            SqlConnection connection = new SqlConnection("Server=tcp:cuwfalcons.database.windows.net,1433;Initial Catalog=cuwfalcons;Persist Security Info=False;User ID=cuwfalcons;Password=Falcons9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            connection.Open();

            string commandText = $"INSERT INTO {TABLE_NAME} (id, fname, lname, number, sport, position, year, hometown, state, major) VALUES (@id, @fname, @lname, @number, @sport, @position, @year, @hometown, @state, @major";
            using (var cmd = new SqlCommand(commandText, connection))
            {
                cmd.Parameters.AddWithValue("id", athlete.id);
                cmd.Parameters.AddWithValue("fname", athlete.fName);
                cmd.Parameters.AddWithValue("lname", athlete.lName);
                cmd.Parameters.AddWithValue("number", athlete.number);
                cmd.Parameters.AddWithValue("sport", athlete.sport);
                cmd.Parameters.AddWithValue("position", athlete.lName);
                cmd.Parameters.AddWithValue("year", athlete.number);
                cmd.Parameters.AddWithValue("hometown", athlete.sport);
                cmd.Parameters.AddWithValue("state", athlete.lName);
                cmd.Parameters.AddWithValue("major", athlete.number);
                await cmd.ExecuteNonQueryAsync();
                connection.Close();
            }
        }



        // reads the information from the database to get athletes
        public static AthleteModel readAthletes(SqlDataReader reader)
        {
            int? id = reader["id"] as int?;
            string fname = reader["fname"] as string;
            string lname = reader["lname"] as string;
            string sport = reader["sport_code"] as string;
            Nullable<int> number = reader["number"] as int?;
            string position = reader["position"] as string;
            string year = reader["year"] as string;
            string city = reader["hometown"] as string;
            string state = reader["state"] as string;
            string major = reader["major"] as string;

            AthleteModel athlete = new AthleteModel(id.Value, sport, fname, lname, number, position, year, city, state, major)
            {
                id = id.Value,
                fName = fname,
                lName = lname,
                sport = sport,
                number = number,
                position = position,
                year = year,
                hometown = city,
                state = state,
                major = major
            };

            return athlete;
           
        }

        public static int getNextId(SqlDataReader reader)
        {
            int? id = reader["id"] as int?;
            return id.Value;
        }

    }
}
