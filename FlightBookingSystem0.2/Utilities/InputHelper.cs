using System;
using System.Globalization;

namespace FlightBookingSystem.Utilities
{
    public class InputHelper
    {
        private TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        public string? ToUpperInputReader(string fieldName)
        {      
                return Console.ReadLine()?.ToUpper();
        }

        public void Pause() // helper buffer 
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}