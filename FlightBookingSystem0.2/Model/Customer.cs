
namespace FlightBookingSystem.Model
{
    public class Customer : User
    {
        private int loyaltyPoints;
        private string membershipTier;

        public Customer(
            string userId,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string address,
            string phoneNumber,
            int loyaltyPoints,
            string membershipTier)

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
            this.loyaltyPoints = loyaltyPoints;
            this.membershipTier = membershipTier;
        }

        public int LoyaltyPoints
        {
            get { return loyaltyPoints; }
            private set { loyaltyPoints = value; }
        }   

        public string MembershipTier
        {
            get { return membershipTier; }
            private set { membershipTier = value; }
        }
    }
}