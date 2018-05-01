using System;
using Pharmacy.Model;

namespace Pharmacy
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                ConsoleEx.WriteLine(Console.ForegroundColor, "I.  Zarządzania listą leków: ");
                ConsoleEx.WriteLine(Console.ForegroundColor, "      1. Dodanie leku -               add");
                ConsoleEx.WriteLine(Console.ForegroundColor, "      2. Edycja leku -                mod");
                ConsoleEx.WriteLine(Console.ForegroundColor, "      3. Usuwanie leku -              del");
                ConsoleEx.WriteLine(Console.ForegroundColor, "      4. Wyświetlenie listy leków -   disp");
                ConsoleEx.WriteLine(Console.ForegroundColor, "II.  Sprzedaż leków: ");
                ConsoleEx.WriteLine(Console.ForegroundColor, "      1. Dodanie zamówienia -         ord");
                ConsoleEx.WriteLine(Console.ForegroundColor, "\nWyjście - exit ");
                Console.WriteLine();

                string input = Ask.ForString("Wpisz polecenie: ").ToLower();

                if (input == "exit")
                {
                    break;
                }

                if (input == "add")
                {
                    DisplayHeader(ConsoleColor.Green, "DODWANIE LEKU");
                    bool addNext;
                    do
                    {
                        ProgramLogic.AddMedicine();

                        Console.WriteLine();
                        addNext = Ask.ForBool("Dodać kolejny lek t/n: ".PadRight(25));
                        Console.WriteLine();

                    } while (addNext);
                }

                if (input == "mod")
                {
                    ProgramLogic.ModifyMedicine();
                }

                if (input == "disp")
                {
                    ProgramLogic.DisplayMedicineList(Medicine.LoadAll());
                    Console.WriteLine();
                }

                if (input == "del")
                {
                    ProgramLogic.DeleteMedicine();
                }

            }

            void DisplayHeader(ConsoleColor color, string header)
            {
                ConsoleEx.WriteLine(color, "".PadLeft(header.Length + 4, '-'));
                ConsoleEx.WriteLine(color, $"--{header}--");
                ConsoleEx.WriteLine(color, "".PadLeft(header.Length + 4, '-'));
            }
        }
    }
}
