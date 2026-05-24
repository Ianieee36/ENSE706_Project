using System;
using FlightBookingSystem.Model;

namespace FlightBookingSystem.Services
{
    public interface IUserService
    {
        User? Login(string email, string password);
        User? Register(User user);
        User? Logout();
        string GenerateUniqueUserId();
    }
}