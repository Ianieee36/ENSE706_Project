using System;
using System.Collections.Generic;
using FlightBookingSystem.Model;
using Oracle.ManagedDataAccess.Client;

namespace FlightBookingSystem.Repository
{
    internal class FlightRepository : IFlightRepository
    {
        private readonly DatabaseConnection dbConnection;

        public FlightRepository()
        {
            dbConnection = new DatabaseConnection();
        }
        
        // Retrieves list of flights from DB
        public List<Flight> FindAllFlights()
        { 
            List<Flight> flights = new List<Flight>(); // create an emptylist

            using OracleConnection conn = dbConnection.GetConnection(); // database connection
            conn.Open(); // open connection

            string query = "SELECT * FROM FLIGHTS"; // SQL query

            using OracleCommand cmd = new OracleCommand(query, conn); // execute query
            
            using OracleDataReader reader = cmd.ExecuteReader(); // reads data row by row

            while(reader.Read()) // reads all rows
            {
                string flightId = reader["FLIGHTID"].ToString()!;
                string origin = reader["ORIGIN"].ToString()!;
                string destination = reader["DESTINATION"].ToString()!;
                DateTime departure = Convert.ToDateTime(reader["DEPARTURE"]);

                int availableSeats = Convert.ToInt32(reader["AVAILABLESEATS"]);
                int totalSeats = Convert.ToInt32(reader["TOTALSEATS"]);

                Flight flight = new Flight( // return a flight object
                    flightId,
                    origin,
                    destination,
                    departure,
                    availableSeats,
                    totalSeats
                );
                
                flights.Add(flight); // add it to the list
            }

            return flights; // return list of flights
        }
        
        // Finds Flight by flighID
        public Flight? FindFlightById(string flightId)
        {
            using OracleConnection conn = dbConnection.GetConnection();
            conn.Open();


            string query = "SELECT FROM FLIGHTS WHERE FLIGHTID = :flightId";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("flightId", flightId));

            using OracleDataReader reader = cmd.ExecuteReader();

            if(reader.Read())
            {
                string dbflightId = reader["FLIGHTID"].ToString()!;
                string origin = reader["ORIGIN"].ToString()!;
                string destination = reader["DESTINATION"].ToString()!;
                DateTime departure = Convert.ToDateTime(reader["DEPARTURE"]);

                int availableSeats = Convert.ToInt32(reader["AVAILABLESEATS"]);
                int totalSeats = Convert.ToInt32(reader["TOTALSEATS"]);

                return new Flight (
                    dbflightId,
                    origin,
                    destination,
                    departure,
                    availableSeats,
                    totalSeats
                );
            }

            return null;
        }

        // Saves Flight to DB
        public void SaveFlight(Flight flight)
        {
            using OracleConnection conn = dbConnection.GetConnection(); // database connection
            conn.Open();

            string query = @"INSERT INTO FLIGHTS 
                             (FLIGHTID, ORIGIN, DESTINATION, DEPARTURE, AVAILABLESEATS, TOTALSEATS)
                             VALUES
                             (:flightId, origin, destination, departure, availableSeats, totalSeats)"; // SQL Query

            using OracleCommand cmd = new OracleCommand(query, conn);

            // safely passing user input into SQL queries
            cmd.Parameters.Add(new OracleParameter("flightId", flight.FlightId));
            cmd.Parameters.Add(new OracleParameter("origin", flight.Origin));
            cmd.Parameters.Add(new OracleParameter("destination", flight.Destination));
            cmd.Parameters.Add(new OracleParameter("departure", flight.DepartureDateTime));
            cmd.Parameters.Add(new OracleParameter("availableSeats", flight.AvailableSeats));
            cmd.Parameters.Add(new OracleParameter("totalSeats", flight.TotalSeats));

            cmd.ExecuteNonQuery();
        }
        
        // Update Flight Information
        public void UpdateFlight(Flight flight) 
        {
            using OracleConnection conn = dbConnection.GetConnection(); // database connection
            conn.Open();

            // SQL Query 
            string query = @"UPDATE FLIGHTS 
                             SET
                                ORIGIN = :origin,
                                DESTINATION = :destination,
                                DEPARTURE = :departure,
                                AVAILABLESEATS = :availableSeats,
                                TOTALSEATS = :totalSeats";

            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add(new OracleParameter("origin", flight.Origin));
            cmd.Parameters.Add(new OracleParameter("destination", flight.Destination));
            cmd.Parameters.Add(new OracleParameter("departure", flight.DepartureDateTime));
            cmd.Parameters.Add(new OracleParameter("availableSeats", flight.AvailableSeats));
            cmd.Parameters.Add(new OracleParameter("totalSeats", flight.TotalSeats));

            cmd.ExecuteNonQuery();
         }
        
        // Deletes Flight from DB by flightId
        public bool DeleteFlightByFlightId(string flightId)
        {
           using OracleConnection conn = dbConnection.GetConnection();
           conn.Open();

           string query = "DELETE FROM FLIGHTS WHERE FLIGHTID = :flightId";

           using OracleCommand cmd = new OracleCommand(query, conn);

           cmd.Parameters.Add("flightId", OracleDbType.Varchar2).Value = flightId;

           int rowsAffected = cmd.ExecuteNonQuery(); 
           
           return rowsAffected > 0;
        }   
    }
}
