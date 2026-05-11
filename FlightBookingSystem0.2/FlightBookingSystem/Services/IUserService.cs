using FlightBookingSystem.Model;

namespace FlightBookingSystem.Services
{
    public interface IUserService
    {
        User? Login(string email, string password);
        User? Register(string email, string password, Role role);
        void Logout(string userId);
    }
}