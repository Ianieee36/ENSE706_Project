using FlightBookingSystem.Model;

namespace FlightBookingSystem.Repository
{
    public interface IUserRepository
    {
        User? FindUserById(string userId);
        User? FindUserByEmail(string email);
        void SaveUser(User user);
        bool EmailExists(string email);

        
    }
}