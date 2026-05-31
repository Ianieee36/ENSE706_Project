using System;
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

        public Customer? RegisterCustomer(Customer customer)
        {
            User? existingUser = userRepository.FindUserByEmail(customer.Email); // checks DB with any existing emails

            // Enail already exists
            if(existingUser != null) 
            {
                return null; 
            }

            string userId = GenerateUniqueCustomerId();

            Customer newCustomer = new Customer(
                userId,
                customer.Email,
                customer.PasswordHash,
                customer.FirstName,
                customer.LastName,
                customer.DateOfBirth,
                customer.Address,
                customer.PhoneNumber
            );
            
            // Save to database
            userRepository.SaveUser(newCustomer);

            // Return new registered user
            return newCustomer;
        }

        public Admin? RegisterAdmin(Admin currentAdmin, Admin admin)
        {   
            if(currentAdmin == null)
            {
                throw new Exception("Current admin cannot be null.");
            }

            if(!currentAdmin.CanCreateAdmins())
            {
                throw new Exception("Only system admins can create admin accounts.");
            }

            User? existingUser = userRepository.FindUserByEmail(admin.Email); // checks DB with any existing emails

            // Enail already exists
            if(existingUser != null) 
            {
                return null; 
            }

            string userId = GenerateUniqueAdminId();

            Admin newAdmin = new Admin(
                userId,
                admin.Email,
                admin.PasswordHash,
                admin.FirstName,
                admin.LastName,
                admin.DateOfBirth,
                admin.Address,
                admin.PhoneNumber,
                admin.AdminLevel
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
