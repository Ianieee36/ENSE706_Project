using FlightBookingSystem.Model;
using FlightBookingSystem.Repository;
using FlightBookingSystem.Utilities;

namespace FlightBookingSystem.Services
{
    internal class BookingService : IBookingService
    {  
        private readonly IBookingRepository bookingRepository;
        private readonly IUserRepository userRepository;
        private readonly IFlightRepository flightRepository;
        private readonly ITicketService ticketService;
        private readonly ITicketRepository ticketRepository;
        public BookingService(IBookingRepository bookingRepository, IUserRepository userRepository, 
                              IFlightRepository flightRepository, ITicketService ticketService, ITicketRepository ticketRepository)
        {
            this.bookingRepository = bookingRepository;
            this.userRepository = userRepository;
            this.flightRepository = flightRepository;
            this.ticketService = ticketService;
            this.ticketRepository = ticketRepository;
        }


        public Booking BookFlight(string customerId, string flightId) // Books Flight
        {   
        
            // finds user by userId
            User? user = userRepository.FindUserById(customerId);
            
            // checks if user exists
            if(user == null) 
            {
                throw new Exception("Customer not found");
            }

            // checks if user Role if Customer or not
            if(user is not Customer)
            {
                throw new Exception("Only customers can book flights.");
            }

            // finds flight by flightId
            Flight? flight = flightRepository.FindFlightById(flightId);

            // checks if the flight exists or not
            if(flight == null)
            {
                throw new Exception("Flight not found");
            }
            
            // checks if the flight still has available seats
            if (!flight.HasAvailableSeats())
            {
                throw new Exception("No available seats for this flight.");
            }

            List<Booking> existingBookings = bookingRepository.FindBookingsByCustomerId(customerId);

            foreach(Booking booking in existingBookings)
            {
                if(booking.Flight.FlightId == flightId && booking.IsActive())
                {
                    throw new Exception("You already booked this flight.");
                }
            }

            string bookingId = GenerateUniqueBookingId();

            // creates new booking
            Booking newBooking = new Booking(
                bookingId,
                DateTime.Now,
                BookingStatus.CONFIRMED,
                user,
                flight
            );

            // saves booking to DB
            bookingRepository.SaveBooking(newBooking);
            // updates flight seat count
            flight.UpdateSeatCount(-1);
            // updates flight 
            flightRepository.UpdateFlight(flight);

            // Issue Ticket
            Ticket bookingTicket = ticketService.IssueTicket(newBooking);

            // Save Ticket
            ticketRepository.SaveTicket(bookingTicket);

            // Explicit casting
            Customer customer = (Customer)user;

            // Adds 100 points after booking
            customer.AddLoyaltyPoints(100);

            // Update customer loyalty points
            userRepository.UpdateCustomerLoyalty(customer);

            // returns newBooking
            return newBooking;
        }

        public Booking? GetBookingById(string bookingId)
        {
            return bookingRepository.FindBookingById(bookingId);
        }
        public bool CancelBookingById(Booking booking, string customerId)
        {

            if(booking == null) // checks if fail to find bookingId
            {
                throw new Exception("Booking not found."); // return an error message
            }

            if(booking.Customer.UserId != customerId) // checks if the booking belongs to a specific user
            {
                throw new Exception("You can only cancel your own booking.");
                
            } 

            if(!booking.IsActive()) // checks if the status is already cancelled or not 
            {
                throw new Exception("Booking is already cancelled."); // cancellation message
            }

            booking.CancelBooking(); // updates booking status to cancel

            bookingRepository.UpdateBooking(booking); // updates booking status in repository

            booking.Flight.UpdateSeatCount(1); // update seat count 

            flightRepository.UpdateFlight(booking.Flight); // updates flight seats in repository.

            Ticket? ticket = ticketRepository.FindTicketByBookingId(booking.BookingId);

            if(ticket != null)
            {
                ticket.CancelTicket();
                ticketRepository.UpdateTicket(ticket);
            }

            return true;
        }
        public List<Booking> GetUserBookingsById(string customerId)
        {
            User? user = userRepository.FindUserById(customerId); // checks if the user exists

            if(user == null) // checks if the user failed to find a user
            {
                return new List<Booking>(); // return a empty list instead of crashing or returning null
            }

            return bookingRepository.FindBookingsByCustomerId(customerId); // if the user exists the service asks repo to retrieve all existing bookings.
        }
        public List<Booking> GetCurrentBookingsByUserId(string customerId)
        {
            User? user = userRepository.FindUserById(customerId);

            if (user == null)
            {
                return new List<Booking>();
            }

            return bookingRepository.FindCurrentBookingsByCustomerId(customerId);
        }
        public List<Booking> GetBookingHistory(string customerId)
        {
            User? user = userRepository.FindUserById(customerId); // checks if the user exists

            if(user == null) // checks if the user failed to find a user
            {
                return new List<Booking>(); // return a empty list instead of crashing or returning null
            }

            return bookingRepository.FindPastBookingsByCustomerId(customerId); // if the user exists the services asks repo to retrieve past bookigns.
        }

        public Booking? AssistCustomerBooking(Admin admin, string customerId, string flightId)
        {
            if(admin == null)
            {
                throw new Exception("Admin cannot be null");
            }

            if(!admin.CanAssistCustomers())
            {
                throw new Exception("You do not have permission to assist customers.");
            }

            User? customer = userRepository.FindUserById(customerId);

            if(customer is not Customer)
            {
                throw new Exception("The selected user is not a customer.");
            }

            return BookFlight(customerId, flightId);
        }

        public bool AssistCustomerCancelBooking(Admin admin, string bookingId, string customerId)
        {
            if(admin == null)
            {
                throw new Exception("Admin cannot be null");
            }

            if(!admin.CanAssistCustomers())
            {
                throw new Exception("You do not have a permission to assist customers");
            }

            Booking? booking = bookingRepository.FindBookingById(bookingId);

            if(booking == null)
            {
                throw new Exception("Booking not found.");
            }

            if(booking.Customer.UserId != customerId)
            {
                throw new Exception("This booking does not belong to the selected customer.");
            }

            return CancelBookingById(booking, customerId);

            
        }

        private string GenerateUniqueBookingId()
        {
            string bookingId;

            do
            {
                bookingId = IdGenerator.GenerateBookingId();
            }
            while (bookingRepository.BookingIdExists(bookingId));

            return bookingId;
        }
    }
}