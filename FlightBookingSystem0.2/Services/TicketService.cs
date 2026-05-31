using FlightBookingSystem.Model;
using FlightBookingSystem.Utilities;
using FlightBookingSystem.Repository;

namespace FlightBookingSystem.Services
{

    public class TicketService : ITicketService
    {
        private readonly ITicketRepository ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }
        
        
        public Ticket IssueTicket(Booking booking)
        {
            if (!booking.IsActive())
            {
                throw new Exception("Cannot issue ticket for inactive booking.");
            }
            
            return new Ticket(
                GenerateUniqueTicketId(),
                GenerateTicketNumber(),
                DateTime.Now,
                IdGenerator.GenerateSeatNumber(),
                IdGenerator.GenerateGateNumber(),
                DateTime.Now.AddMinutes(30),
                TicketStatus.ISSUED,
                booking
            );
        }

        public void CancelTicket(string ticketId)
        {
            Ticket? ticket = ticketRepository.FindTicketById(ticketId);

            if(ticket == null)
            {
                throw new Exception("Ticket not found.");
            }

            ticket.CancelTicket();

            ticketRepository.UpdateTicket(ticket);
        }

        public Ticket GetTicketByBookingId(string bookingId)
        {
            Ticket? ticket = ticketRepository.FindTicketByBookingId(bookingId);
            
            if(ticket == null)
            {
                throw new Exception("Ticket not found for this booking.");
            }

            return ticket;
        }
       public bool ValidateTicket(string ticketId)
        {
            Ticket? ticket = ticketRepository.FindTicketById(ticketId);

            if(ticket == null)
            {
                return false;
            }

            return ticket.IsValid();
        }

        private string GenerateUniqueTicketId()
        {
            string ticketId;

            do
            {
                ticketId = IdGenerator.GenerateTicketId();
            }
            while (ticketRepository.TicketIdExists(ticketId));

            return ticketId;
        }

        private string GenerateTicketNumber()
        {
            string ticketNumber;

            do
            {
                ticketNumber = IdGenerator.GenerateTicketNumber();
            }
            while (ticketRepository.TicketIdExists(ticketNumber));

            return ticketNumber;
        }
    }
}