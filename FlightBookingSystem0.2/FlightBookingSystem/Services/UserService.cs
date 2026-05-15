using System;
using FlightBookingSystem.Model;
using FlightBookingSystem.Repository;

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

            if(user.PasswordHash != password) // checks if the stored password matches the entered password.
            {
                return null;
            }

            return user; // return user
        }

        public User? Register(string email, string password, Role role, string firstName, 
                             string lastName, DateTime dateOfBirth, 
                             string address, string phoneNumber)
        {
            User? existingUser = userRepository.FindUserByEmail(email); // checks DB with any existing emails

            if(existingUser != null) // checks if theres existing email
            {
                return null; 
            }

            string userId = Guid.NewGuid().ToString(); // Generate ID

            User newUser = new User( // creates a new User
                userId,
                email,
                password,
                role,
                firstName,
                lastName,
                dateOfBirth,
                address,
                phoneNumber
            );

            userRepository.SaveUser(newUser); // saves newUser to DB
            
            return newUser;
        }

        public User? Logout()
        {
            return null; // logouts user;
        }
    }
}
