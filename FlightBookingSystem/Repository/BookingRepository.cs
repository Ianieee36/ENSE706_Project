using System;
using FlightBookingSystem.Model;
using Oracle.ManagedDataAccess.Client;

namespace FlightBookingSystem.Repository
{
    public class BookingRepository : IBookingRepository
    {
        public Booking? FindBookingsByUserId(string userId)
        {
            // -- to be implemented
        }

        public Booking? FindBookingsByFlightId(string flightId)
        {
            // -- to be implemented
        }

        public void SaveBookings(Booking bookings)
        {
            // -- to be implemented
        }

        public void DeleteBooking(string userId)
        {
            // -- to be implemented
        }


    }
}