
namespace FlightBookingSystem.Model
{
    public class Admin : User
    {
        private AdminLevel adminLevel;

        public Admin(
            string userId,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string address,
            string phoneNumber,
            AdminLevel adminLevel)

            : base(
                userId,
                email,
                passwordHash,
                firstName,
                lastName,
                dateOfBirth,
                address,
                phoneNumber)
        {
            this.adminLevel = AdminLevel.SUPPORT_ADMIN;
        }

        public AdminLevel AdminLevel
        {
            get { return adminLevel; }
            private set { adminLevel = value; }
        }
    }
}