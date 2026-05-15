using System;
using System.Collections.Generic;
using FlightBookingSystem.Model;
using FlightBookingSystem.Repository;

namespace FlightBookingSystem.Services
{
    internal class BookingService : IBookingService
    {  
        private readonly IBookingRepository bookingRepository;
        private readonly IUserRepository userRepository;
        private readonly IFlightRepository flightRepository;
        public BookingService()
        {
            bookingRepository = new BookingRepository();
            userRepository = new UserRepository();
            flightRepository = new FlightRepository();
        }


        public Booking? BookFlight(string userId, string flightId) // Books Flight
        {

            // finds user by userId
            User? user = userRepository.FindUserById(userId);
            
            // checks if user exists
            if(user == null) 
            {
                Console.WriteLine("User not found!");
                return null;
            }

            // checks if user Role if Customer or not
            if(user.UserRole != Role.CUSTOMER)
            {
                Console.WriteLine("Only customers can book flights.");
                return null;
            }

            // finds flight by flightId
            Flight? flight = flightRepository.FindFlightById(flightId);

            // checks if the flight exists or not
            if(flight == null)
            {
                Console.WriteLine("Flight not found.");
                return null;
            }
            
            // checks if the flight still has available seats
            if (!flight.HasAvailableSeats())
            {
                Console.WriteLine("No seats available.");
                return null;
            }

            // creates new booking
            Booking newBooking = new Booking(
                Guid.NewGuid().ToString(),
                DateTime.Now,
                BookingStatus.CONFIRMED,
                user,
                flight
            );

            // updates flight seat count
            flight.UpdateSeatCount(-1);

            // saves booking to DB
            bookingRepository.SaveBooking(newBooking);

            // updates flight 
            flightRepository.UpdateFlight(flight);

            // returns newBooking
            return newBooking;
        }
        public bool CancelBookingById(string bookingId, string userId)
        {
            Booking? booking = bookingRepository.FindBookingById(bookingId);

            if(booking == null)
            {
                Console.WriteLine("Booking not found.");
                return false;
            }

            if(booking.Customer.UserId != userId)
            {
                Console.WriteLine("This booking does not belong to the user.");
                return false;
            } 

            if(booking.Status == BookingStatus.CANCELLED)
            {
                Console.WriteLine("Booking already cancelled.");
                return false;
            }

            booking.Cancel();

            booking.Flight.UpdateSeatCount(1);

            bookingRepository.UpdateBooking(booking);

            flightRepository.UpdateFlight(booking.Flight);

            return true;
        }
        public List<Booking> GetUserBookingsById(string userId)
        {
            throw new NotImplementedException();
        }
        public List<Booking> GetBookingHistory(string userId)
        {
            throw new NotImplementedException();
        }
    }
}