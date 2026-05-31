using System.Collections.Generic;
using FlightBookingSystem.Model;


namespace FlightBookingSystem.Repository
{
    public interface IBookingRepository
    {
        Booking? FindBookingById(string bookingId);
        // Booking? FindBookingByFlightId(string flightId);

        List<Booking> FindBookingsByCustomerId(string customerId);
        List<Booking> FindCurrentBookingsByCustomerId(string customerId);

        List<Booking> FindPastBookingsByCustomerId(string customerId);

        void UpdateBooking(Booking booking);
        void SaveBooking(Booking bookings);

        // bool DeleteBookingByBookingId(string bookingId);
        bool BookingIdExists(string bookingId);
    }
}