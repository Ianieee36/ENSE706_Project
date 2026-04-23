using System;
using System.Collections.Generic;
using FlightBookingSystem.Model;
 class Program
    {
        static List<User> users = new List<User> // in-memory lists
            {
                new User("AD01", "admin01@airnz.co.nz", "pass123", Role.ADMIN, "Admin User", new DateTime(1990, 01, 01), "Auckland", "22122341243"),
                new User("CS01", "customer01@hotmail.com", "pass123", Role.CUSTOMER, "Customer User", new DateTime(2003, 03, 06), "ChristChurch", "23155876432")
    
            };

        static List<Flight> flights = new List<Flight>
            {
                new Flight("F001", "Auckland", "Wellington", new DateTime(2026, 4, 18, 8, 0, 0), 2),
                new Flight("F002", "Wellington", "Christchurch", new DateTime(2026, 4, 20, 20, 0, 0), 1)
            };


        static List<Booking> bookings = new List<Booking>(); // storage for created bookings
        static int nextBookingNumber = 1; // counter for bookingId

        static int nextCustomerNumber = 1; // counter for customerId

        static void Main(string[] args) // Main Menu
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("\n============ Air New Zealand ==============");
                Console.WriteLine("====Flight Booking System (Demo Version) ====");
                Console.WriteLine("1. Search for flights");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Register");
                Console.WriteLine("4. Exit");
                Console.Write("Select option: ");

                string? choice = Console.ReadLine();

                switch(choice)
                {
                    case "1": 
                        SearchFlights();
                        break;
                    case "2": 
                        Login();
                        break;
                    case "3": 
                        RegisterCustomer();
                        break;
                    case "4": 
                        exit = true;
                        break;
                    default: 
                        Console.WriteLine("Invalid");
                        Pause();
                        break;
                }
            }
        }
        static void Login() // Login Menu
        {
            Console.Clear();
            Console.WriteLine("=== Welcome to Air New Zealand ===");
            Console.WriteLine("===== Login to your account ===== ");
            Console.Write("Enter your email: ");
            string? email = Console.ReadLine();

            Console.Write("Enter your password:");
            string? password = Console.ReadLine();

            User? foundUser = null;

            foreach(User u in users) // search from list of Users
            {
                if(u.Email.ToLower() == email.ToLower() && u.Password.ToLower() == password.ToLower()) // validate email and password
                {
                    foundUser = u; // user found
                    break;
                }
            }

            if(foundUser == null) // user does not exists
            {
                Console.WriteLine("Invalid login"); 
                Pause();
                return;
            }

            Console.WriteLine($"Login successful. Welcome {foundUser.Name}");
            Pause();

            if(foundUser.UserRole == Role.CUSTOMER) // check if the user is customer
            {
                CustomerMenu(foundUser); // displays customer menu
            }
            else if(foundUser.UserRole == Role.ADMIN)
            {
                AdminMenu(); // displays admin menu
            }
        }

        static void CustomerMenu(User customer) 
        {
            bool logout = false;

            while(!logout)
            {
                Console.Clear();
                Console.WriteLine("====== Welcome to Air New Zealand ======");
                Console.WriteLine($"====== Kia Ora, {customer.Name} ====== ");
                Console.WriteLine("1. Search Flights ");
                Console.WriteLine("2. Book Flight ");
                Console.WriteLine("3. View my Bookings ");
                Console.WriteLine("4. Logout");
                Console.Write("Select option: ");

                string? choice = Console.ReadLine();

                switch(choice)
                {
                    case "1":
                        SearchFlights();
                        break;
                    case "2":
                        BookFlight(customer);
                        break;
                    case "3":
                        ViewMyBookings(customer);
                        break;
                    case "4":
                        logout = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        Pause();
                        break;
                }
            }
        }

        static void AdminMenu()
        {
            bool logout = false;

            while(!logout)
            {
                Console.Clear();
                Console.WriteLine("====== Welcome to Air New Zealand ======");
                Console.WriteLine("============ Admin Access ==============");
                Console.WriteLine("1. Add Flight");
                Console.WriteLine("2. View All Flights");
                Console.WriteLine("3. Logout");
                Console.Write("Select option: ");

                string? choice = Console.ReadLine();

                switch(choice)
                {
                    case "1":
                        AddFlight();
                        break;
                    case "2":
                        ViewAllFlights();
                        break;
                    case "3":
                        logout = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        Pause();
                        break;
                }
            }
        }

        static void SearchFlights() 
        {
            Console.Clear();
            Console.WriteLine("=== Search for Flights ===");
            Console.Write("Enter origin: ");
            string? origin = Console.ReadLine();

            Console.Write("Enter destination: ");
            string? destination = Console.ReadLine();

            List<Flight> results = new List<Flight>(); // create an empty list

            foreach(Flight f in flights) // iterate through list of flights
            {
                if(f.Origin.ToLower() == origin.ToLower() && 
                   f.Destination.ToLower() == destination.ToLower()) // checks if the flight exists
                {
                    results.Add(f); // flight exists
                }
            }

            if(results.Count == 0) 
            {
                Console.WriteLine("No flights found."); 
                Pause();
                return;
            }
            
            // Displays available flights
            Console.WriteLine("\nAvailable Flights:");
            foreach(Flight fList in results)
            {
                Console.WriteLine($"{fList.FlightId} | {fList.Origin} -> {fList.Destination} | {fList.DepartureDateTime:dd/MM/yyyy hh:mm tt} | Seats: {fList.AvailableSeats}");
            }

            Console.WriteLine("\n1. View Flight Details"); // prompts if user wants to view flight details
            Console.WriteLine("2. Back to Menu");
            string? choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    ViewFlightDetails();
                    break;
                case "2":
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    Pause();
                    break;
            }       
        }

        static void ViewFlightDetails() 
        {
            Console.Clear();
            Console.WriteLine("\nEnter Flight ID:"); 
            string? flightId = Console.ReadLine();

            Flight? selectedFlight = null; // create a placeholder for selected flight 

            foreach(Flight f in flights) // iterate through list of flights
            {
                if(f.FlightId.ToLower() == flightId.ToLower()) // checks if flightId matches to any existing flights
                {
                    selectedFlight = f; // flight found
                    break;
                }
            }

            if(selectedFlight == null) 
            {
                Console.WriteLine("Flight not found.");
            }
            else
            {
                // Displays flight details
                Console.WriteLine($"\nFlight ID: {selectedFlight.FlightId}");
                Console.WriteLine($"Origin: {selectedFlight.Origin}");
                Console.WriteLine($"Destination: {selectedFlight.Destination}");
                Console.WriteLine($"Departure: {selectedFlight.DepartureDateTime:dd/MM/yyyy hh:mm tt}");
                Console.WriteLine($"Available Seats: {selectedFlight.AvailableSeats}");
                Console.WriteLine($"Total Seats: {selectedFlight.TotalSeats}");
            }
                
            Pause();
        }

        static void RegisterCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== Register as Customer === ");

            string userId = "CS" + (nextCustomerNumber + 1).ToString("00"); // creates unique userId for newly registered customer.

            Console.Write("Enter email: ");
            string? email = Console.ReadLine();

            bool exists = false; 

            foreach(User u in users) // iterate through list of users
            {
                if(u.Email.ToLower() == email.ToLower()) // checks if the email matches any registered users
                {
                    exists = true; // matched
                    break;
                }
            }

            if (exists)
            {
                Console.WriteLine("Email already exists. Please login to your account");
                Pause();
                return;
            }

            Console.Write("Enter password: ");
            string? password = Console.ReadLine();

            Console.Write("Enter full name: ");
            string? name = Console.ReadLine();

            DateTime dateOfBirth;

            while (true)
            {
                Console.Write("Enter date of birth (mm/dd/yyyy): ");
                if(DateTime.TryParse(Console.ReadLine(), out dateOfBirth)) // it converts string into DateTime format
                {
                    break;
                }
                Console.WriteLine("Invalid date. Please try again");
            }

            Console.Write("Enter address: ");
            string? address = Console.ReadLine();

            Console.Write("Enter phone number: ");
            string? phoneNumber = Console.ReadLine();

            User newCustomer = new User( // add new User object
                userId,
                email!,
                password!,
                Role.CUSTOMER,
                name!,
                dateOfBirth,
                address!,
                phoneNumber!
            );

            users.Add(newCustomer); // add to the list
            nextCustomerNumber++; // increments UserId 

            Console.WriteLine("Registration successful. You can now log in as a customer.");
            Pause();
        }

        static void BookFlight(User customer)
        {
            Console.Clear();
            Console.WriteLine("\n=== Book Flight ===");
            ViewAllFlightWithoutPause();

            Console.Write("\nEnter Flight ID to book: ");
            string? flightId = Console.ReadLine().ToUpper();

            Flight? selectedFlight = null; 

            foreach(Flight f in flights)
            {
                if(f.FlightId.ToLower() == flightId.ToLower())
                {
                    selectedFlight = f;
                    break;
                }
            }
                
            if(selectedFlight == null)
            {
                Console.WriteLine("Flight not found.");
                Pause();
                return;
            }

            if (!selectedFlight.HasAvailableSeats()) // checks if flight has available seats
            {
                Console.WriteLine("Sorry, there are no available seats!");
                Pause();
                return;
            }

            bool alreadyBooked = false;

            foreach(Booking b in bookings) // avoids double booking
            {
                if(b.Customer.UserId == customer.UserId &&          
                   b.Flight.FlightId == selectedFlight.FlightId &&
                   b.Status != BookingStatus.CANCELLED) 
                {
                    alreadyBooked = true;
                    break;
                }
            }

            if (alreadyBooked)
            {
                Console.WriteLine("You are already booked in this flight.");
                Pause();
                return;
            }

            Booking booking = new Booking( // new Booking object
                "B" + nextBookingNumber.ToString("000"),
                customer,
                selectedFlight,
                DateTime.Now
            );

            selectedFlight.UpdateSeatCount(1); // updates seat count 
            booking.Confirm(); // change booking status to CONFIRMED
            bookings.Add(booking); // add bookings to the list
            nextBookingNumber++; // increments bookingId

            Console.WriteLine("\n===You successfully booked your flight===");
            Console.WriteLine("============= Safe Travels! =============");
            Console.WriteLine($"Booking ID: {booking.BookingId}");
            Console.WriteLine($"Status: {booking.Status}");
            Pause();
                
        }

        static void ViewMyBookings(User customer)
        {
            Console.Clear();
            Console.WriteLine("\n====== My Bookings ======");

            List<Booking> myBookings = new List<Booking>(); // empty list

            foreach(Booking b in bookings)
            {
                if(b.Customer.UserId == customer.UserId)
                {
                    myBookings.Add(b); // add to the list
                }
            }

            if(myBookings.Count == 0) 
            {
                Console.WriteLine("No Bookings Found");
            }
            else
            {   
                // displays all bookings
                foreach(Booking booking in myBookings)
                {
                    Console.WriteLine($"\n{booking.BookingId} | {booking.Flight.FlightId} | {booking.Flight.Origin} -> {booking.Flight.Destination} | {booking.Flight.DepartureDateTime} | {booking.Status}");
                }
            }                    
            Pause();
        }

        static void AddFlight() // Admin-Acess Only
        {
            Console.Clear();
            Console.WriteLine("\n====== Add Flight ======");

            Console.Write("\nEnter Flight ID: ");
            string? flightId = Console.ReadLine();

            bool flightExists = false; 

            foreach(Flight f in flights) 
            {
                if(f.FlightId.ToLower() == flightId.ToLower()) // checks flight existence
                {
                    flightExists = true; // flight exists
                    break;
                }
            }

            if(flightExists)
            {   
                Console.WriteLine("Flight ID already exists");
                Pause();
                return;
            }

            Console.Write("Enter Origin: ");
            string? origin = Console.ReadLine();

            Console.Write("Enter Destination: ");
            string? destination = Console.ReadLine();

            DateTime departureDateTime; // stores date and time

            while (true)
            {
                Console.Write("Enter departure date and time (mm/dd/yyyy hh:mm AM/PM): ");
                if(DateTime.TryParse(Console.ReadLine(), out departureDateTime)) 
                {
                    break;
                }
                Console.WriteLine("Invalid date/time. Please try again.");
            }

            int totalSeats;
            while(true)
            {
                Console.WriteLine("Enter total seats: ");
                if(int.TryParse(Console.ReadLine(), out totalSeats) && totalSeats > 0)
                {
                    break;
                }
                Console.WriteLine("Please enter a valid number greater than 0");
            }

            Flight newFlight = new Flight( // add new Flight object
                flightId!,
                origin!,
                destination!,
                departureDateTime,
                totalSeats
            );

            flights.Add(newFlight); // add to the list of flights

            Console.WriteLine("Flight added successfully");
            Pause();
        }

        static void ViewAllFlights()
        {
            Console.Clear();
            Console.WriteLine("=== All Flights ===");
            ViewAllFlightWithoutPause();
            Pause();
        }

        static void ViewAllFlightWithoutPause() // helper to list flights
        {
            foreach(Flight flight in flights)
            {
                Console.WriteLine($"{flight.FlightId} | {flight.Origin} -> {flight.Destination} | {flight.DepartureDateTime:dd/MM/yyyy hh:mm tt} | Seats: {flight.AvailableSeats}/{flight.TotalSeats}");
            }
        }

        static void Pause() // helper buffer 
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
