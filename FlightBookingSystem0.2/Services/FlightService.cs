using FlightBookingSystem.Model;
using FlightBookingSystem.Repository;
using FlightBookingSystem.Utilities;

namespace FlightBookingSystem.Services
{
    internal class FlightService : IFlightService
    {
        private readonly IFlightRepository flightRepository;

        public FlightService(IFlightRepository flightRepository)
        {
            this.flightRepository = flightRepository;
        }

        public List<Flight> SearchFlights(string origin, string destination)
        {
            return flightRepository.FindFlightsByRoute(origin, destination);
        }

        public Flight? GetFlightDetailsById(string flightId)
        {
            Flight? flight = flightRepository.FindFlightById(flightId);

            if(flight == null)
            {
                return null;
            }

            return flight;
        }

        public void AddFlight(Admin admin, Flight flight)
        {
            if (admin == null)
            {
                throw new ArgumentException("Admin cannot be null.");
            }

            if (!admin.CanAddFlight())
            {
                throw new Exception("You do not have permission to add a flight.");
            }

            if (flight == null)
            {
                throw new ArgumentException("Flight cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(flight.Origin))
            {
                throw new ArgumentException("Origin cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(flight.Destination))
            {
                throw new ArgumentException("Destination cannot be empty.");
            }

            if (flight.Origin.Equals(flight.Destination, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Origin and destination cannot be the same.");
            }

            if (flight.DepartureDateTime <= DateTime.Now)
            {
                throw new ArgumentException("Departure date must be in the future.");
            }

            if (flight.TotalSeats <= 0)
            {
                throw new ArgumentException("Total seats must be greater than zero.");
            }

            string flightId = GenerateUniqueFlightId();

            Flight newFlight = new Flight(
                flightId,
                flight.Origin,
                flight.Destination,
                flight.DepartureDateTime,
                flight.TotalSeats
            );

            flightRepository.SaveFlight(newFlight);
        }

        public void UpdateFlight(Admin admin, Flight flight)
        {
            if(admin == null)
            {
                throw new Exception("Admin cannot be null");
            }

            if(!admin.CanUpdateFlight())
            {
                throw new Exception("You do not have permission to update a flight.");
            }

            if (flight == null)
            {
                throw new ArgumentException("Flight cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(flight.Origin))
            {
                throw new ArgumentException("Origin cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(flight.Destination))
            {
                throw new ArgumentException("Destination cannot be empty.");
            }

            if (flight.Origin.Equals(flight.Destination, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Origin and destination cannot be the same.");
            }

            if (flight.DepartureDateTime <= DateTime.Now)
            {
                throw new ArgumentException("Departure date must be in the future.");
            }

            if (flight.TotalSeats <= 0)
            {
                throw new ArgumentException("Total seats must be greater than zero.");
            }


            flightRepository.UpdateFlight(flight);
        }
        
        public void RemoveFlightById(Admin admin, string flightId)
        {
            if(admin == null)
            {
                throw new Exception("Admin cannot be null");
            }

            if(!admin.CanDeleteFlight())
            {
                throw new Exception("You do not have permission to remove a flight.");
            }

            Flight? flight = flightRepository.FindFlightById(flightId);

            if(flight == null)
            {
                throw new ArgumentException("Flight not found");
            }

            flightRepository.DeleteFlightById(flightId);
        }
        
        public List<Flight> GetAllFlights()
        {   
            return flightRepository.FindAllFlights();
        }

        public int GetFlightOccupancy(string flightId)
        {
            Flight? flight = flightRepository.FindFlightById(flightId);

            if(flight == null)
            {
                throw new ArgumentException("Flight not found");
            }

            return flight.TotalSeats - flight.AvailableSeats;
        }

        private string GenerateUniqueFlightId()
        {
            string flightId;

            do
            {
                flightId = IdGenerator.GenerateFlightId();
            }
            while (flightRepository.FlightIdExists(flightId));

            return flightId;
        }
        
    }
}