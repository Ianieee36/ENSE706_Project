namespace FlightBookingSystem.Repository
{
    public interface IFlightRepository
    {
        void FindAllFlights();
        Flight FindFlightById(string flightId);
        void SaveFlight(Flight flights);
        void UpdateFlight(Flight flights);
        void DeleteFlightById(string flightId);
    }
}
