using FlightBookingSystem.Model;
using FlightBookingSystem.Services;
using FlightBookingSystem.Utilities;

namespace FlightBookingSystem.UI
{
    public class CustomerMenu
    {
        private readonly IFlightService flightService;
        private readonly IBookingService bookingService;
        private readonly InputHelper inputHelper;

        public CustomerMenu(IFlightService flightService, IBookingService bookingService, InputHelper inputHelper)
        {
            this.flightService = flightService;
            this.bookingService = bookingService;
            this.inputHelper = inputHelper;
        }

        public void DisplayCustomerMenu(User customer)
        {
            bool logout = false;

            while(!logout)
            {
                Console.Clear();

                Console.WriteLine("===== Air New Zealand =====");
                Console.WriteLine("===== We fly for you =====");
                Console.WriteLine($"\n==== Welcome, {customer.FirstName} ====");
                Console.WriteLine("1. Search Flights");
                Console.WriteLine("2. Manage my bookings");
                Console.WriteLine("3. View Booking History");
                Console.WriteLine("4. Logout");
                Console.Write("Select option: ");

                string? choice = Console.ReadLine();

                switch(choice)
                {
                    case "1":
                        HandleSearchFlights(customer);
                        break;
                    
                    case "2":
                        HandleManageBooking(customer);
                        break;

                    case "3":
                        HandleViewBookingHistory(customer);
                        break;

                    case "4":
                        logout = true;
                        break;

                    default:
                        Console.WriteLine("Choose from options (1-3)");
                        break;
                }
            }
        }

        private void HandleSearchFlights(User currentUser)
        {
            while (true)
            {
                Console.Clear();

                string origin = inputHelper.ReadProperName("origin")!;
                string destination = inputHelper.ReadProperName("destination")!;

                List<Flight> flights =
                    flightService.SearchFlights(origin, destination);

                if (flights.Count == 0)
                {
                    Console.WriteLine("No flights found.");
                    Console.WriteLine("\n1. Search Again");
                    Console.WriteLine("2. Back to Customer Menu");
                    Console.Write("Select option: ");

                    string? noResultChoice = Console.ReadLine();

                    switch (noResultChoice)
                    {
                        case "1":
                            continue;

                        case "2":
                            return;

                        default:
                            Console.WriteLine("Invalid input.");
                            inputHelper.Pause();
                            continue;
                    }
                }

                foreach (Flight f in flights)
                {
                    Console.WriteLine(
                        $"{f.FlightId} || {f.Origin} -> {f.Destination} || {f.DepartureDateTime:dd/MM/yyyy hh:mm tt}"
                    );
                }

                Console.WriteLine("\n1. View Flight Details");
                Console.WriteLine("2. Book Flight");
                Console.WriteLine("3. Search Again");
                Console.WriteLine("4. Back to Customer Menu");
                Console.Write("Select option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        HandleViewFlightDetails();
                        return;

                    case "2":
                        HandleBookFlight(currentUser);
                        return;

                    case "3":
                        continue;

                    case "4":
                        return;

                    default:
                        Console.WriteLine("Invalid input.");
                        inputHelper.Pause();
                        break;
                }
            }
        }

        public void HandleViewFlightDetails()
        {   
                Console.Write("Enter flight id: ");
                string? flightId = Console.ReadLine()!.ToUpper();

                if(string.IsNullOrWhiteSpace(flightId))
                {
                    Console.WriteLine("Flight Id is required.");
                    inputHelper.Pause();
                    return;
                }

                Flight? flight = flightService.GetFlightDetailsById(flightId);

                if(flight == null)
                {
                    Console.WriteLine("Flight not found");
                    inputHelper.Pause();
                    return;
                }

                Console.Clear();
                Console.WriteLine("==== Flight Details ====");
                Console.WriteLine($"======== {flightId} ========");
                Console.WriteLine($"Origin: {flight.Origin}");
                Console.WriteLine($"Destination: {flight.Destination}");
                Console.WriteLine($"Departure: {flight.DepartureDateTime:dd/MM/yyyy hh:mm tt}");
                Console.WriteLine($"Available Seats: {flight.AvailableSeats}");
                Console.WriteLine($"Total Seats: {flight.TotalSeats}");

                inputHelper.Pause();
        }

        private void HandleBookFlight(User currentUser)
        {
            while (true)
            {
                Console.Write("\nEnter Flight ID to book, or B to go back: ");
                string flightId = Console.ReadLine()!.ToUpper();

                if(flightId == "B")
                {
                    return;
                }

                if(string.IsNullOrWhiteSpace(flightId))
                {
                    Console.WriteLine("Flight ID is required.");
                    continue;
                }

                Booking? booking = bookingService.BookFlight(currentUser.UserId, flightId);

                if(booking == null)
                {
                    continue;

                }
               
                Console.WriteLine("\nBooking successful!");
                Console.WriteLine($"Booking ID: {booking.BookingId}");
                
                inputHelper.Pause();
                return;
            }
            
        }

        private void HandleManageBooking(User currentUser)
        {
            while(true)
            {
                Console.Clear();

                List<Booking> bookings = bookingService.GetCurrentBookingsByUserId(currentUser.UserId);

                if(bookings.Count == 0)
                {
                    Console.WriteLine("No current bookings found.");

                    inputHelper.Pause();
                    return;
                }

                Console.WriteLine("===== Current Bookings =====\n");

                foreach (Booking booking in bookings)
                {
                    Console.WriteLine(
                        $"Booking ID: {booking.BookingId}"
                    );

                    Console.WriteLine(
                        $"{booking.Flight.Origin} -> " +
                        $"{booking.Flight.Destination}"
                    );

                    Console.WriteLine(
                        $"Departure: " +
                        $"{booking.Flight.DepartureDateTime:dd/MM/yyyy hh:mm tt}"
                    );

                    Console.WriteLine(
                        $"Status: {booking.Status}"
                    );

                    Console.WriteLine("--------------------------");
                }

                Console.WriteLine("\n1. Cancel Booking");
                Console.WriteLine("2. Back to menu");
                Console.Write("Select option: ");

                string? choice = Console.ReadLine();

                switch(choice)
                {
                    case "1":
                        HandleCancelBooking(currentUser);
                        break;

                    case "2":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void HandleCancelBooking(User currentUser) 
        {
            Console.Write("Enter booking ID to cancel: ");
            string bookingId = Console.ReadLine()!;

            if(string.IsNullOrWhiteSpace(bookingId))
            {
                Console.WriteLine("Booking ID is required");
                return;
            }

            bool cancelled = bookingService.CancelBookingById(bookingId, currentUser.UserId);

            if(cancelled)
            {
                Console.WriteLine("Booking cancelled successfully.");
            }
            else
            {
                Console.WriteLine("Booking cancellation failed.");
            }

            inputHelper.Pause();
        }

        private void HandleViewBookingHistory(User currentUser)
        {
            Console.Clear();

            List<Booking> pastBookings = bookingService.GetBookingHistory(currentUser.UserId);

            Console.WriteLine("===== Booking History =====\n");

            if(pastBookings.Count == 0)
            {
                Console.WriteLine("No booking history found.");
                inputHelper.Pause();
                return;
            }

            foreach(Booking b in pastBookings)
            {   
                Console.WriteLine($"{b.BookingId} || " +
                                  $"{b.Flight.Origin} -> {b.Flight.Destination} || " +
                                  $"{b.Flight.DepartureDateTime: dd/MM/yyyy hh:mm tt}"

                );
            }

            inputHelper.Pause();
        } 
    }
}