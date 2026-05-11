using System;
using FlightBookingSystem.Model;

namespace FlightBookingSystem.Services
{
    internal class UserService : IUserService
    {
        public User Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public User Register(string email, string password, Role role)
        {
            throw new NotImplementedException();
        }

        public void Logout(string userId)
        {
            
        }
    }
}
