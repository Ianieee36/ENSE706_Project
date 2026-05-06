using System;
using FlightBookingSystem.Model;
using Oracle.ManagedDataAccess.Client;

namespace FlightBookingSystem.Repository
{
    public class FlightRepository : IFlightRepository
    {
        DatabaseConnection dbConnection = new DatabaseConnection();
        public void FindAllFlights()
        { 
            using OracleConnection conn = dbConnection.GetConnection();
            conn.Open();

            string query = "SELECT * FROM FLIGHTS";

            using OracleCommand cmd = new OracleCommand(query, conn);
            
            using OracleDataReader reader = cmd.ExecuteReader();


        }
        
        public Flight FindFlightById(string flightId)
        {
            throw new NotImplementedException();
        }
        public void SaveFlight(Flight flights)
        {
            throw new NotImplementedException();
        }
        public void UpdateFlight(Flight flights)
        {
            throw new NotImplementedException();
        }
        
        public void DeleteFlightById(string flightId)
        {
            throw new NotImplementedException();                                                                                
        }
    }
}
