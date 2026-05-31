using FlightBookingSystem.Repository;
using FlightBookingSystem.Services;
using FlightBookingSystem.Utilities;
using FlightBookingSystem.UI;
using FlightBookingSystem.Security;

public class Program
    {   
        static void Main(string[] args) // Main Menu
        {
            //Utilities
            InputHelper inputHelper = new InputHelper();

            // repositories
            IUserRepository userRepository = new UserRepository();
            IFlightRepository flightRepository = new FlightRepository();
            IBookingRepository bookingRepository = new BookingRepository(userRepository, flightRepository);
            ITicketRepository ticketRepository = new TicketRepository(bookingRepository);

            // services
            ITicketService ticketService = new TicketService(
                ticketRepository
            );

            IUserService userService = new UserService(userRepository);

            IFlightService flightService = new FlightService(flightRepository);

            IBookingService bookingService = new BookingService(
                bookingRepository,
                userRepository,
                flightRepository,
                ticketService,
                ticketRepository
            );

            // menus
            CustomerMenu customerMenu = new CustomerMenu(
                flightService,
                bookingService,
                ticketService,
                userService,
                inputHelper
            );

            AdminMenu adminMenu = new AdminMenu(
                flightService,
                bookingService,
                userService,
                inputHelper
            );

            MainMenu mainMenu = new MainMenu(
                flightService,
                userService,
                inputHelper,
                customerMenu,
                adminMenu
            );

            mainMenu.DisplayMainMenu();

        }
    }
