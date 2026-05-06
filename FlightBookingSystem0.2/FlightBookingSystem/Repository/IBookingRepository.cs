using FlightBookingSystem.Model;

namespace FlightBookingSystem.Repository
{
    public interface IBookingRepository
    {
        Booking? FindBookingsByUserId(string userId);
        Booking? FindBookingsByFlightId(string flightId);
        void SaveBooking(Booking bookings);
        bool DeleteBookingByBookingId(string bookingId);
    }
}