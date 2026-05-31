using FlightBookingSystem.Model;

namespace FlightBookingSystem.Services
{
    public interface IFlightService
    {
        List<Flight> SearchFlights(string origin, string destination);
        Flight? GetFlightDetailsById(string flightId);
        void AddFlight(Admin admin,Flight flight);
        void UpdateFlight(Admin admin, Flight flight);
        void RemoveFlightById(Admin admin, string flightId);
        List<Flight> GetAllFlights();
        int GetFlightOccupancy(string flightId);
        
    }
}