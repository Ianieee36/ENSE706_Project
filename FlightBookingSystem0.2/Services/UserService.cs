using System;
using FlightBookingSystem.Model;
using FlightBookingSystem.Repository;
using FlightBookingSystem.Security;

namespace FlightBookingSystem.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository userRepository; // interface dependency 

        public UserService()
        {
            userRepository = new UserRepository(); // instantiate the UserRepository class
        }

        public User? Login(string email, string password)
        {
            User? user = userRepository.FindUserByEmail(email); // search email through the database

            if(user == null) // checks if user email exists 
            {
                return null;
            }

            string hashedInputPassword = PasswordHasher.HashPassword(password);

            if(user.PasswordHash != hashedInputPassword) // checks if the stored password matches the entered password.
            {
                return null;
            }

            return user; // return user
        }

        public User? Register(User user)
        {
            User? existingUser = userRepository.FindUserByEmail(user.Email); // checks DB with any existing emails

            // Enail already exists
            if(existingUser != null) 
            {
                return null; 
            }
            
            // Save to database
            userRepository.SaveUser(user);

            // Return registered user
            return user;
        }

        public User? Logout()
        {
            return null; // logouts user;
        }
    }
}
