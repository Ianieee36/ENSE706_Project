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

        public DateTime BookingDate
        {
            get{return bookingDate;}
            private set{bookingDate = value;}
        }

        public BookingStatus Status
        {
            get{return status;}
            private set{status = value;} // attribute can only modify by its own class.
        }

        public void Confirm()
        {
            if (Status != BookingStatus.PENDING)
            {
                throw new InvalidOperationException("Only pending bookings can be confirmed");
            }

            Status = BookingStatus.CONFIRMED;
        }

        public void Cancel()
        {
            if (!CanBeCancelled())
            {
                throw new InvalidOperationException("This booking cannot be cancelled");
            }

            Status = BookingStatus.CANCELLED;
        }

        public bool IsActive()
        {
            return Status == BookingStatus.CONFIRMED;
        }

        public bool CanBeCancelled()
        {
            return Status == BookingStatus.PENDING || Status == BookingStatus.CONFIRMED;
        }
    }
}