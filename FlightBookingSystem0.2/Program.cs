using FlightBookingSystem.Repository;
using FlightBookingSystem.Services;
using FlightBookingSystem.Utilities;
using FlightBookingSystem.UI;
using FlightBookingSystem.Security;

public class Program
    {   
        static void Main(string[] args) // Main Menu
        {
            
            // Database connection
            DatabaseConnection dbConnection = new DatabaseConnection();

            // Repositories
            UserRepository userRepository = new UserRepository(dbConnection);

            FlightRepository flightRepository = new FlightRepository(dbConnection);

            BookingRepository bookingRepository = new BookingRepository(
                userRepository,
                flightRepository,
                dbConnection
            );

            // Services
            UserService userService = new UserService(userRepository);

            FlightService flightService = new FlightService(flightRepository);

            BookingService bookingService = new BookingService(
                bookingRepository,
                userRepository,
                flightRepository
            );

            // Utilities 
            InputHelper inputHelper = new InputHelper();

            // Menus
            CustomerMenu customerMenu = new CustomerMenu(
                flightService,
                bookingService,
                inputHelper
            );

            AdminMenu adminMenu = new AdminMenu(
                flightService,
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
