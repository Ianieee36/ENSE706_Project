using FlightBookingSystem.Model;
using Oracle.ManagedDataAccess.Client;

namespace FlightBookingSystem.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IBookingRepository bookingRepository;

        public TicketRepository(IBookingRepository bookingRepository)
        {
            this.bookingRepository = bookingRepository;
        }
        public Ticket? FindTicketById(string ticketId)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"
                SELECT *
                FROM TICKETS
                WHERE TICKETID = :ticketId";
            
            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add(new OracleParameter("ticketId", ticketId));

            using OracleDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string dbTicketId = reader["TICKETID"].ToString()!;
                string ticketNumber = reader["TICKETNUMBER"].ToString()!;
                DateTime issueDate = Convert.ToDateTime(reader["ISSUEDATE"]);
                string seatNumber = reader["SEATNUMBER"].ToString()!;
                string gateNumber = reader["GATENUMBER"].ToString()!;
                DateTime boardingTime = Convert.ToDateTime(reader["BOARDINGTIME"]);
                TicketStatus ticketStatus = Enum.Parse<TicketStatus>(reader["TICKETSTATUS"].ToString()!);

                string bookingId = reader["BOOKINGID"].ToString()!;

                Booking booking = bookingRepository.FindBookingById(bookingId)!;

                if(booking == null)
                {
                    throw new Exception("Booking not found.");
                } 

                return new Ticket(
                    dbTicketId,
                    ticketNumber,
                    issueDate,
                    seatNumber,
                    gateNumber,
                    boardingTime,
                    ticketStatus,
                    booking
                );              
            }

            return null;
        }
        public Ticket? FindTicketByBookingId(string bookingId)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"
                SELECT *
                FROM TICKETS
                WHERE BOOKINGID = :bookingId";
            
            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("bookingId", bookingId));

            using OracleDataReader reader = cmd.ExecuteReader();

            if(reader.Read())
            {
                string dbTicketId = reader["TICKETID"].ToString()!;
                string ticketNumber = reader["TICKETNUMBER"].ToString()!;
                DateTime issueDate = Convert.ToDateTime(reader["ISSUEDATE"]);
                string seatNumber = reader["SEATNUMBER"].ToString()!;
                string gateNumber = reader["GATENUMBER"].ToString()!;
                DateTime boardingTime = Convert.ToDateTime(reader["BOARDINGTIME"]);
                TicketStatus ticketStatus = Enum.Parse<TicketStatus>(reader["TICKETSTATUS"].ToString()!);

                Booking booking = bookingRepository.FindBookingById(bookingId)!;

                if(booking == null)
                {
                    throw new Exception("Booking not found.");
                }

                return new Ticket(
                    dbTicketId,
                    ticketNumber,
                    issueDate,
                    seatNumber,
                    gateNumber,
                    boardingTime,
                    ticketStatus,
                    booking
                );
            }

            return null;
        }
        public void SaveTicket(Ticket ticket)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"
                INSERT INTO TICKETS
                (
                    TICKETID,
                    TICKETNUMBER,
                    ISSUEDATE,
                    SEATNUMBER,
                    GATENUMBER,
                    BOARDINGTIME,
                    TICKETSTATUS,
                    BOOKINGID
                )
                VALUES
                (
                    :ticketId,
                    :ticketNumber,
                    :issueDate,
                    :seatNumber,
                    :gateNumber,
                    :boardingTime,
                    :ticketStatus,
                    :bookingId
                )";

            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add(new OracleParameter("ticketId", ticket.TicketId));
            cmd.Parameters.Add(new OracleParameter("ticketNumber", ticket.TicketNumber));
            cmd.Parameters.Add(new OracleParameter("issueDate", ticket.IssueDate));
            cmd.Parameters.Add(new OracleParameter("seatNumber", ticket.SeatNumber));
            cmd.Parameters.Add(new OracleParameter("gateNumber", ticket.GateNumber));
            cmd.Parameters.Add(new OracleParameter("boardingTime", ticket.BoardingTime));
            cmd.Parameters.Add(new OracleParameter("ticketStatus", ticket.TicketStatus.ToString()));
            cmd.Parameters.Add(new OracleParameter("bookingId", ticket.Booking.BookingId));

            cmd.ExecuteNonQuery();
        }
        public void UpdateTicket(Ticket ticket)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"
                UPDATE TICKETS
                SET TICKETSTATUS = :ticketStatus
                WHERE TICKETID = :ticketId";

            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add("ticketStatus", OracleDbType.Varchar2).Value =
                ticket.TicketStatus.ToString();

            cmd.Parameters.Add("ticketId", OracleDbType.Varchar2).Value =
                ticket.TicketId;

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected == 0)
                throw new Exception("Ticket update failed. Ticket not found.");
        }

        public bool TicketIdExists(string ticketId)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"
                SELECT COUNT(*)
                FROM TICKETS
                WHERE TICKETID = :ticketId";

            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add(new OracleParameter("ticketId", ticketId));

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }
    }
}