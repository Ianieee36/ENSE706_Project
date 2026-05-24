namespace FlightBookingSystem.Utilities
{
    public static class IdGenerator
    {
        public static string GenerateUserId()
        {
            return GenerateId("USR");
        }

        public static string GenerateFlightId()
        {
            return GenerateId("FL");
        }

        public static string GenerateBookingId()
        {
            return GenerateId("BK");
        }

        private static string GenerateId(string prefix)
        {
            Random random = new Random();

            int number = random.Next(100, 1000);

            return $"{prefix}{number}";
        }
    }
}