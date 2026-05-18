using System.Collections.Generic;
using FlightBookingSystem.Model;

namespace FlightBookingSystem.Repository
{
    public interface IFlightRepository
    {
        List<Flight> FindAllFlights();
        List<Flight> FindFlightsByRoute(string origin, string destination);
        Flight? FindFlightById(string flightId);
        void SaveFlight(Flight flight);
        void UpdateFlight(Flight flight);
        bool DeleteFlightByFlightId(string flightId);
    }
}
