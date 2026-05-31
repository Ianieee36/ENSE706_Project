using FlightBookingSystem.Model;

namespace FlightBookingSystem.Services
{
    public interface ITicketService
    {
       
       Ticket IssueTicket(Booking booking);
       void CancelTicket(string ticketId);
       Ticket GetTicketByBookingId(string bookingId);
       bool ValidateTicket(string ticketId);
        
    }
}