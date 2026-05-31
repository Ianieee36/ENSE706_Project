using FlightBookingSystem.Model;

namespace FlightBookingSystem.Repository
{
    public interface ITicketRepository
    {
        Ticket? FindTicketById(string ticketId);
        Ticket? FindTicketByBookingId(string bookingId);
        void SaveTicket(Ticket ticket);
        void UpdateTicket(Ticket ticket);
        bool TicketIdExists(string ticketId);
    }
}