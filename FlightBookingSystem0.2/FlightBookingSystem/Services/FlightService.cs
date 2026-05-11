using System;
using System.Collections.Generic;
using FlightBookingSystem.Model;

namespace FlightBookingSystem.Services
{
    internal class FlightService : IFlightService
    {
        public List<Flight> SearchFlights(string origin, string destination)
        {
            throw new NotImplementedException();
        }

        public Flight GetFlightDetails(string flightId)
        {
            throw new NotImplementedException();
        }

        public void AddFlight(Flight flight)
        {
            
        }
        public void UpdateFlight(Flight flight)
        {
            
        }
        public void RemoveFlightById(string flightId)
        {
            
        }
        public List<Flight> GetAllFlights()
        {
            throw new NotImplementedException();
        }
        public int GetFlightOccupancy(string flightId)
        {
            throw new NotImplementedException();
        }
    }
}