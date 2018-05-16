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
                ConsoleEx.Write(Console.ForegroundColor, "      1. Dodanie leku - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "add");
                ConsoleEx.Write(Console.ForegroundColor, "      2. Edycja leku - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "mod");
                ConsoleEx.Write(Console.ForegroundColor, "      3. Usuwanie leku - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "del");
                ConsoleEx.Write(Console.ForegroundColor, "      4. Wyświetlenie listy leków - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "show");
                ConsoleEx.Write(Console.ForegroundColor, "      5. Wyszukiwanie leków - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "find");
                ConsoleEx.WriteLine(Console.ForegroundColor, "II.  Sprzedaż leków: ");
                ConsoleEx.Write(Console.ForegroundColor, "      1. Dodanie zamówienia - ".PadRight(40)); ConsoleEx.WriteLine(ConsoleColor.Green, "ord");
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
                        addNext = Ask.ForBool("Dodać kolejny lek t/n: ");
                        Console.WriteLine();

                    } while (addNext);
                }

                if (input == "mod")
                {
                    DisplayHeader(ConsoleColor.Green, "MODYFIKACJA LEKU");
                    bool modNext;
                    do
                    {
                        ProgramLogic.ModifyMedicine();

                        Console.WriteLine();
                        modNext = Ask.ForBool("Czy zmieniany będzie kolejny lek t/n: ");
                        Console.WriteLine();

                    } while (modNext);
                }

                if (input == "show")
                {
                    DisplayHeader(ConsoleColor.Green, "LISTA WSZYSTKICH LEKÓW");

                    ProgramLogic.DisplayMedicineList(Medicine.LoadAll());
                    Console.WriteLine();
                }

                if (input == "del")
                {
                    DisplayHeader(ConsoleColor.Green, "USUWANIE LEKU");
                    bool delNext;
                    do
                    {
                        ProgramLogic.DeleteMedicine();

                        Console.WriteLine();
                        delNext = Ask.ForBool("Czy usuwany będzie kolejny lek t/n: ");
                        Console.WriteLine();

                    } while (delNext);
                }

                if (input == "ord")
                {
                    DisplayHeader(ConsoleColor.Green, "SKŁADANIE ZAMÓWIENIA");
                    bool ordNext;
                    do
                    {
                        ProgramLogic.AddOrder();

                        Console.WriteLine();
                        ordNext = Ask.ForBool("Czy dodać kolejne zamówienie t/n: ");
                        Console.WriteLine();

                    } while (ordNext);
                }

                if (input == "find")
                {
                    DisplayHeader(ConsoleColor.Green, "WYSZUKIWANIE LEKU");
                    bool findNext;
                    do
                    {
                        ProgramLogic.SearchForMedicine();

                        Console.WriteLine();
                        findNext = Ask.ForBool("Czy wyszukać kolejny lek t/n: ");
                        Console.WriteLine();

                    } while (findNext);
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
