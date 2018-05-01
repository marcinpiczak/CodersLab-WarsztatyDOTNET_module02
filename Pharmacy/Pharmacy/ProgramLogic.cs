using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Pharmacy.Model;

namespace Pharmacy
{
    class ProgramLogic
    {
        public static void AddMedicine()
        {
            string name = Ask.ForString("Podaj nazwę leku: ".PadRight(25));
            string manufacturer = Ask.ForString("Podaj producenta leku: ".PadRight(25));
            decimal price = Ask.ForDecimal("Podaj cenę leku: ".PadRight(25));
            decimal amount = Ask.ForDecimal("Podaj ilość leku: ".PadRight(25));
            bool withPrescription = Ask.ForBool("Na receptę t/n: ".PadRight(25));

            try
            {
                using (var medicine = new Medicine(name, manufacturer, price, amount, withPrescription))
                {
                    medicine.Save();
                    ConsoleEx.WriteLine(Console.ForegroundColor, "Lek został dodany");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Wystąpił błąd. ERROR: {0}", e.Message);
            }
        }

        public static void ModifyMedicine()
        {
            int id = Ask.ForInt("Podaj Id leku: ".PadRight(25));

            try
            {
                using (var medicine = new Medicine())
                {
                    medicine.Reload(id);

                    ConsoleEx.WriteLine(Console.ForegroundColor, "Pozostaw puste jeżeli nie chcesz zmieniać: ");

                    string name = Ask.ForString("Podaj nazwę leku: ".PadRight(25), true, medicine.Name);
                    string manufacturer = Ask.ForString("Podaj producenta leku: ".PadRight(25), true, medicine.Manufacturer);
                    decimal price = Ask.ForDecimal("Podaj cenę leku: ".PadRight(25), true, medicine.Price);
                    decimal amount = Ask.ForDecimal("Podaj ilość leku: ".PadRight(25), true, medicine.Amount);
                    bool withPrescription = Ask.ForBool("Na receptę t/n: ".PadRight(25), true, medicine.WithPrescription);

                    Console.WriteLine();

                    var medicineToMod = new List<Medicine>()
                    {
                        medicine,
                        new Medicine(name, manufacturer, price, amount, withPrescription)
                    };

                    DisplayMedicineList(medicineToMod);
                    Console.WriteLine();

                    if (Ask.ForBool("Czy na pewno chcesz wprowadzić zmiany: t/n "))
                    {
                        medicine.Name = name;
                        medicine.Manufacturer = manufacturer;
                        medicine.Price = price;
                        medicine.Amount = amount;
                        medicine.WithPrescription = withPrescription;

                        medicine.Save();
                        ConsoleEx.WriteLine(Console.ForegroundColor, "Zmiany zostały wprowadzone. ");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Wystąpił błąd. ERROR: {0}", e.Message);
            }
        }

        public static void DeleteMedicine()
        {
            int id = Ask.ForInt("Podaj Id leku: ".PadRight(25));

            try
            {
                using (var medicine = new Medicine())
                {
                    //dodać sprawdzanie czy lek o podanym id istnieje
                    medicine.Reload(id);

                    var medicineToDelete = new List<Medicine>()
                    {
                        medicine
                    };

                    DisplayMedicineList(medicineToDelete);
                    Console.WriteLine();

                    if (Ask.ForBool("Czy na pewno chcesz usunąć lek: t/n "))
                    {
                        medicine.Remove();
                        ConsoleEx.WriteLine(Console.ForegroundColor, "Lek został usunięty. ");
                        Console.WriteLine();

                        //dodać sprawdzanie czy dla leku nie zostły stworzone jakieś zamówienia
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Wystąpił błąd. ERROR: {0}", e.Message);
            }
        }

        public static void DisplayMedicineList(List<Medicine> medicineList)
        {
            //var medicineLIst = Medicine.LoadAll()

            string header = string.Format("| {0} | {1} | {2} | {3} | {4} | {5} |",
                "ID".PadRight(6),
                "Nazwa".PadRight(15),
                "Producent".PadRight(15),
                "Cena".PadRight(8),
                "Ilość".PadRight(8),
                "Na".PadRight(8)
                );
            string header2 = string.Format("| {0} | {1} | {2} | {3} | {4} | {5} |",
                "".PadRight(6),
                "".PadRight(15),
                "".PadRight(15),
                "".PadRight(8),
                "".PadRight(8),
                "receptę?".PadRight(8)
            );

            Console.WriteLine("".PadRight(header.Length, '-'));
            Console.WriteLine(header + "\n" + header2);
            Console.WriteLine("".PadRight(header.Length, '-'));

            foreach (var medicine in medicineList)
            {
                Console.WriteLine("| {0} | {1} | {2} | {3} | {4} | {5} |",
                    medicine.ID.ToString().PadRight(6),
                    medicine.Name.PadRight(15),
                    medicine.Manufacturer.PadRight(15),
                    Math.Round(medicine.Price, 2).ToString().PadLeft(8),
                    Math.Round(medicine.Amount, 2).ToString().PadLeft(8),
                    medicine.WithPrescription ? "T".PadLeft(5).PadRight(8) : "N".PadLeft(5).PadRight(8)
                    );
            }
            Console.WriteLine("".PadRight(header.Length, '-'));
        }

    }
}
