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
            Booking? booking = bookingRepository.FindBookingById(bookingId); // checks db if the bookingId exists

            if(booking == null) // checks if fail to find bookingId
            {
                Console.WriteLine("Booking not found."); // return an error message
                return false;
            }

            if(booking.Customer.UserId != userId) // checks if the booking belongs to a specific user
            {
                Console.WriteLine("This booking does not belong to the user.");
                return false;
            } 

            if(booking.Status == BookingStatus.CANCELLED) // checks if the status is already cancelled or not 
            {
                Console.WriteLine("Booking already cancelled."); // cancellation message
                return false;
            }

            booking.Cancel(); // updates booking status to cancel

            booking.Flight.UpdateSeatCount(1); // update seat count

            bookingRepository.UpdateBooking(booking); // updates booking status in repository

            flightRepository.UpdateFlight(booking.Flight); // updates flight seats in repository.

            return true;
        }
        public List<Booking> GetUserBookingsById(string userId)
        {
            User? user = userRepository.FindUserById(userId); // checks if the user exists

            if(user == null) // checks if the user failed to find a user
            {
                return new List<Booking>(); // return a empty list instead of crashing or returning null
            }

            return bookingRepository.FindBookingsByUserId(userId); // if the user exists the service asks repo to retrieve all existing bookings.
        }
        public List<Booking> GetBookingHistory(string userId)
        {
            User? user = userRepository.FindUserById(userId); // checks if the user exists

            if(user == null) // checks if the user failed to find a user
            {
                return new List<Booking>(); // return a empty list instead of crashing or returning null
            }

            return bookingRepository.FindPastBookingsByUserId(userId); // if the user exists the services asks repo to retrieve past bookigns.
        }
    }
}