using System;
using FlightBookingSystem.Model;

namespace FlightBookingSystem.Services
{
    public interface IUserService
    {
        User? Login(string email, string password);
        User? Register(string email, string password, Role role, string firstName, 
                             string lastName, DateTime dateOfBirth, 
                             string address, string phoneNumber);
        User? Logout();
    }
}