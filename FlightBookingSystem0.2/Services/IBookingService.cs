using FlightBookingSystem.Model;
using System.Collections.Generic;

namespace FlightBookingSystem.Services
{
    public interface IBookingService
    {
        Booking? BookFlight(string userId, string flightId);
        bool CancelBookingById(Booking booking, string userId);
        List<Booking> GetUserBookingsById(string userId);
        List<Booking> GetCurrentBookingsByUserId(string userId);
        List<Booking> GetBookingHistory(string userId);
        Booking? GetBookingById(string bookingId);
        Booking? AssistCustomerBooking(Admin admin, string customerId, string flightId);
        bool AssistCustomerCancelBooking(Admin admin, string bookingId, string userId);
    }
}