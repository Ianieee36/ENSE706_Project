
namespace FlightBookingSystem.Model
{
    public class Customer : User // Inherits from User Class
    {
        private int loyaltyPoints;
        private MembershipTier membershipTier;

        
        public Customer(
            string userId,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string address,
            string phoneNumber
            )

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
            this.loyaltyPoints = 0;
            this.membershipTier = MembershipTier.BRONZE;
        }

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
            MembershipTier membershipTier)
            : base(userId, email, passwordHash, firstName, lastName, dateOfBirth, address, phoneNumber)
        {
            LoyaltyPoints = loyaltyPoints;
            MembershipTier = membershipTier;
        }

        public int LoyaltyPoints
        {
            get { return loyaltyPoints; }
            private set { loyaltyPoints = value; }
        }   

        public MembershipTier MembershipTier
        {
            get { return membershipTier; }
            private set { membershipTier = value; }
        }

        public void UpdateLoyaltyInfo(int loyaltyPoints, MembershipTier membershipTier)
        {
            LoyaltyPoints = loyaltyPoints;
            MembershipTier = membershipTier;
        }

        public void AddLoyaltyPoints(int points)
        {
            if(points <= 0)
            {
                throw new Exception("Points must be greater than zero.");
            }

            LoyaltyPoints += points;
            UpdateMembershipTier();


        }

        private void UpdateMembershipTier()
        {
            if(LoyaltyPoints >= 1000)
            {
                MembershipTier = MembershipTier.GOLD;
            }
            else if(LoyaltyPoints >= 500)
            {
                MembershipTier = MembershipTier.SILVER;
            }
            else
            {
                MembershipTier = MembershipTier.BRONZE;
            }
        }
    }
}