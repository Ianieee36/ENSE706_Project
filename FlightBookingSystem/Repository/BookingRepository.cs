using System;
using FlightBookingSystem.Model;
using Oracle.ManagedDataAccess.Client;

namespace FlightBookingSystem.Repository
{
    public class BookingRepository : IBookingRepository
    {
        public Booking? FindBookingsByUserId(string userId)
        {
            using OracleConnection conn = dbConnection.GetConnection();
            conn.Open();

            string query = "SELECT * FROM BOOKINGS WHERE USERID :userId";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("userId", userId));

            using OracleDataReader reader = cmd.ExecuteReader();

            if (reader.read())
            {
                BookingStatus status = Enum.Parse<BookingStatus>(reader["BOOKINGSTATUS"].ToString()!);

                return new Booking (
                    reader["BOOKINGID"].ToString()!,
                    Convert.ToDateTime(reader["BOOKINGDATE"]),
                    reader["USERID"].ToString()!,
                    reader["FLIGHTID"].ToString()!
                    
                );
            }
            return null;
        }

        public Booking? FindBookingsByFlightId(string flightId)
        {
            using OracleConnection conn = dbConnection.GetConnection();
            conn.Open();

            string query = "SELECT * FROM BOOKINGS WHERE FLIGHTID :flightId";

            using OracleCommand cmd = new OracleCommand(query, conn);
            cmd.Parameters.Add(new OracleParameter("flightId", flightId));

            using OracleDataReader reader = cmd.ExecuteReader();

            if (reader.read())
            {
                BookingStatus status = Enum.Parse<BookingStatus>(reader["BOOKINGSTATUS"].ToString()!);

                return new Booking (
                    reader["BOOKINGID"].ToString()!,
                    Convert.ToDateTime(reader["BOOKINGDATE"]),
                    reader["USERID"].ToString()!,
                    reader["FLIGHTID"].ToString()!
                    
                );
                
            }
            return null;
        }

        public void SaveBookings(Booking bookings)
        {
            using OracleConnection conn = dbConnection.GetConnection();
            conn.Open();

            string query = @"INSERT INTO BOOKINGS (BOOKINGID, BOOKINGDATE, STATUS, USERID, FLIGHTID)
                             VALUES (:bookingId :bookingDate :status :userId :flightId)";

            using OracleCommand cmd = new OracleCommand(query, conn);

            cmd.Parameters.Add(new OracleParameter("bookingId", bookings.BookingId));
            cmd.Parameters.Add(new OracleParameter("bookingDate", bookings.BookingDate));
            cmd.Parameters.Add(new OracleParameter("status", bookings.BookingStatus.ToString()));
            cmd.Parameters.Add(new OracleParameter("bookingId", bookings.BookingId));
            cmd.Parameters.Add(new OracleParameter("userId", bookings.User.UserId));
            cmd.Parameters.Add(new OracleParameter("flightId", bookings.Flight.FlightId));

            cmd.ExecuteNonQuery();
        }

        public void DeleteBooking(string userId)
        {
            // -- to be implemented
        }


    }
}