using System;
using FlightBookingSystem.Model;
using FlightBookingSystem.Repository;
using FlightBookingSystem.Utilities;

namespace FlightBookingSystem.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository userRepository; // interface dependency 

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository; // assign injected repository dependency
        }

        public User? Login(string email, string passwordHash)
        {
            User? user = userRepository.FindUserByEmail(email); // search email through the database

            if(user == null) // checks if user email exists 
            {
                return null;
            }

            if(user.PasswordHash != passwordHash) // checks if the stored password matches the entered password.
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

            string userId = GenerateUniqueUserId();

            User newUser = new User(
                userId,
                user.Email,
                user.PasswordHash,
                user.UserRole,
                user.FirstName,
                user.LastName,
                user.DateOfBirth,
                user.Address,
                user.PhoneNumber
            );
            
            // Save to database
            userRepository.SaveUser(newUser);

            // Return new registered user
            return newUser;
        }

        public User? Logout()
        {
            return null; // logouts user;
        }

        public string GenerateUniqueUserId()
        {
            string userId;

            do
            {
                userId = IdGenerator.GenerateUserId();
            }
            while (userRepository.UserIdExists(userId));

            return userId;
        }
    }
}
