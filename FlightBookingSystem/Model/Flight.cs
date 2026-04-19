using System;

namespace FlightBookingSystem.Model
{
    public class Flight
    {
        // private fields
        private string flightId;
        private string origin;
        private string destination;
        private DateTime departureDateTime;
        private int availableSeats;
        private int totalSeats;

        // Flight constructor
        public Flight(string flightId, string origin, string destination, DateTime departureDateTime, 
                      int totalSeats)
        {
            this.flightId = flightId;
            this.origin = origin;
            this.destination = destination;
            this.departureDateTime = departureDateTime;
            this.availableSeats = totalSeats; // Initial state
            this.totalSeats = totalSeats; 
        }

        // public properties (read-only)
        public string FlightId
        {
            get{return flightId;}
            private set{flightId = value;}
        }

        public string Origin
        {
            get{return origin;}
            private set{origin = value;}
        }

        public string Destination
        {
            get{return destination;}
            private set{destination = value;}
        }

        public DateTime DepartureDateTime
        {
            get{return departureDateTime;}
            private set{departureDateTime = value;}
        }

        public int AvailableSeats
        {
            get{return availableSeats;}
            private set{availableSeats = value;}
        }

        public int TotalSeats
        {
            get{return totalSeats;}
            private set{totalSeats = value;}
        }

        public bool HasAvailableSeats()
        {
            return availableSeats > 0;
        }

        public void UpdateSeatCount(int seatsToReduce)
        {
            if (seatsToReduce <= 0)
            {
                throw new ArgumentException("Seats to reduce must be greater than zero.");
            }

            if(availableSeats < seatsToReduce)
            {
                throw new InvalidOperationException("Not enough seats available");
            }

            availableSeats -= seatsToReduce;
        }
    }
}

