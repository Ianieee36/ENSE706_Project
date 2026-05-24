using FlightBookingSystem.Services;
using FlightBookingSystem.Model;
using FlightBookingSystem.Utilities;

namespace FlightBookingSystem.UI
{
    public class AdminMenu
    {
        private readonly IFlightService flightService;
        private readonly InputHelper inputHelper;

        public AdminMenu(IFlightService flightService, InputHelper inputHelper) 
        {
            this.flightService = flightService;
            this.inputHelper = inputHelper;
        }

        public void DisplayAdminMenu(User admin)
        {
            bool logout = false;

            while(!logout)
            {
                Console.Clear();

                Console.WriteLine("===== Admin Only Access =====");
                Console.WriteLine($"Welcome, {admin.FirstName}");
                Console.WriteLine("1. View All Flights");
                Console.WriteLine("2. Add Flight");
                Console.WriteLine("3. Update Flight");
                Console.WriteLine("4. Remove Flight");
                Console.WriteLine("5. View Flight Occupancy");
                Console.WriteLine("6. Logout");
                Console.Write("Select option: ");

                string? choice = Console.ReadLine();

                switch(choice)
                {
                    case "1":
                        HandleViewAllFlights();
                        break;
                    
                    case "2":
                        HandleAddFlight();
                        break;

                    case "3":
                        HandleUpdateFlight();
                        break;

                    case "4":
                        HandleRemoveFlight();
                        break;

                    case "5":
                        HandleViewFlightOccupancy();
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

        private void HandleAddFlight()
        {
            Console.Clear();

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

            flightService.AddFlight(flight);

            Console.WriteLine("Flight added successfully.");
            inputHelper.Pause();
        }

        private void HandleRemoveFlight()
        {   
            Console.Clear();

            Console.Write("Enter flight ID to remove: ");
            string flightId = Console.ReadLine()!.ToUpper();

            flightService.RemoveFlightById(flightId);

            Console.WriteLine("Flight deleted successfully.");
            inputHelper.Pause();
        }

        private void HandleUpdateFlight()
        {
            Console.Clear();

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

            Flight updatedFlight = new Flight(
                flightId,
                newOrigin,
                newDestination,
                newDeparture,
                newTotalSeats,
                existingFlight.AvailableSeats
            );

            flightService.UpdateFlight(updatedFlight);

            Console.WriteLine("Flight updated successfully.");
            inputHelper.Pause();
        }

        private void HandleViewAllFlights()
        {
            Console.Clear();

            List<Flight> flights = flightService.GetAllFlights();

            if(flights.Count == 0)
            {
                Console.WriteLine("No flights found.");
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
    }
}