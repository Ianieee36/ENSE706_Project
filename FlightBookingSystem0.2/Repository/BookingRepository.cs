using FlightBookingSystem.Model;
using Oracle.ManagedDataAccess.Client;

namespace FlightBookingSystem.Repository
{
    internal class BookingRepository : IBookingRepository
    {
        private readonly IUserRepository userRepository;
        private readonly IFlightRepository flightRepository;
    
        public BookingRepository(IUserRepository userRepository, IFlightRepository flightRepository)
        {
            this.userRepository = userRepository;
            this.flightRepository = flightRepository;
        }

        public Booking? FindBookingById(string bookingId)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = "SELECT * FROM BOOKINGS WHERE BOOKINGID = :bookingId";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("bookingId", bookingId));

            using OracleDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string dbBookingId = reader["BOOKINGID"].ToString()!;
                DateTime bookingDate = Convert.ToDateTime(reader["BOOKINGDATE"]);
                BookingStatus status = Enum.Parse<BookingStatus>(reader["STATUS"].ToString()!);

                string dbUserId = reader["USERID"].ToString()!;
                string dbFlightId = reader["FLIGHTID"].ToString()!;

                User customer = userRepository.FindUserById(dbUserId)!;
                Flight? flight = flightRepository.FindFlightById(dbFlightId);

                return new Booking (
                    bookingId,
                    bookingDate,
                    status,
                    customer,
                    flight!
                );
            }
            return null;
        }

        // public Booking? FindBookingByFlightId(string flightId)
        // {
        //     using OracleConnection conn = dbConnection.GetConnection();
        //     conn.Open();

        //     string query = "SELECT * FROM BOOKINGS WHERE FLIGHTID = :flightId";

        //     using OracleCommand cmd = new OracleCommand(query, conn);
        //     cmd.Parameters.Add(new OracleParameter("flightId", flightId));

        //     using OracleDataReader reader = cmd.ExecuteReader();

        //     if (reader.Read())
        //     {   
        //         string bookingId = reader["BOOKINGID"].ToString()!;
        //         DateTime bookingDate = Convert.ToDateTime(reader["BOOKINGDATE"]);
        //         BookingStatus status = Enum.Parse<BookingStatus>(reader["STATUS"].ToString()!);

        //         string dbUserId = reader["USERID"].ToString()!;
        //         string dbFlightId = reader["FLIGHTID"].ToString()!;

        //         User customer = userRepository.FindUserById(dbUserId)!;
        //         Flight? flight = flightRepository.FindFlightById(dbFlightId);

        //         return new Booking (
        //             bookingId,
        //             bookingDate,
        //             status,
        //             customer,
        //             flight!
        //         );
                
        //     }
        //     return null;
        // }

        public List<Booking> FindBookingsByCustomerId(string customerId)
        {
            List<Booking> bookings = new List<Booking>();

            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = "SELECT * FROM BOOKINGS WHERE USERID = :userId";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("userId", customerId));

            using OracleDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {   
                string bookingId = reader["BOOKINGID"].ToString()!;
                DateTime bookingDate = Convert.ToDateTime(reader["BOOKINGDATE"]);
                BookingStatus status = Enum.Parse<BookingStatus>(reader["STATUS"].ToString()!);

                string dbUserId = reader["USERID"].ToString()!;
                string dbFlightId = reader["FLIGHTID"].ToString()!;

                User? customer = userRepository.FindUserById(dbUserId)!;
                Flight? flight = flightRepository.FindFlightById(dbFlightId);

                if(customer != null && flight != null)
                {
                    Booking booking = new Booking(
                    bookingId,
                    bookingDate,
                    status,
                    customer,
                    flight 
                );
                
                bookings.Add(booking);
                }   
            }
            return bookings;
        }

        public List<Booking> FindCurrentBookingsByCustomerId(string customerId)
        {
            List<Booking> bookings = new List<Booking>();

            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"
                SELECT b.*
                FROM BOOKINGS b
                JOIN FLIGHTS f ON b.FLIGHTID = f.FLIGHTID
                WHERE b.USERID = :userId
                AND f.DEPARTURE >= SYSTIMESTAMP
                AND b.STATUS != 'CANCELLED'";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("userId", customerId));

            using OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string bookingId = reader["BOOKINGID"].ToString()!;
                DateTime bookingDate = Convert.ToDateTime(reader["BOOKINGDATE"]);
                BookingStatus status = Enum.Parse<BookingStatus>(reader["STATUS"].ToString()!);

                string dbUserId = reader["USERID"].ToString()!;
                string dbFlightId = reader["FLIGHTID"].ToString()!;

                User? customer = userRepository.FindUserById(dbUserId);
                Flight? flight = flightRepository.FindFlightById(dbFlightId);

                if (customer != null && flight != null)
                {
                    bookings.Add(new Booking(
                        bookingId,
                        bookingDate,
                        status,
                        customer,
                        flight
                    ));
                }
            }

            return bookings;
        }

        public List<Booking> FindPastBookingsByCustomerId(string customerId)
        {
            List<Booking> bookings = new List<Booking>();

            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"
                    SELECT b.*
                    FROM BOOKINGS b
                    JOIN FLIGHTS f ON b.FLIGHTID = f.FLIGHTID
                    WHERE b.USERID = :userId
                    AND f.DEPARTURE < SYSTIMESTAMP";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("userId", customerId));

            using OracleDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {   
                string bookingId = reader["BOOKINGID"].ToString()!;
                DateTime bookingDate = Convert.ToDateTime(reader["BOOKINGDATE"]);
                BookingStatus bookingStatus = Enum.Parse<BookingStatus>(reader["STATUS"].ToString()!);

                string dbUserId = reader["USERID"].ToString()!;
                string dbFlightId = reader["FLIGHTID"].ToString()!;

                User? customer = userRepository.FindUserById(dbUserId)!;
                Flight? flight = flightRepository.FindFlightById(dbFlightId);

                if(customer != null && flight != null)
                {
                    Booking booking = new Booking(
                    bookingId,
                    bookingDate,
                    bookingStatus,
                    customer,
                    flight 
                );
                
                bookings.Add(booking);
                }   
            }
            return bookings;  
        }
        

        public void UpdateBooking(Booking booking)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = "UPDATE BOOKINGS SET STATUS = :bookingStatus WHERE BOOKINGID = :bookingId";

            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add(new OracleParameter("bookingStatus", booking.BookingStatus.ToString()));
            cmd.Parameters.Add(new OracleParameter("bookingId", booking.BookingId));
            
            cmd.ExecuteNonQuery();
        }

        public void SaveBooking(Booking bookings)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = @"INSERT INTO BOOKINGS 
                             (BOOKINGID, BOOKINGDATE, STATUS, USERID, FLIGHTID)
                             VALUES 
                             (:bookingId, :bookingDate, :bookingStatus, :userId, :flightId)";

            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add(new OracleParameter("bookingId", bookings.BookingId));
            cmd.Parameters.Add(new OracleParameter("bookingDate", bookings.BookingDate));
            cmd.Parameters.Add(new OracleParameter("bookingStatus", bookings.BookingStatus.ToString()));
            cmd.Parameters.Add(new OracleParameter("userId", bookings.Customer.UserId));
            cmd.Parameters.Add(new OracleParameter("flightId", bookings.Flight.FlightId));

            cmd.ExecuteNonQuery();
        }

        // public bool DeleteBookingByBookingId(string bookingId)
        // {
        //     using OracleConnection conn = dbConnection.GetConnection();
        //     conn.Open();

        //     string query = "DELETE FROM BOOKINGS WHERE BOOKINGID = :bookingId";

        //     using OracleCommand cmd = new OracleCommand(query,conn);

        //     cmd.Parameters.Add("bookingId", OracleDbType.Varchar2).Value = bookingId;

        //     int rowsAffected = cmd.ExecuteNonQuery();

        //     return rowsAffected > 0;

        // }

        public bool BookingIdExists(string bookingId)
        {
            using OracleConnection conn = DatabaseConnection.Instance.GetConnection();
            conn.Open();

            string query = "SELECT COUNT(*) FROM BOOKINGS WHERE BOOKINGID = :bookingId";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("bookingId", bookingId));

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

    }
}