using System;

namespace FlightBookingSystem.Model
{
    public class Booking
    {
        // private fields
        private string bookingId;
        private User user;
        private Flight flight;
        private DateTime bookingDate;
        private BookingStatus status;

        // Booking constructor
        public Booking(string bookingId, string userId, string flightId, 
                       DateTime bookingDate)
        {
            this.bookingId = bookingId;
            this.userId = userId;
            this.flightId = flightId;
            this.bookingDate = bookingDate;
            this.status = BookingStatus.PENDING; // Initial state
        }

        public string BookingId
        {
            get{return bookingId;}
        }

        public string UserId
        {
            get{return userId;}
        }

        public string FlightId
        {
            get{return flightId;}
        }

        public DateTime BookingDate
        {
            get{return bookingDate;}
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




