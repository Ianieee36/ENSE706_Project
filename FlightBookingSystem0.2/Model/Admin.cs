
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
            this.adminLevel = adminLevel;
        }

        public AdminLevel AdminLevel
        {
            get { return adminLevel; }
            private set { adminLevel = value; }
        }

        public bool CanAddFlight()
        {
            return AdminLevel == AdminLevel.FLIGHT_MANAGER || 
                   AdminLevel == AdminLevel.SYSTEM_ADMIN;
        }

        public bool CanUpdateFlight()
        {
            return AdminLevel == AdminLevel.FLIGHT_MANAGER ||
                   AdminLevel == AdminLevel.SYSTEM_ADMIN;
        }

        public bool CanDeleteFlight()
        {
            return AdminLevel == AdminLevel.FLIGHT_MANAGER ||
                   AdminLevel == AdminLevel.SYSTEM_ADMIN;
        }
        
        public bool CanManageFlight()
        {
            return AdminLevel == AdminLevel.FLIGHT_MANAGER ||
                   AdminLevel == AdminLevel.SYSTEM_ADMIN;
        }
        public bool CanAssistCustomers()
        {
            return AdminLevel == AdminLevel.SUPPORT_ADMIN;
        }

        public bool CanCreateAdmins()
        {
            return AdminLevel == AdminLevel.SYSTEM_ADMIN;
        }
    }
}