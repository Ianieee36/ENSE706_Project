namespace FlightBookingSystem.Repository
{
    public interface IBookingRepository
    {
        Booking? FindBookingsByUserId(string userId);
        Booking? FindBookingsByFlightId(string flightId);
        void SaveBookings(Booking bookings);
        void DeleteBookings(string bookingId);
    }
}