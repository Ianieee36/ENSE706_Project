using System;
using Oracle.ManagedDataAccess.Client;

namespace FlightBookingSystem.Repository
{
    public class DatabaseConnection
    {
        private string connectionString;

        public DatabaseConnection()
        {
            connectionString = "User Id=system;Password=1234;Data Source=localhost:1521/XE;";
        }

        public OracleConnection GetConnection()
        {
            return new OracleConnection(connectionString);
        }

        public void TestConnection()
        {
            using OracleConnection conn = GetConnection();
            conn.Open();
            Console.WriteLine("Database connected successfully!");
        }
    }
}