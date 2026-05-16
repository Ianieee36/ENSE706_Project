using System.Collections.Generic;
using FlightBookingSystem.Model;


namespace FlightBookingSystem.Repository
{
    public interface IBookingRepository
    {
        Booking? FindBookingById(string bookingId);
        Booking? FindBookingByFlightId(string flightId);

        List<Booking> FindBookingsByUserId(string userId);

        List<Booking> FindPastBookingsByUserId(string userId);

        void UpdateBooking(Booking booking);
        void SaveBooking(Booking bookings);

        bool DeleteBookingByBookingId(string bookingId);
    }
}