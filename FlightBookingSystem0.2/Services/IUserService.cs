using System;
using FlightBookingSystem.Model;

namespace FlightBookingSystem.Services
{
    public interface IUserService
    {
        User? Login(string email, string password);
        Customer? RegisterCustomer(string email, string password, string firstName, string lastName,
            DateTime dateOfBirth, string address, string phoneNumber);
        Admin? RegisterAdmin(Admin currentAdmin, string email, string password, string firstName, string lastName,
            DateTime dateOfBirth, string address, string phoneNumber, AdminLevel adminLevel);
        User? Logout();

        bool UpdateProfile(User user,
                                  string? email = null,
                                  string? firstName = null,
                                  string? lastName = null,
                                  string? address = null,
                                  string? phoneNumber = null);

        bool ChangePassword(string userId, string oldPassword, string newPassword);

        User? GetUserById(string userId);


    }
}