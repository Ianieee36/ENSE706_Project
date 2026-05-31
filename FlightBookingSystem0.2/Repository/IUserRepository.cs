using FlightBookingSystem.Model;

namespace FlightBookingSystem.Repository
{
    public interface IUserRepository
    {
        User? FindUserById(string userId);
        User? FindUserByEmail(string email);
        void SaveUser(User user);
        bool UpdateUser(User user);
        bool UpdatePassword(string userId, string newPasswordHash);
        bool EmailExists(string email);
        void UpdateCustomerLoyalty(Customer customer);
        bool UserIdExists(string userId);

        
    }
}