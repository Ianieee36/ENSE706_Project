using FlightBookingSystem.Model;
using FlightBookingSystem.Services;
using FlightBookingSystem.Utilities;

namespace FlightBookingSystem.UI
{
    public class CustomerMenu
    {
        private readonly IFlightService flightService;
        private readonly IBookingService bookingService;
        private readonly ITicketService ticketService;
        private readonly IUserService userService;
        private readonly InputHelper inputHelper;

        public CustomerMenu(IFlightService flightService, IBookingService bookingService, 
                            ITicketService ticketService, IUserService userService, InputHelper inputHelper)
        {
            this.flightService = flightService;
            this.bookingService = bookingService;
            this.ticketService = ticketService;
            this.userService = userService;
            this.inputHelper = inputHelper;
        }

        public void DisplayCustomerMenu(Customer customer)
        {
            bool logout = false;

            while(!logout)
            {
                Console.Clear();

                Console.WriteLine("=============================");
                Console.WriteLine("====== Air New Zealand ======");
                Console.WriteLine("====== We fly for you  ======");
                Console.WriteLine("=============================");
                Console.WriteLine($"\n==== Welcome, {customer.FirstName} =====");
                Console.WriteLine($"=== Airpoints: {customer.LoyaltyPoints} : {customer.MembershipTier} ===");
                Console.WriteLine("=============================");
                Console.WriteLine("1. Search Flights");
                Console.WriteLine("2. Manage my bookings");
                Console.WriteLine("3. View Booking History");
                Console.WriteLine("4. Manage Account");
                Console.WriteLine("5. Logout");
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
                        HandleManageAccount(customer);
                        break;

                    case "5":
                        logout = true;
                        break;

                    default:
                        Console.WriteLine("Choose from options (1-3)");
                        break;
                }
            }
        }

        private void HandleSearchFlights(Customer currentUser)
        {
            while (true)
            {
                Console.Clear();

                string origin = inputHelper.ReadProperName("origin (enter B to go back)")!;
                string destination = inputHelper.ReadProperName("destination (enter B to go back)")!;

                if(origin == "B" || destination == "B")
                {
                    return;
                }

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
                        $"{f.FlightId} || {f.Origin} -> {f.Destination} || {f.DepartureDateTime:dd/MM/yyyy hh:mm}"
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
            Console.WriteLine("\n");
            Console.Write("Enter flight id (enter B to go back): ");
            string? flightId = Console.ReadLine()!.ToUpper();

            if(flightId == "B")
            {
                return;
            }

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
            Console.WriteLine($"Departure: {flight.DepartureDateTime:dd/MM/yyyy hh:mm}");
            Console.WriteLine($"Available Seats: {flight.AvailableSeats}");
            Console.WriteLine($"Total Seats: {flight.TotalSeats}");

            inputHelper.Pause();
        }

        private void HandleBookFlight(Customer currentUser)
        {
            try
            {
                Console.Write("\nEnter Flight ID to book (enter B to go back): ");
                string? flightId = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(flightId))
                {
                    Console.WriteLine("Flight ID is required.");
                    inputHelper.Pause();
                    return;
                }

                flightId = flightId.ToUpper();

                if (flightId == "B")
                {
                    return;
                }

                Booking? booking = bookingService.BookFlight(currentUser.UserId, flightId);

                if (booking == null)
                {
                    Console.WriteLine("Booking failed.");
                    inputHelper.Pause();
                    return;
                }

                User? refreshedUser = userService.GetUserById(currentUser.UserId);

                if (refreshedUser is Customer refreshedCustomer)
                {
                    currentUser.UpdateLoyaltyInfo(
                        refreshedCustomer.LoyaltyPoints,
                        refreshedCustomer.MembershipTier
                    );
                }

                Console.WriteLine("\nBooking successful!");
                Console.WriteLine($"Booking ID: {booking.BookingId}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            inputHelper.Pause();
        }

        private void HandleManageBooking(Customer currentUser)
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
                    if (booking.IsActive())
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
                            $"{booking.Flight.DepartureDateTime:dd/MM/yyyy hh:mm}"
                        );

                        Console.WriteLine(
                            $"Status: {booking.BookingStatus}"
                        );

                        Console.WriteLine("\n----------------------------");   
                    }
                }

                Console.WriteLine("\n1. View Ticket");
                Console.WriteLine("2. Cancel Booking");
                Console.WriteLine("3. Back to menu");
                Console.Write("Select option: ");

                string? choice = Console.ReadLine();

                switch(choice)
                {   
                    case "1":
                        HandleViewTicket();
                        break;

                    case "2":
                        HandleCancelBooking(currentUser);
                        break;

                    case "3":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private void HandleCancelBooking(Customer currentUser) 
        {

            try
            {
                Console.Write("Enter booking ID to cancel (enter B to go back): ");
                string bookingId = Console.ReadLine()!.ToUpper();

                if(bookingId == "B")
                {
                    return;
                }

                if(string.IsNullOrWhiteSpace(bookingId))
                {
                    Console.WriteLine("Booking ID is required");
                    return;
                }

                Booking booking = bookingService.GetBookingById(bookingId)!;

                if(booking == null)
                {
                    Console.WriteLine("Booking not found");
                    inputHelper.Pause();
                    return;
                }

                bool cancelled = bookingService.CancelBookingById(booking, currentUser.UserId);

                if(cancelled)
                {
                    Console.WriteLine("Booking cancelled successfully.");
                }
                else
                {
                    Console.WriteLine("Booking cancellation failed.");
                }

            }
            catch(Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            
            inputHelper.Pause();
        }

        private void HandleViewBookingHistory(Customer currentUser)
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
                                  $"{b.Flight.DepartureDateTime: dd/MM/yyyy hh:mm}"

                );
            }

            inputHelper.Pause();
        }

        private void HandleViewTicket()
        {
            Console.WriteLine("\n");

            try
            {
                Console.Write("Enter Booking ID (enter B to go back): ");
                string bookingId = Console.ReadLine()!.ToUpper();

                if(bookingId == "B")
                {
                    return;
                }

                Console.Clear();

                if (string.IsNullOrWhiteSpace(bookingId))
                {
                    Console.WriteLine("Booking ID is required");
                }

                Ticket? bookingTicket = ticketService.GetTicketByBookingId(bookingId);

                if(bookingTicket == null)
                {
                    Console.WriteLine("Ticket has not been issued");
                    inputHelper.Pause();
                    return;
                }
                
                Console.Clear();
                Console.WriteLine("\n======== Ticket Details ========");
                Console.WriteLine($"Passenger Name: {bookingTicket.Booking.Customer.FullName}");
                Console.WriteLine($"Flight: {bookingTicket.Booking.Flight.FlightId}");
                Console.WriteLine($"Date: {bookingTicket.IssueDate: dd/MM}");
                Console.WriteLine($"Time: {bookingTicket.BoardingTime: HH:mm}");
                Console.WriteLine($"Gate Number: {bookingTicket.GateNumber}");
                Console.WriteLine($"Seat Number: {bookingTicket.SeatNumber}");
                Console.WriteLine("===================================");    
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            inputHelper.Pause();
        }

        public void HandleManageAccount(Customer customer)
        {
            Console.Clear();

            Console.WriteLine("\n====== Manage Account ======");
            Console.WriteLine("1. Update Profile Information");
            Console.WriteLine("2. Change Password");
            Console.WriteLine("3. Back");
            Console.Write("Select options: ");
            
            string? choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    HandleUpdateProfile(customer);
                    break;
                
                case "2":
                    HandleChangePassword(customer);
                    break;
                
                case "3":
                    return;
                
                default:
                    Console.WriteLine("Invalid options");
                    break;
            }
        } 

        public void HandleUpdateProfile(Customer currentCustomer)
        {
            Console.Clear();

            Console.WriteLine("\n======= Update Profile =======");
            Console.WriteLine("1. Change email");
            Console.WriteLine("2. Change first name");
            Console.WriteLine("3. Change last name");
            Console.WriteLine("4. Change address");
            Console.WriteLine("5. Change phone number");
            Console.WriteLine("6. Back");

            int choice = inputHelper.ReadPositiveInt("choice");

            try
            {
                switch(choice)
                {
                    case 1:
                    {
                        string email = inputHelper.ReadRequiredInput("new email");

                        userService.UpdateProfile(currentCustomer, email: email);

                        Console.WriteLine("Email updated successfully.");
                        break;
                    }

                    case 2:
                    {
                        string firstName = inputHelper.ReadRequiredInput("new first name");

                        userService.UpdateProfile(currentCustomer, firstName: firstName);

                        Console.WriteLine("First name updated successfully.");
                        break;
                    }

                    case 3:
                    {
                        string lastName = inputHelper.ReadRequiredInput("new last name");

                        userService.UpdateProfile(currentCustomer, lastName: lastName);

                        Console.WriteLine("Last name updated successfully.");
                        break;           
                    }

                    case 4:
                    {
                        string address = inputHelper.ReadRequiredInput("new address");

                        userService.UpdateProfile(currentCustomer, address: address);

                        Console.WriteLine("Address updated successfully.");
                        break;           
                    }

                    case 5:
                    {
                        string phoneNumber = inputHelper.ReadRequiredInput("new phone number");

                        userService.UpdateProfile(currentCustomer, phoneNumber: phoneNumber);

                        Console.WriteLine("Phone number updated successfully.");
                        break;           
                    }

                    case 6:
                        return;

                    default:
                        Console.WriteLine("Invalid option");
                        break;            
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            inputHelper.Pause();
        }

        public void HandleChangePassword(Customer currentCustomer)
        {
            Console.Clear();

            try
            {
                Console.WriteLine("\n========= Change Password ===========");

                string oldPassword = inputHelper.ReadRequiredInput("current password (enter B to go back)");

                string newPassword = inputHelper.ReadRequiredInput("new password (enter B to go back)");

                string confirmPassword = inputHelper.ReadRequiredInput("confirm password (enter B to go back)");

                if(oldPassword == "B" || newPassword == "B" || confirmPassword == "B")
                {
                    return;
                }
                
                if(newPassword != confirmPassword)
                {
                    Console.WriteLine("Password do not match.");
                    inputHelper.Pause();
                    return;
                }

                bool changed = userService.ChangePassword(
                    currentCustomer.UserId,
                    oldPassword,
                    newPassword
                );

                if(changed)
                {
                    Console.WriteLine("Password changed successfully.");
                }
                else
                {
                    Console.WriteLine("Password change failed.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            inputHelper.Pause();
        }
    }
}