namespace FlightBookingSystem.Repository
{
    public interface IUserRepository
    {
        User? FindUserByEmail(string email);
        void SaveUser(User user);
        bool EmailExists(string email);

        
    }
}