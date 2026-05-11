using System;
using FlightBookingSystem.Model;
using Oracle.ManagedDataAccess.Client;

namespace FlightBookingSystem.Repository
{
    internal class BookingRepository : IBookingRepository
    {
        private readonly IUserRepository userRepository;
        private readonly IFlightRepository flightRepository;
        private readonly DatabaseConnection dbConnection;

        public BookingRepository()
        {
            userRepository = new UserRepository();
            flightRepository = new FlightRepository();
            dbConnection = new DatabaseConnection();
        }

        public Booking? FindBookingsByUserId(string userId)
        {
            using OracleConnection conn = dbConnection.GetConnection();
            conn.Open();

            string query = "SELECT * FROM BOOKINGS WHERE USERID = :userId";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("userId", userId));

            using OracleDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string bookingId = reader["BOOKINGID"].ToString()!;
                DateTime bookingDate = Convert.ToDateTime(reader["BOOKINGDATE"]);
                BookingStatus status = Enum.Parse<BookingStatus>(reader["BOOKINGSTATUS"].ToString()!);

                string dbUserId = reader["USERID"].ToString()!;
                string dbFlightId = reader["FLIGHTID"].ToString()!;

                User customer = userRepository.FindUserById(dbUserId)!;
                Flight flight = flightRepository.FindFlightById(dbFlightId);

                return new Booking (
                    bookingId,
                    bookingDate,
                    status,
                    customer,
                    flight
                );
            }
            return null;
        }

        public Booking? FindBookingsByFlightId(string flightId)
        {
            using OracleConnection conn = dbConnection.GetConnection();
            conn.Open();

            string query = "SELECT * FROM BOOKINGS WHERE FLIGHTID = :flightId";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("flightId", flightId));

            using OracleDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {   
                string bookingId = reader["BOOKINGID"].ToString()!;
                DateTime bookingDate = Convert.ToDateTime(reader["BOOKINGDATE"]);
                BookingStatus status = Enum.Parse<BookingStatus>(reader["BOOKINGSTATUS"].ToString()!);

                string dbUserId = reader["USERID"].ToString()!;
                string dbFlightId = reader["FLIGHTID"].ToString()!;

                User customer = userRepository.FindUserById(dbUserId)!;
                Flight flight = flightRepository.FindFlightById(dbFlightId);

                return new Booking (
                    bookingId,
                    bookingDate,
                    status,
                    customer,
                    flight 
                );
                
            }
            return null;
        }

        public void SaveBooking(Booking bookings)
        {
            using OracleConnection conn = dbConnection.GetConnection();
            conn.Open();

            string query = @"INSERT INTO BOOKINGS 
                             (BOOKINGID, BOOKINGDATE, STATUS, USERID, FLIGHTID)
                             VALUES 
                             (:bookingId, :bookingDate, :status, :userId, :flightId)";

            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add(new OracleParameter("bookingId", bookings.BookingId));
            cmd.Parameters.Add(new OracleParameter("bookingDate", bookings.BookingDate));
            cmd.Parameters.Add(new OracleParameter("status", bookings.Status.ToString()));
            cmd.Parameters.Add(new OracleParameter("userId", bookings.Customer.UserId));
            cmd.Parameters.Add(new OracleParameter("flightId", bookings.Flight.FlightId));

            cmd.ExecuteNonQuery();
        }

        public bool DeleteBookingByBookingId(string bookingId)
        {
            using OracleConnection conn = dbConnection.GetConnection();
            conn.Open();

            string query = "DELETE FROM BOOKINGS WHERE BOOKINGID = :bookingId";

            using OracleCommand cmd = new OracleCommand(query,conn);

            cmd.Parameters.Add("bookingId", OracleDbType.Varchar2).Value = bookingId;

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;

        }

    }
}