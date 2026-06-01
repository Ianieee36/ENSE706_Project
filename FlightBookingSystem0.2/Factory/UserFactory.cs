using FlightBookingSystem.Model;

namespace FlightBookingSystem.Factory
{
    public static class UserFactory // Applied Factory Method centralizes the creation of Admin and Customer object
    {
        // Create Customer Object
        public static Customer CreateCustomer(
            string userId,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string address,
            string phoneNumber)
        {
            return new Customer(
                userId,
                email,
                passwordHash,
                firstName,
                lastName,
                dateOfBirth,
                address,
                phoneNumber
            );
        }

        // Customer Constructor mainly for Database 
        public static Customer CreateCustomerFromDatabase(
            string userId,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string address,
            string phoneNumber,
            int loyaltyPoints,
            MembershipTier membershipTier)
        {
            return new Customer(
                userId,
                email,
                passwordHash,
                firstName,
                lastName,
                dateOfBirth,
                address,
                phoneNumber,
                loyaltyPoints,
                membershipTier
            );
        }

        // Create Admin Object
        public static Admin CreateAdmin(
            string userId,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string address,
            string phoneNumber,
            AdminLevel adminLevel)
        {
            return new Admin(
                userId,
                email,
                passwordHash,
                firstName,
                lastName,
                dateOfBirth,
                address,
                phoneNumber,
                adminLevel
            );
        }
    }
}