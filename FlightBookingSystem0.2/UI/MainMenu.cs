using System;
using System.Linq;
using System.Collections.Generic;
using FlightBookingSystem.Utilities;
using FlightBookingSystem.Model;
using FlightBookingSystem.Services;

namespace FlightBookingSystem.UI
{
    public class MainMenu
    {

        private readonly IFlightService flightService;
        private readonly IUserService userService;
        private readonly InputHelper inputHelper;

        public MainMenu()
        {
            flightService = new FlightService();
            userService = new UserService();
            inputHelper = new InputHelper();
        }

        public void DisplayMainMenu()
        {
            bool exit = false;

            while(!exit)
            {
                Console.Clear();
                Console.WriteLine("\n====== Welcome to Air New Zealand ======");
                Console.WriteLine("1. Search Flights");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Register");
                Console.WriteLine("4. Exit");
                Console.Write("Select option: ");

                string? choice = Console.ReadLine();

                switch(choice)
                {
                    case "1":
                        HandleSearchFlights();
                        break;
                    
                    case "2":
                        HandleLogin();
                        break;

                    case "3":
                        HandleRegister();
                        break;

                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Choose from options (1-4)");
                        break;

                }

            }
        }

        public User? HandleLogin()
        {
            while(true)
            {
                // Prompt for email
                Console.Write("Enter email: ");
                string email = Console.ReadLine()!;

                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("Email is required");
                    inputHelper.Pause();
                    continue;
                }

                // Prompt for password
                Console.WriteLine("Enter password: ");
                string password = Console.ReadLine()!; 

                if(string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Password is required");
                    inputHelper.Pause();
                    continue;
                }

                User? user = userService.Login(email, password);

                // validate if the email and password exists
                if(user == null)
                {
                    Console.WriteLine("Invalid password or email");
                    inputHelper.Pause();
                    return null;
                }

                // checks if the user is customer or admin
                if(user.UserRole is Role.CUSTOMER)
                {
                    Console.WriteLine("Opening Customer Menu");
                }
                else if(user.UserRole is Role.ADMIN)
                {
                    Console.WriteLine("Opening Admin Menu");
                }

                return user;
            }
            
        }

        public User? HandleRegister()
        {   
            while(true)
            {
            
            Console.Clear();

            // prompt for email
            Console.Write("Enter email:");
            string? email = Console.ReadLine();

            if(string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Email is required!");
                inputHelper.Pause();
                continue;
            }

            // prompt for password
            Console.Write("Enter password: ");
            string? password = Console.ReadLine();

            if(string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password is required!");
                inputHelper.Pause();
                continue;
            }

            // prompt for First Name
            Console.Write("Enter first name: ");
            string? firstName = inputHelper.ToUpperInputReader("firstName");

            if(string.IsNullOrWhiteSpace(firstName))
            {
                Console.WriteLine("First name is required!");
                inputHelper.Pause();
                continue;
            }

            // prompt for Last Name
            Console.Write("Enter last name: ");
            string? lastName = inputHelper.ToUpperInputReader("lastName");

            if(string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Last name is required!");
                inputHelper.Pause();
                continue;
            }

            // prompt for dateofbirth
            Console.Write("Enter date of birth (dd/MM/yyyy) ");
            string? dateOfBirth = Console.ReadLine();

            DateTime dob;

            // convert string to exact DateTime format (dd/MM/yyyy)
            while(!DateTime.TryParseExact(
                dateOfBirth,
                "dd/MM/yyyy",
                null,
                System.Globalization.DateTimeStyles.None,
                out dob))
            {
                Console.WriteLine("Invalid format. User dd/MM/yyyy");

                Console.WriteLine("Enter date of birth: ");
                dateOfBirth = Console.ReadLine();
            }

            // prompt for address
            Console.Write("Enter address: ");
            string? address = inputHelper.ToUpperInputReader("address");

            if(string.IsNullOrWhiteSpace(address))
            {
                Console.WriteLine("Address is required!");
                inputHelper.Pause();
                continue;
            }

            // prompt for phonenumber
            Console.WriteLine("Enter phone number (9 digits): ");
            string? phoneNumber = Console.ReadLine();

            if(string.IsNullOrWhiteSpace(phoneNumber))
            {
                Console.WriteLine("Phone number is required!");
                inputHelper.Pause();
                continue;
            }
            
            // makes sures only digits are valid
            if(!phoneNumber.All(char.IsDigit))
            {
                Console.WriteLine("Digits only!");
                inputHelper.Pause();
                continue;
            }

            // makes sure 9 digits is entered.
            if(phoneNumber.Length < 9)
            {
                Console.WriteLine("Phone number must be 9 digits!");
                inputHelper.Pause();
                continue;
            }
            
            // generates unique userId
            string userId = Guid.NewGuid().ToString();

            // creates new customer
            User? customer = new User(
                userId,
                email,
                password,
                Role.CUSTOMER,
                firstName,
                lastName,
                dob,
                address,
                phoneNumber
            );

            // registers customer and saves it to database
            User? newCustomer = userService.Register(customer);

            // check if the registered customer already existed
            if(newCustomer == null)
            {
                Console.WriteLine("Registration failed. Email already exists");
                inputHelper.Pause();
                return null;
            } 

                Console.WriteLine("Registration successful.");
                inputHelper.Pause();
                return newCustomer;
            }
            
        }

        public void HandleSearchFlights()
        {
            while(true)
            {
            
            Console.Clear();

            // prompt for origin
            Console.Write("Enter origin: ");
            string? origin = inputHelper.ToUpperInputReader("origin");
            
            if(string.IsNullOrWhiteSpace(origin))
            {
                Console.WriteLine("Origin is required");
                inputHelper.Pause();
                continue;        
            }
            // prompt for destination
            Console.Write("Enter destination: ");
            string? destination = inputHelper.ToUpperInputReader("destination");

            if(string.IsNullOrWhiteSpace(destination))
            {
                Console.WriteLine("Destination is required");
                inputHelper.Pause();
                continue;        
            }

            List<Flight> flights = flightService.SearchFlights(origin, destination);

            if(flights.Count == 0)
            {
                Console.WriteLine("No flights found!");
                inputHelper.Pause();
                continue;
            }

            foreach(Flight f in flights)
            {
                Console.WriteLine($"{f.FlightId} || {f.Origin} -> {f.Destination} || {f.DepartureDateTime:dd/MM/yyyy hh:mm tt}");
                inputHelper.Pause();        
            }

            Console.WriteLine("\n 1. View Flight Details");
            Console.WriteLine("2. Search Flights");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Select option: ");
            string? choice = Console.ReadLine();

            switch(choice)
                {
                    case "1":
                        HandleViewFlightDetails();
                        break;
                    case "2":
                        continue;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }

        }

        public void HandleViewFlightDetails()
        {   
            while (true)
            {
                Console.Clear();
                Console.Write("Enter flight id: ");
                string? flightId = Console.ReadLine()!;

                if(string.IsNullOrWhiteSpace(flightId))
                {
                    Console.WriteLine("Flight Id is required.");
                    continue;
                }

                Flight? flight = flightService.GetFlightDetailsById(flightId);

                if(flight == null)
                {
                    Console.WriteLine("Flight not found");
                    continue;
                }

                Console.WriteLine("\n====== Flight Details ======");
                Console.WriteLine($"======== {flightId} ========");
                Console.WriteLine($"Origin: {flight.Origin}");
                Console.WriteLine($"Destination: {flight.Destination}");
                Console.WriteLine($"Departure: {flight.DepartureDateTime:dd/MM/yyyy hh:mm tt}");
                Console.WriteLine($"Available Seats: {flight.AvailableSeats}");
                Console.WriteLine($"Total Seats: {flight.TotalSeats}");

                inputHelper.Pause();

            }   
        }
    }
}