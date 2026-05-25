using Oracle.ManagedDataAccess.Client;

namespace FlightBookingSystem.Repository
{
    public sealed class DatabaseConnection
    {
        private static DatabaseConnection? instance; // Singleton Pattern
        private readonly string connectionString;

        // Private constructor prevents external instantiation
        private DatabaseConnection()
        {
            connectionString = "User Id=system;Password=1234;Data Source=localhost:1521/XE;";
        }

        // Global access point
        public static DatabaseConnection Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new DatabaseConnection();
                }
                return instance;
            }
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