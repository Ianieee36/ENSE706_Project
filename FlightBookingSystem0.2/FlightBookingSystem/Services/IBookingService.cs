using FlightBookingSystem.Model;
using System.Collections.Generic;

namespace FlightBookingSystem.Services
{
    public interface IBookingService
    {
        Booking BookFlight(string userId, string flightId);
        void CancelBookingById(string bookingId);
        List<Booking> GetUserBookingsById(string userId);
        List<Booking> GetBookingHistory(string userId);
    }
}