using FlightBookingSystem.Services;
using FlightBookingSystem.Model;
using FlightBookingSystem.Utilities;
using FlightBookingSystem.Security;

namespace FlightBookingSystem.UI
{
    public class AdminMenu
    {
        private readonly IFlightService flightService;
        private readonly IBookingService bookingService;
        private readonly IUserService userService;
        private readonly InputHelper inputHelper;

        public AdminMenu(IFlightService flightService, IBookingService bookingService, IUserService userService,
                         InputHelper inputHelper) 
        {
            this.flightService = flightService;
            this.bookingService = bookingService;
            this.userService = userService;
            this.inputHelper = inputHelper;
            
        }

        public void DisplayAdminMenu(Admin admin)
        {
            bool logout = false;

            while(!logout)
            {
                Console.Clear();

                Console.WriteLine("===== Admin Only Access =====");
                Console.WriteLine($"====={admin.UserId + "-" + admin.AdminLevel}=====");
                Console.WriteLine("1. View All Flights");
                Console.WriteLine("2. Manage Flights");
                Console.WriteLine("3. View Flight Occupancy");
                Console.WriteLine("4. Customer Support");
                Console.WriteLine("5. Create Admin");
                Console.WriteLine("6. Logout");
                Console.Write("Select option: ");

                string? choice = Console.ReadLine();

                switch(choice)
                {
                    case "1":
                        HandleViewAllFlights();
                        break;
                    
                    case "2":
                        HandleManageFlights(admin);
                        break;

                    case "3":
                        HandleViewFlightOccupancy();
                        break;

                    case "4":
                        HandleAssistCustomer(admin);
                        break;

                    case "5":
                        HandleCreateAdmin(admin);
                        break;

                    case "6":
                        logout = true;
                        break;

                    default:
                        Console.WriteLine("Choose from options (1-6)");
                        break;

                }

            }
        }

        private void HandleManageFlights(Admin admin)
        {
            Console.Clear();

            if (!admin.CanManageFlight())
            {
                Console.WriteLine("You do not have permission to access managing flights");
                inputHelper.Pause();
                return;
            }

            try
            {
                Console.WriteLine("1. Add Flight");
                Console.WriteLine("2. Update Flight");
                Console.WriteLine("3. Remove flight");
                Console.WriteLine("4. Back");
                Console.Write("Select options: ");

                string? choice = Console.ReadLine();

                switch(choice)
                {
                    case "1":
                        HandleAddFlight(admin);
                        break;
                    
                    case "2":
                        HandleUpdateFlight(admin);
                        break;
                    
                    case "3":
                        HandleRemoveFlight(admin);
                        break;
                    
                    case "4":
                        return;
                    
                    default:
                        Console.WriteLine("Invalid options");
                        break;
                }    
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            inputHelper.Pause();
        }

        private void HandleAddFlight(Admin admin)
        {
            Console.Clear();

            if (!admin.CanAddFlight())
            {
                Console.WriteLine("You do not have permission to add flights.");
                inputHelper.Pause();
                return;
            }

            try
            {
                string? origin = inputHelper.ReadProperName("origin");

                string? destination = inputHelper.ReadProperName("destination");

                DateTime departure = inputHelper.ReadDateTime("departure date/time", "yyyy-MM-dd HH:mm");

                int totalSeats = inputHelper.ReadPositiveInt("total seats");

                Flight flight = new Flight(
                "",
                origin,
                destination,
                departure,
                totalSeats
                );

                flightService.AddFlight(admin, flight);

                Console.WriteLine("Flight added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            inputHelper.Pause();
        }

        private void HandleRemoveFlight(Admin admin)
        {   
            Console.Clear();

            if(!admin.CanDeleteFlight())
            {
                Console.WriteLine("You do not have permission to delete flight.");
                inputHelper.Pause();
                return;
            }

            try
            {
                Console.Write("Enter flight ID to remove: ");
                string flightId = Console.ReadLine()!.ToUpper();

                if(string.IsNullOrWhiteSpace(flightId))
                {
                    Console.WriteLine("Flight ID cannot be empty.");
                    inputHelper.Pause();
                    return;
                }

                flightService.RemoveFlightById(admin, flightId);

                Console.WriteLine("Flight deleted successfully.");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        private void HandleUpdateFlight(Admin admin)
        {
            Console.Clear();

            if (!admin.CanUpdateFlight())
            {
                Console.WriteLine("You do not have permission to update a flight.");
                inputHelper.Pause();
                return;
            }

            try
            {
                Console.Write("Enter flight ID to update: ");
                string flightId = Console.ReadLine()!.ToUpper();

                Flight? existingFlight = flightService.GetFlightDetailsById(flightId);

                if(existingFlight == null)
                {
                    Console.WriteLine("Flight not found.");
                    inputHelper.Pause();
                    return;
                }  

                string newOrigin = inputHelper.ReadProperName("new origin");

                string newDestination = inputHelper.ReadProperName("new destination");

                DateTime newDeparture = inputHelper.ReadDateTime("departure date/time", "yyyy-MM-dd HH:mm");
                
                int newTotalSeats = inputHelper.ReadPositiveInt("new total seats");

                int bookedSeats = existingFlight.TotalSeats - existingFlight.AvailableSeats;

                if(newTotalSeats < bookedSeats)
                {
                    Console.WriteLine($"Cannot set total seats below current booked seats ({bookedSeats})."
                    );
                    inputHelper.Pause();
                    return;
                }

                int newAvailableSeats = newTotalSeats - bookedSeats;

                Flight updatedFlight = new Flight(
                    flightId,
                    newOrigin,
                    newDestination,
                    newDeparture,
                    newTotalSeats,
                    newAvailableSeats
                );

                flightService.UpdateFlight(admin, updatedFlight);

                Console.WriteLine("Flight updated successfully."); 
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        private void HandleViewAllFlights()
        {
            Console.Clear();

            List<Flight> flights = flightService.GetAllFlights();

            if(flights.Count == 0)
            {
                Console.WriteLine("No flights found.");
                inputHelper.Pause();
                return;
            }
            else
            {
                foreach(Flight flight in flights)
                {
                    Console.WriteLine($@"{flight.FlightId} || {flight.Origin} -> {flight.Destination} || {flight.DepartureDateTime:dd/MM/yyyy hh:mm tt} || {flight.AvailableSeats}/{flight.TotalSeats}");
                }
            }

            inputHelper.Pause();
        }

        private void HandleViewFlightOccupancy()
        {   
            Console.Clear();

            Console.Write("Enter flight ID: ");
            string flightId = Console.ReadLine()!.ToUpper();

            Flight? flight = flightService.GetFlightDetailsById(flightId);

            if(flight == null)
            {
                Console.WriteLine("Flight not found.");
                inputHelper.Pause();
                return;
            }

            int bookedSeats = flight.TotalSeats - flight.AvailableSeats;

            Console.WriteLine($"Flight: {flight.FlightId}");
            Console.WriteLine($"Route: {flight.Origin} -> {flight.Destination}");
            Console.WriteLine($"Total Seats: {flight.TotalSeats}");
            Console.WriteLine($"Available Seats: {flight.AvailableSeats}");
            Console.WriteLine($"Booked Seats: {bookedSeats}");

            inputHelper.Pause();

        }

        private void HandleCreateAdmin(Admin currentAdmin)
        {
            Console.Clear();

            if (!currentAdmin.CanCreateAdmins())
            {
                Console.WriteLine("You do not have permission to create admins.");
                inputHelper.Pause();
                return;
            }

            try
            {
                string email = inputHelper.ReadRequiredInput("email");
                string password = inputHelper.ReadRequiredInput("password");
                string firstName = inputHelper.ReadRequiredInput("first name");
                string lastName = inputHelper.ReadRequiredInput("last name");
                DateTime dob = inputHelper.ReadDateOfBirth();
                string address = inputHelper.ReadRequiredInput("address");
                string phoneNumber = inputHelper.ReadPhoneNumber();

                Console.WriteLine("Select admin level:");
                Console.WriteLine("1. Support Admin");
                Console.WriteLine("2. Flight Manager");
                Console.WriteLine("3. System Admin");

                int choice = inputHelper.ReadPositiveInt("choice");

                AdminLevel adminLevel = choice switch
                {
                    1 => AdminLevel.SUPPORT_ADMIN,
                    2 => AdminLevel.FLIGHT_MANAGER,
                    3 => AdminLevel.SYSTEM_ADMIN,
                    _ => throw new Exception("Invalid admin level.")
                };

                string hashedPassword = PasswordHasher.HashPassword(password);

                Admin newAdmin = new Admin(
                    "",
                    email,
                    hashedPassword,
                    firstName,
                    lastName,
                    dob,
                    address,
                    phoneNumber,
                    adminLevel
                );

                User? createdAdmin = userService.RegisterAdmin(currentAdmin, newAdmin);

                if(createdAdmin == null)
                {
                    Console.WriteLine("Admin creation failed. Email already exist.");
                    inputHelper.Pause();
                    return;
                }

                Console.WriteLine($"{createdAdmin.UserId} created successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            inputHelper.Pause();
        }

        private void HandleAssistCustomer(Admin admin)
        {
            Console.Clear();

            if(!admin.CanAssistCustomers())
            {
                Console.WriteLine("You do not have permission to book a flight.");
                inputHelper.Pause();
                return;
            }

            Console.Clear();

            Console.WriteLine("====== Customer Support ======");
            Console.WriteLine("1. Book flight");
            Console.WriteLine("2. Cancel booking");
            Console.Write("Select options: ");

            string? choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    BookCustomerAFlight(admin);
                    break;

                case "2":
                    CancelCustomerBooking(admin);
                    break;

                default:
                    Console.WriteLine("Invalid options");
                    break;


            }
        }

        private void BookCustomerAFlight(Admin admin) 
        {
            Console.Clear();

            try
            {
                Console.Write("Enter Customer ID: ");
                string customerId = Console.ReadLine()!.ToUpper();

                if(string.IsNullOrWhiteSpace(customerId))
                {
                    Console.WriteLine("Customer ID is required.");
                    inputHelper.Pause();
                    return;
                }

                Console.Write("Enter Flight ID: ");
                string flightId = Console.ReadLine()!.ToUpper();

                if(string.IsNullOrWhiteSpace(flightId))
                {
                    Console.WriteLine("Flight ID is required.");
                    inputHelper.Pause();
                    return;
                }

                Booking? booking = bookingService.AssistCustomerBooking(
                    admin,
                    customerId.ToUpper(),
                    flightId.ToUpper()
                );

                Console.WriteLine("Customer flight booked successfully.");
                Console.WriteLine("Booking ID: {booking.BookingId}");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            inputHelper.Pause();
        }

        private void CancelCustomerBooking(Admin admin)
        {
            Console.Clear();

            try
            {
                Console.Write("Enter Customer ID: ");
                string customerId = Console.ReadLine()!.ToUpper();

                if(string.IsNullOrWhiteSpace(customerId))
                {
                    Console.WriteLine("Customer ID is required.");
                    inputHelper.Pause();
                    return;
                }

                Console.Write("Enter Booking ID: ");
                string bookingId = Console.ReadLine()!.ToUpper();

                if(string.IsNullOrWhiteSpace(bookingId))
                {
                    Console.WriteLine("Booking ID is required.");
                    inputHelper.Pause();
                    return;
                }

                bool cancelled = bookingService.AssistCustomerCancelBooking(
                    admin,
                    bookingId,
                    customerId
                );

                if(cancelled)
                {
                    Console.WriteLine("Customer booking cancelled successfully.");
                }
                else
                {
                    Console.WriteLine("Booking cancellation failed.");
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