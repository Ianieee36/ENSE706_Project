using FlightBookingSystem.Model;

namespace FlightBookingSystem.Factory
{
    public static class UserFactory
    {
        public static Customer CreateCustomer(
            string userId,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string address,
            string phoneNumber,
            int loyaltyPoints,
            string membershipTier)
        {
            return new Customer(userId, email, passwordHash, firstName, lastName,
                dateOfBirth, address, phoneNumber);
        }

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
            return new Admin(userId, email, passwordHash, firstName, lastName,
                dateOfBirth, address, phoneNumber, adminLevel);
        }
    }
}