using System;
using FlightBookingSystem.Model;

namespace FlightBookingSystem.Services
{
    public interface IUserService
    {
        User? Login(string email, string password);
        Customer? RegisterCustomer(Customer customer);
        Admin? RegisterAdmin(Admin currentAdmin, Admin admin);
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