using System;
using System.Collections.Generic;
using FlightBookingSystem.Model;

namespace FlightBookingSystem.Services
{
    internal class BookingService : IBookingService
    {
        public Booking BookFlight(string userId, string flightId)
        {
            throw new NotImplementedException();
        }
        public void CancelBookingById(string bookingId)
        {
            
        }
        public List<Booking> GetUserBookingsById(string userId)
        {
            throw new NotImplementedException();
        }
        public List<Booking> GetBookingHistory(string userId)
        {
            throw new NotImplementedException();
        }
    }
}