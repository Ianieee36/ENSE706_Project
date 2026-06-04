using System;
using FlightBookingSystem.Factory;
using FlightBookingSystem.Model;
using FlightBookingSystem.Repository;
using FlightBookingSystem.Security;
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

        public User? Login(string email, string password)
        {
            User? user = userRepository.FindUserByEmail(email); // search email through the database

            string hashedPassword = PasswordHasher.HashPassword(password);

            if(user == null) // checks if user email exists 
            {
                return null;
            }

            if(user.PasswordHash != hashedPassword) // checks if the stored password matches the entered password.
            {
                return null;
            }

            return user; // return user
        }

        public Customer? RegisterCustomer(string email, string password, string firstName, string lastName,
            DateTime dateOfBirth, string address, string phoneNumber)
        {
            User? existingUser = userRepository.FindUserByEmail(email); // checks DB with any existing emails

            // Enail already exists
            if(existingUser != null) 
            {
                return null; 
            }

            string userId = GenerateUniqueCustomerId();

            string hashPassword = PasswordHasher.HashPassword(password);

            Customer newCustomer = UserFactory.CreateCustomer(
                userId,
                email,
                hashPassword,
                firstName,
                lastName,
                dateOfBirth,
                address,
                phoneNumber
            );
            
            // Save to database
            userRepository.SaveUser(newCustomer);

            // Return new registered user
            return newCustomer;
        }

        public Admin? RegisterAdmin(Admin currentAdmin, string email, string password, string firstName, string lastName,
            DateTime dateOfBirth, string address, string phoneNumber, AdminLevel adminLevel)
        {   
            if(currentAdmin == null)
            {
                throw new Exception("Current admin cannot be null.");
            }

            if(!currentAdmin.CanCreateAdmins())
            {
                throw new Exception("Only system admins can create admin accounts.");
            }

            User? existingUser = userRepository.FindUserByEmail(email); // checks DB with any existing emails

            // Enail already exists
            if(existingUser != null) 
            {
                return null; 
            }

            string userId = GenerateUniqueAdminId();

            string hashPassword = PasswordHasher.HashPassword(password);

            Admin newAdmin =  UserFactory.CreateAdmin(
                userId,
                email,
                hashPassword,
                firstName,
                lastName,
                dateOfBirth,
                address,
                phoneNumber,
                adminLevel
            );
            
            // Save to database
            userRepository.SaveUser(newAdmin);

            // Return new registered user
            return newAdmin;
        }

        public User? Logout()
        {
            return null; // logouts user;
        }

        public bool UpdateProfile(User user,
                                  string? email = null,
                                  string? firstName = null,
                                  string? lastName = null,
                                  string? address = null,
                                  string? phoneNumber = null)
        {
            if (user == null)
                throw new Exception("User cannot be null.");

            user.UpdateProfile(email, firstName, lastName, address, phoneNumber);

            return userRepository.UpdateUser(user);
        }

        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {
            User? user = userRepository.FindUserById(userId);

            if(user == null)
            {
                throw new Exception("User not found.");
            }

            string oldPasswordHash = PasswordHasher.HashPassword(oldPassword);

            if(user.PasswordHash != oldPasswordHash)
            {
                throw new Exception("Old password is incorrect");
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                throw new Exception("New password cannot be empty.");
            }

            string newPasswordHash = PasswordHasher.HashPassword(newPassword);

            user.ChangePassword(newPasswordHash);
            
            return userRepository.UpdatePassword(userId, newPasswordHash);
        }

        public User? GetUserById(string userId)
        {
            return userRepository.FindUserById(userId);
        }
        private string GenerateUniqueCustomerId()
        {
            string userId;

            do
            {
                userId = IdGenerator.GenerateCustomerId();
            }
            while (userRepository.UserIdExists(userId));

            return userId;
        }

        private string GenerateUniqueAdminId()
        {
            string userId;

            do
            {
                userId = IdGenerator.GenerateAdminId();
            }
            while (userRepository.UserIdExists(userId));

            return userId;
        }
    }
}
