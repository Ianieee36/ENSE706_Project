namespace FlightBookingSystem.Model
{
    public class Ticket
    {
        private string ticketId;
        private string ticketNumber;
        private DateTime issueDate;
        private string seatNumber;
        private string gateNumber;
        private DateTime boardingTime;
        private TicketStatus ticketStatus;
        private Booking booking;

        public Ticket(string ticketId, string ticketNumber, DateTime issueDate, string seatNumber, 
                      string gateNumber, DateTime boardingTime, TicketStatus ticketStatus, Booking booking )
        {
            this.ticketId = ticketId;
            this.ticketNumber = ticketNumber;
            this.issueDate = issueDate;
            this.seatNumber = seatNumber;
            this.gateNumber = gateNumber;
            this.boardingTime = boardingTime;
            this.ticketStatus = TicketStatus.ISSUED;
            this.booking = booking; 
        }

        public string TicketId
        {
            get { return ticketId; }
            private set { ticketId = value; }
        }

        public string TicketNumber
        {
            get {return ticketNumber; }
            private set { ticketNumber = value; }
        }

        public DateTime IssueDate
        {
            get { return issueDate; }
            private set { issueDate = value;}
        }

        public string SeatNumber
        {
            get { return seatNumber; }
            private set { seatNumber = value; }
        }

        public string GateNumber
        {
            get { return gateNumber; }
            private set { gateNumber = value; }
        }

        public DateTime BoardingTime
        {
            get { return boardingTime; }
            private set { boardingTime = value; }
        }

        public TicketStatus TicketStatus
        {
            get { return ticketStatus; }
            private set { ticketStatus = value; }
        }

        public Booking Booking
        {
            get { return booking; }
            private set { booking = value; }
        }

        public bool IsValid()
        {
            return TicketStatus == TicketStatus.ISSUED;
        }

        public void CancelTicket()
        {
            if (TicketStatus == TicketStatus.CANCELLED)
                throw new Exception("Ticket is already cancelled.");

            TicketStatus = TicketStatus.CANCELLED;
        }
    }
}