using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharmacy.Model;

namespace Pharmacy
{
    class Ask
    {
        public static string ForString(string message, bool allowEmpty = false, string stringValueIfEmptyAllowed = "")
        {
            ConsoleEx.Write(ConsoleColor.Green, message);
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) && allowEmpty)
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

            if (string.IsNullOrWhiteSpace(input) && allowEmpty)
            {
                return datetimeValueIfEmptyAllowed ?? DateTime.MinValue;
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

            if (string.IsNullOrWhiteSpace(input) && allowEmpty)
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

            if (string.IsNullOrWhiteSpace(input) && allowEmpty)
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

            if (string.IsNullOrWhiteSpace(input) && allowEmpty)
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

        public static int ForMedicineId(string message)
        {
            bool exists;
            int id;
            int count = 0;

            do
            {
                id = ForInt(message);
                exists = Medicine.CheckIfExists(id);

                if (exists)
                {
                    //break;
                    return id;
                }

                ConsoleEx.Write(ConsoleColor.Red, "Lek o podanym ID nie istnieje\n");

                count++;

                if (count >= 2)
                {
                    if (Ask.ForBool("Wyświetlić wszystkie leki: t/n "))
                    {
                        Console.WriteLine();
                        ProgramLogic.DisplayMedicineList(Medicine.LoadAll());
                        Console.WriteLine();
                    }

                    count = 0;
                }
 
            } while (!exists);

            return id;
        }

        private static bool IsValidPesel(string pesel)
        {
            if (pesel.Length == 11 && pesel.All(char.IsDigit))
            {
                int[] wagi = new int[] { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
                int sum = 0;

                for (int i = 0; i < pesel.Length - 1; i++)
                {
                    sum += (int)char.GetNumericValue(pesel[i]) * wagi[i];
                }

                if ((int)char.GetNumericValue(pesel[10]) == 10 - sum % 10)
                {
                    return true;
                }
            }

            return false;
        }

        public static string ForPesel(string message)
        {
            string input = Ask.ForString(message, false, "");

            if (!IsValidPesel(input))
            {
                ConsoleEx.Write(ConsoleColor.Red, "Wprowadzony PESEL nie jest prawidłowy. \n");
                input = ForPesel(message);
            }

            return input;
        }

        public static int ForIntListItem(string message, int from, int to)
        {
            string input = ForString(message);

            if (!int.TryParse(input, out int result))
            {
                ConsoleEx.Write(ConsoleColor.Red, "Podana wartość nie jest liczbą całkowitą. \n");
                result = ForIntListItem(message, from, to);
            }

            if (result > to || result < from)
            {
                ConsoleEx.Write(ConsoleColor.Red, $"Wybrana wartość jest poza zakresem {from} - {to}. \n");
                result = ForIntListItem(message, from, to);
            }

            return result;
        }
    }
}
