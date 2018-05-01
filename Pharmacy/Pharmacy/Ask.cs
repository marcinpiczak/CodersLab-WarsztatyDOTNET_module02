using System;
using System.Collections.Generic;
using System.Text;

namespace Pharmacy
{
    class Ask
    {
        public static string ForString(string message, bool allowEmpty = false, string stringValueIfEmptyAllowed = "")
        {
            ConsoleEx.Write(ConsoleColor.Green, message);
            string input = Console.ReadLine();

            if (input == "" && allowEmpty)
            {
                return stringValueIfEmptyAllowed;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                ConsoleEx.Write(ConsoleColor.Red, "Podałeś pustą wartość. \n");
                input = ForString(message, allowEmpty, stringValueIfEmptyAllowed);
            }

            return input;
        }

        public static DateTime ForDate(string message, bool allowEmpty = false, DateTime? datetimeValueIfEmptyAllowed = null)
        {
            string input = ForString(message, allowEmpty, "");
            //bool dateOk = DateTime.TryParse(input, out DateTime date);
            //if (!dateOk)

            if (input == "" && allowEmpty)
            {
                return datetimeValueIfEmptyAllowed.HasValue ? datetimeValueIfEmptyAllowed.Value : DateTime.MinValue;
            }

            if (!DateTime.TryParse(input, out DateTime date))
            {
                ConsoleEx.Write(ConsoleColor.Red, "Podana data jest nieprawidłowa. \n");
                date = ForDate(message, allowEmpty, datetimeValueIfEmptyAllowed);
            }

            return date;
        }

        public static bool ForBool(string message, bool allowEmpty = false, bool boolValueIfEmptyAllowed = false)
        {
            string input = ForString(message, allowEmpty, "");

            if (input == "" && allowEmpty)
            {
                return boolValueIfEmptyAllowed;
            }

            bool result = false;

            if (input.ToLower() != "t" && input.ToLower() != "n")
            {
                ConsoleEx.Write(ConsoleColor.Red, "Odpowiadaj tylko t/n. \n");
                result = ForBool(message, allowEmpty, boolValueIfEmptyAllowed);
            }

            if (input.ToLower() == "t")
            {
                result = true;
            }

            return result;
        }

        public static int ForInt(string message, bool allowEmpty = false, int intValueIfEmptyAllowed = -9999)
        {
            string input = ForString(message, allowEmpty, "");

            if (input == "" && allowEmpty)
            {
                return intValueIfEmptyAllowed;
            }

            if (!int.TryParse(input, out int result))
            {
                ConsoleEx.Write(ConsoleColor.Red, "Podana wartość nie jest liczbą całkowitą. \n");
                result = ForInt(message, allowEmpty, intValueIfEmptyAllowed);
            }

            return result;
        }

        public static decimal ForDecimal(string message, bool allowEmpty = false, decimal decimalValueIfEmptyAllowed = -9999M)
        {
            string input = ForString(message, allowEmpty, "");

            if (input == "" && allowEmpty)
            {
                return decimalValueIfEmptyAllowed;
            }

            if (!decimal.TryParse(input, out decimal result))
            {
                ConsoleEx.Write(ConsoleColor.Red, "Podana wartość nie jest liczbą dziesiętną. \n");
                result = ForDecimal(message, allowEmpty, decimalValueIfEmptyAllowed);
            }

            return result;
        }
    }
}
