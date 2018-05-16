using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Pharmacy.Model;

namespace Pharmacy
{
    class ProgramLogic
    {
        public static void AddMedicine()
        {
            string name = Ask.ForString("Podaj nazwę leku: ");
            string manufacturer = Ask.ForString("Podaj producenta leku: ");
            decimal price = Ask.ForDecimal("Podaj cenę leku: ");
            decimal amount = Ask.ForDecimal("Podaj ilość leku: ");
            bool withPrescription = Ask.ForBool("Na receptę t/n: ");

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
                ConsoleEx.WriteLine(ConsoleColor.Red, "Wystąpił błąd. ERROR: {0}", e.Message);
            }
        }

        public static void ModifyMedicine()
        {
            try
            {
                int id = Ask.ForMedicineId("Podaj ID leku: ");

                using (var medicine = new Medicine())
                {
                    medicine.Reload(id);

                    ConsoleEx.WriteLine(Console.ForegroundColor, "Pozostaw puste jeżeli nie chcesz zmieniać: ");

                    string name = Ask.ForString("Podaj nazwę leku: ", true, medicine.Name);
                    string manufacturer = Ask.ForString("Podaj producenta leku: ", true, medicine.Manufacturer);
                    decimal price = Ask.ForDecimal("Podaj cenę leku: ", true, medicine.Price);
                    decimal amount = Ask.ForDecimal("Podaj ilość leku: ", true, medicine.Amount);
                    bool withPrescription = Ask.ForBool("Na receptę t/n: ", true, medicine.WithPrescription);

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
                ConsoleEx.WriteLine(ConsoleColor.Red, "Wystąpił błąd. ERROR: {0}", e.Message);
            }
        }

        public static void DeleteMedicine()
        {
            try
            {
                int id = Ask.ForMedicineId("Podaj ID leku: ");

                using (var medicine = new Medicine())
                {
                    medicine.Reload(id);

                    if (medicine.OrderExistsForMedicine())
                    {
                        ConsoleEx.WriteLine(ConsoleColor.Red, "Lek nie może zostać usunięty ponieważ istnieją dla niego zamówienia. ");
                    }
                    else
                    {
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
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ConsoleEx.WriteLine(ConsoleColor.Red, "Wystąpił błąd. ERROR: {0}", e.Message);
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

        public static void AddOrder()
        {
            try
            {
                int id = Ask.ForMedicineId("Podaj ID leku do sprzedaży: ");
                DateTime saleDate = Ask.ForDate("Podaj datę sprzedaży bądź pozostaw puste aby uzupełnić bieżącą datą: ", true, DateTime.Now);
                decimal amount = Ask.ForDecimal("Podaj ilość sprzedawanych sztuk: ");

                using (var medicine = new Medicine())
                {
                    medicine.Reload(id);

                    while (medicine.Amount < amount)
                    {
                        ConsoleEx.WriteLine(ConsoleColor.Red, "Wprowadzona ilość sztuk przekracza ilość w magazynie. ");
                        if (Ask.ForBool($"Czy ustawić ilość na maksymalną dostepną ilość {medicine.Amount} t/n: "))
                        {
                            amount = medicine.Amount;
                        }
                        else
                        {
                            amount = Ask.ForDecimal("Podaj nową ilość sprzedawanych sztuk: ");
                        }
                    }

                    int? prescriptioId = null;
                    Prescription prescription = null;

                    if (medicine.WithPrescription)
                    {
                        string customerName = Ask.ForString("Podaj imię i nazwisko: ");
                        string pesel = Ask.ForPesel("Podaje PESEL: ");
                        string prescriptionNumber = Ask.ForString("Podaj numer recepty: ");

                        prescription = new Prescription(customerName, pesel, prescriptionNumber);
                    }

                    if (Ask.ForBool($"Czy zapisać wprowadzone zamówienie t/n: "))
                    {
                        medicine.Amount -= amount;
                        medicine.Save();

                        if (medicine.WithPrescription && prescription != null)
                        {
                            using (prescription)
                            {
                                prescription.Save();
                                prescriptioId = prescription.ID;
                            }
                        }
                        
                        using (var order = new Order(prescriptioId, id, saleDate, amount))
                        {
                            order.Save();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ConsoleEx.WriteLine(ConsoleColor.Red, "Wystąpił błąd. ERROR: {0}", e.Message);
            }
        }

        public static void SearchForMedicine()
        {
            //To do:
            //0. działa, ale do zmiany,
            //1. dodać zależność między typem a operatorem,
            //2. dodać dynamiczne generowanie poszczególnych list wyborów,
            //3. dodać mozliwość wyboru filtrowania po więcej niż jednym polu

            Console.WriteLine("Pola dostępne dla leków: ");
            Console.WriteLine(" 1. ID");
            Console.WriteLine(" 2. Nazwa");
            Console.WriteLine(" 3. Producent");
            Console.WriteLine(" 4. Cena");
            Console.WriteLine(" 5. Ilosc");
            Console.WriteLine(" 6. Na receptę");
            int fieldNum = Ask.ForIntListItem("Wybierz pole: ", 1, 6);

            Console.WriteLine();

            Console.WriteLine("Operatory dostępne podczas wyszukiwania: ");
            Console.WriteLine(" 1. Równy");
            Console.WriteLine(" 2. Różny");
            Console.WriteLine(" 3. Zawiera");
            Console.WriteLine(" 4. Zakres/Pomiędzy");
            Console.WriteLine(" 5. Większy");
            Console.WriteLine(" 6. Większy równy");
            Console.WriteLine(" 7. Mniejszy");
            Console.WriteLine(" 8. Mniejszy równy");
            int conditionNum = Ask.ForIntListItem("Wybierz operator: ", 1, 8);

            string selectedField = "";
            string selectedOperator = "";
            object givenValue;
            object givenValue2;
            var sqlParam = new List<SqlParameter>();
            

            switch (fieldNum)
            {
                default:
                    selectedField = "ID";
                    if (conditionNum == 4)
                    {
                        givenValue = Ask.ForInt("Podaj wartość od: ");
                        givenValue2 = Ask.ForInt("Podaj wartość do: ");

                        sqlParam.Add(new SqlParameter()
                        {
                            ParameterName = "@" + selectedField.ToLower() + "To",
                            Value = givenValue2
                        });

                        break;
                    }
                    givenValue = Ask.ForInt("Podaj wartość: ");
                    break;
                case 2:
                    selectedField = "Name";
                    if (conditionNum == 4)
                    {
                        givenValue = Ask.ForString("Podaj wartość od: ");
                        givenValue2 = Ask.ForString("Podaj wartość do: ");

                        sqlParam.Add(new SqlParameter()
                        {
                            ParameterName = "@" + selectedField.ToLower() + "To",
                            Value = givenValue2
                        });
                        break;
                    }
                    givenValue = Ask.ForString("Podaj wartość: ");
                    break;
                case 3:
                    selectedField = "Manufacturer";
                    if (conditionNum == 4)
                    {
                        givenValue = Ask.ForString("Podaj wartość od: ");
                        givenValue2 = Ask.ForString("Podaj wartość do: ");

                        sqlParam.Add(new SqlParameter()
                        {
                            ParameterName = "@" + selectedField.ToLower() + "To",
                            Value = givenValue2
                        });
                        break;
                    }
                    givenValue = Ask.ForString("Podaj wartość: ");
                    break;
                case 4:
                    selectedField = "Price";
                    if (conditionNum == 4)
                    {
                        givenValue = Ask.ForDecimal("Podaj wartość od: ");
                        givenValue2 = Ask.ForDecimal("Podaj wartość do: ");

                        sqlParam.Add(new SqlParameter()
                        {
                            ParameterName = "@" + selectedField.ToLower() + "To",
                            Value = givenValue2
                        });
                        break;
                    }
                    givenValue = Ask.ForDecimal("Podaj wartość: ");
                    break;
                case 5:
                    selectedField = "Amount";
                    if (conditionNum == 4)
                    {
                        givenValue = Ask.ForString("Podaj wartość od: ");
                        givenValue2 = Ask.ForString("Podaj wartość do: ");

                        sqlParam.Add(new SqlParameter()
                        {
                            ParameterName = "@" + selectedField.ToLower() + "To",
                            Value = givenValue2
                        });
                        break;
                    }
                    givenValue = Ask.ForString("Podaj wartość: ");
                    break;
                case 6:
                    selectedField = "WithPrescription";
                    if (conditionNum == 4)
                    {
                        givenValue = Ask.ForBool("Podaj wartość od: ");
                        givenValue2 = Ask.ForBool("Podaj wartość do: ");

                        sqlParam.Add(new SqlParameter()
                        {
                            ParameterName = "@" + selectedField.ToLower() + "To",
                            Value = givenValue2
                        });
                        break;
                    }
                    givenValue = Ask.ForBool("Podaj wartość: ");
                    break;
            }

            switch (conditionNum)
            {
                default:
                    selectedOperator = " = #field#";
                    break;
                case 2:
                    selectedOperator = " <> #field#";
                    break;
                case 3:
                    selectedOperator = " like '%'+#field#+'%'";
                    break;
                case 4:
                    selectedOperator = " between #field# and #field2#";
                    break;
                case 5:
                    selectedOperator = " > #field#";
                    break;
                case 6:
                    selectedOperator = " >= #field#";
                    break;
                case 7:
                    selectedOperator = " < #field#";
                    break;
                case 8:
                    selectedOperator = " <= #field#";
                    break;
            }

            sqlParam.Add(new SqlParameter()
            {
                ParameterName = "@" + selectedField.ToLower(),
                Value = givenValue
            });

            string whereClausule = " where " + selectedField + selectedOperator.Replace("#field#", "@" + selectedField.ToLower()).Replace("#field2#", "@" + selectedField.ToLower() + "To");

            DisplayMedicineList(Medicine.Search(whereClausule, sqlParam.ToArray()));
        }
    }
}
