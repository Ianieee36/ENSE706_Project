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

        public string ReadRequiredInput(string fieldName)
        {
            while(true)
            {
                Console.Write($"Enter {fieldName}: ");
                string? input = Console.ReadLine();

                if(!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine($"{fieldName} is required!");
            }
        }

        public string ReadPhoneNumber()
        {
            while(true)
            {
                Console.WriteLine("Enter phone number (10 digits): ");
                string? phoneNumber = Console.ReadLine();

                if(string.IsNullOrWhiteSpace(phoneNumber))
                {
                    Console.WriteLine("Phone number is required!");
                    continue;
                }

                if(!phoneNumber.All(char.IsDigit))
                {
                    Console.WriteLine("Digits only!");
                    continue;
                }

                if(phoneNumber.Length != 10)
                {
                    Console.WriteLine("Phone number must be 9 digits");
                    continue;
                }

                return phoneNumber;
            }
        }

        public DateTime ReadDateOfBirth()
        {
            while(true)
            {
                Console.Write("Enter date of birth (dd/MM/yyyy): ");
                string? input = Console.ReadLine();

                if(DateTime.TryParseExact(
                    input,
                    "dd/MM/yyyy",
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out DateTime dob))
                {
                    return dob;
                }

                Console.WriteLine("Invalid format. Use dd/MM/yyyy.");
            }
            
        }

        public string ReadProperName(string fieldName)
        {
            while (true)
            {
                Console.Write($"Enter {fieldName}: ");

                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine($"{fieldName} is required.");
                    continue;
                }

                input = input.Trim();

                string formattedName =
                    char.ToUpper(input[0]) +
                    input.Substring(1).ToLower();

                return formattedName;
            }
        }

        public DateTime ReadDateTime(string fieldName, string format)
        {
            while (true)
            {
                Console.Write($"Enter {fieldName} ({format}): ");

                string? input = Console.ReadLine();

                if (DateTime.TryParseExact(
                    input,
                    format,
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out DateTime result))
                {
                    return result;
                }

                Console.WriteLine($"Invalid format. Use {format}");
            }
        }

        public int ReadPositiveInt(string fieldName)
        {
            while (true)
            {
                Console.Write($"Enter {fieldName}: ");

                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int number))
                {
                    Console.WriteLine("Numbers only.");
                    continue;
                }

                if (number <= 0)
                {
                    Console.WriteLine($"{fieldName} must be greater than zero.");
                    continue;
                }

                return number;
            }
        }

    }
}