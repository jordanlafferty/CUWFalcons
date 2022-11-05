using System;
using SQLite;
using System.Threading.Tasks;
using System.Collections.Generic;
using CUWFalcons.Models;
using Npgsql;

namespace CUWFalcons
{
    public class SQLHelper
    {
        // establishes an SQLite connection
        private readonly SQLiteAsyncConnection db;
       // public string ConnectionString = "Server=localhost; Port=5432; User Id=admin; Password=123456; Database = cuwfalcons";


        public SQLHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            // creates a table based of the data model, Athlete Model
            db.CreateTableAsync<AthleteModel>();
        }


        // adds/creates a new athlete, returns numbers of rows added
        public Task<int> createAthlete (AthleteModel athlete)
        {
            return db.InsertAsync(athlete);
        }

        // reads data on the athletes and returns a queryable interface
        public Task<List<AthleteModel>>readAthlete()
        {
            return db.Table<AthleteModel>().ToListAsync();
        }


        // makes changes to any athlete and returns the number of rows changed
        public Task<int> updateAthlete(AthleteModel athlete)
        {
            return db.UpdateAsync(athlete);
        }


        // deletes an athlete and returns the number of rows of athlete(s) deleted
        public Task<int> deleteAthlete(AthleteModel athlete)
        {
            return db.DeleteAsync(athlete);
        }



    }
}

