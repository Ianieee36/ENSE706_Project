using System;
using FlightBookingSystem.Model;
using Oracle.ManagedDataAccess.Client;

namespace FlightBookingSystem.Repository
{
    public class FlightRepository : IFlightRepository
    {
        public void FindAllFlights()
        {
            // -- to be implemented
        }
        public Flight FindFlightById(string flightId)
        {
            // -- to be implemented
        }
        public void SaveFlight(Flight flights)
        {
            // -- to be implemented
        }
        public void UpdateFlight(Flight flights)
        {
            // -- to be implemented
        }
        
        public void DeleteFlightById(string flightId)
        {
            // -- to be implemented
        }
    }
}
