using System;
using System.Collections.Generic;
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

        public void AddFlight(Flight flight)
        {
            if(flight.DepartureDateTime <= DateTime.Now)
            {
                throw new ArgumentException("Departure date must be in the future");
            }

            if(flight.TotalSeats <= 0)
            {
                throw new ArgumentException("Total seats must be greater than zero");
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

        public void UpdateFlight(Flight flight)
        {
            flightRepository.UpdateFlight(flight);
        }
        
        public void RemoveFlightById(string flightId)
        {
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

        public string GenerateUniqueFlightId()
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