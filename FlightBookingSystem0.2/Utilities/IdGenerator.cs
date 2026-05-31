namespace FlightBookingSystem.Utilities
{
    public static class IdGenerator
    {
        public static string GenerateUserId()
        {
            return GenerateId("USR");
        }

        public static string GenerateCustomerId()
        {
            return GenerateId("CS");
        }

        public static string GenerateAdminId()
        {
            return GenerateId("AD");
        }

        public static string GenerateFlightId()
        {
            return GenerateId("FL");
        }

        public static string GenerateBookingId()
        {
            return GenerateId("BK");
        }

        public static string GenerateTicketId()
        {
            return GenerateId("TKT");
        }

        public static string GenerateTicketNumber()
        {
            return GenerateId("TN");
        }

        public static string GenerateSeatNumber()
        {
            return "SN" + Random.Shared.Next(1, 181);
        }

        public static string GenerateGateNumber()
        {
            return "G0" + Random.Shared.Next(1, 21);
        }

        



        private static string GenerateId(string prefix)
        {
            Random random = new Random();

            int number = random.Next(100, 1000);

            return $"{prefix}{number}";
        }


    }
}