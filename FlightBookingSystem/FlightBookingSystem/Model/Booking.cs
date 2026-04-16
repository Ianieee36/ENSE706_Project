using System;

namespace FBS.Model
{
    class Booking
    {
        // private fields
        private string bookingId;
        private string userId;
        private string flightId;
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
    }
}




