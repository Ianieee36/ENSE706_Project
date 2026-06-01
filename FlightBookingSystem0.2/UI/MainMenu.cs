using FlightBookingSystem.Utilities;
using FlightBookingSystem.Model;
using FlightBookingSystem.Services;
using FlightBookingSystem.Security;
using FlightBookingSystem.Factory;

namespace FlightBookingSystem.UI
{
    public class MainMenu
    {

        private readonly IFlightService flightService;
        private readonly IUserService userService;
        private readonly InputHelper inputHelper;
        private readonly CustomerMenu customerMenu;
        private readonly AdminMenu adminMenu;

        public MainMenu(IFlightService flightService, 
                        IUserService userService, 
                        InputHelper inputHelper, 
                        CustomerMenu customerMenu, 
                        AdminMenu adminMenu)
        {
            this.flightService = flightService;
            this.userService = userService;
            this.inputHelper = inputHelper;
            this.customerMenu = customerMenu;
            this.adminMenu = adminMenu;
        }

        public void DisplayMainMenu()
        {
            bool logout = false;

            while(!logout)
            {
                Console.Clear();
                Console.WriteLine("====== Welcome to Air New Zealand ======");
                Console.WriteLine("========== A great way to fly ==========");
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
                        HandleRegisterCustomer();
                        break;

                    case "4":
                        logout = true;
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
                Console.Clear();

                // Prompt for email
                string email = inputHelper.ReadRequiredInput("email");

                // Prompt for password
                string password = inputHelper.ReadRequiredInput("password"); 

                User? user = userService.Login(email, password);

                // validate if the email and password exists
                if(user == null)
                {
                    Console.WriteLine("Invalid password or email");
                    inputHelper.Pause();
                    continue;
                }

                // checks if the user is customer or admin
                if(user is Customer customer)
                {
                    customerMenu.DisplayCustomerMenu(customer);
                }
                else if(user is Admin admin)
                {
                    adminMenu.DisplayAdminMenu(admin);
                }

                return user;
            }
            
        }

        public User? HandleRegisterCustomer()
        {   
            Console.Clear();

            string email = inputHelper.ReadRequiredInput("email");
            string password = inputHelper.ReadRequiredInput("password");
            string firstName = inputHelper.ReadProperName("first name");
            string lastName = inputHelper.ReadProperName("last name");
            DateTime dob = inputHelper.ReadDateOfBirth();
            string address = inputHelper.ReadRequiredInput("address").ToUpper();
            string phoneNumber = inputHelper.ReadPhoneNumber();

            // creates new customer
            Customer? newCustomer = UserFactory.CreateCustomer(
                "",
                email,
                password,
                firstName,
                lastName,
                dob,
                address,
                phoneNumber
            );

            // registers customer and saves it to database
            Customer? customer = userService.RegisterCustomer(email, password, firstName, lastName, dob, 
                                                            address, phoneNumber);

            // check if the registered customer already existed
            if(customer == null)
            {
                Console.WriteLine("Registration failed. Email already exists");
                inputHelper.Pause();
                return null;
            } 

                Console.WriteLine("Registration successful.");
                inputHelper.Pause();
                return customer;
        }
            
        

        public void HandleSearchFlights()
        {
            while (true)
            {
                Console.Clear();

                string origin = inputHelper.ReadProperName("origin");
                string destination = inputHelper.ReadProperName("destination");

                List<Flight> flights =
                    flightService.SearchFlights(origin, destination);

                if (flights.Count == 0)
                {
                    Console.WriteLine("No flights found!");

                    Console.WriteLine("\n1. Search Again");
                    Console.WriteLine("2. Back to Main Menu");
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
                Console.WriteLine("2. Search Flights");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("Select option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        customerMenu.HandleViewFlightDetails();
                        return;

                    case "2":
                        continue;

                    case "3":
                        return;

                    default:
                        Console.WriteLine("Invalid input.");
                        inputHelper.Pause();
                        break;
                }
            }
        }
    }
}