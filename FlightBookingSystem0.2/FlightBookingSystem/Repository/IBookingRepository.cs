using System;
using FlightBookingSystem.Model;


namespace FlightBookingSystem.Repository
{
    public interface IBookingRepository
    {
        Booking? FindBookingById(string bookingId);
        Booking? FindBookingByFlightId(string flightId);

        void UpdateBooking(Booking booking);
        void SaveBooking(Booking bookings);

        bool DeleteBookingByBookingId(string bookingId);
    }
}