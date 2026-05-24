using FlightBookingSystem.Model;
using System.Collections.Generic;

namespace FlightBookingSystem.Services
{
    public interface IBookingService
    {
        Booking? BookFlight(string userId, string flightId);
        bool CancelBookingById(string bookingId, string userId);
        List<Booking> GetUserBookingsById(string userId);
        List<Booking> GetCurrentBookingsByUserId(string userId);
        List<Booking> GetBookingHistory(string userId);
        string GenerateUniqueBookingId();
    }
}