using System;

namespace FlightBookingSystem.Model
{
    public class Booking
    {
        // private fields
        private string bookingId;
        private DateTime bookingDate;
        private BookingStatus status;
        private User customer;
        private Flight flight;
        

        // Booking constructor
        public Booking(string bookingId, DateTime bookingDate, User customer,
                       Flight flight)
        {
            this.bookingId = bookingId;
            this.bookingDate = bookingDate;
            this.status = BookingStatus.PENDING; // Initial state
            this.customer = customer;
            this.flight = flight;
        }

        public Booking(string bookingId, DateTime bookingDate, BookingStatus status, User customer, Flight flight)
        {
            this.bookingId = bookingId;
            this.bookingDate = bookingDate;
            this.status = status;
            this.customer = customer;
            this.flight = flight;
        }

        public string BookingId
        {
            get{return bookingId;}
            private set{bookingId = value;}
        }

        public DateTime BookingDate
        {
            get{return bookingDate;}
            private set{bookingDate = value;}
        }

        public BookingStatus BookingStatus
        {
            get{return status;}
            private set{status = value;} // attribute can only modify by its own class.
        }

        public User Customer
        {
            get{return customer;}
            private set{customer = value;}
        }

        public Flight Flight
        {
            get{return flight;}
            private set{flight = value;}
        }

        public void CancelBooking()
        {
            if (!CanBeCancelled())
            {
                throw new InvalidOperationException("This booking cannot be cancelled");
            }

            BookingStatus = BookingStatus.CANCELLED;
        }

        public bool IsActive()
        {
            return BookingStatus == BookingStatus.CONFIRMED;
        }

        public bool CanBeCancelled()
        {
            return BookingStatus == BookingStatus.PENDING || BookingStatus == BookingStatus.CONFIRMED;
        }
    }
}