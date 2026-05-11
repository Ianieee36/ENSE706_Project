using System.Collections.Generic;
using FlightBookingSystem.Model;

namespace FlightBookingSystem.Services
{
    public interface IFlightService
    {
        List<Flight> SearchFlights(string origin, string destination);
        Flight GetFlightDetails(string flightId);
        void AddFlight(Flight flight);
        void UpdateFlight(Flight flight);
        void RemoveFlightById(string flightId);
        List<Flight> GetAllFlights();
        int GetFlightOccupancy(string flightId);
    }
}