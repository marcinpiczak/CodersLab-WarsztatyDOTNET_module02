using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Pharmacy.Model
{
    internal class Medicine : ActiveRecord
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public bool WithPrescription { get; set; }
        //--------------------------------------\\
        private string _name { get; set; }
        private string _manufacturer { get; set; }
        private decimal _price { get; set; }
        private decimal _amount { get; set; }
        private bool _withPrescription { get; set; }

        public override void Save()
        {
            string insert = "insert into Medicines (Name, Manufacturer, Price, Amount, WithPrescription) " +
                            "values (@name, @manufacturer, @price, @amount, @withprescription); select scope_identity(); ";
            insert = GetInsertStatement();
            string update = "update Medicines set " +
                            "Name = @name ,Manufacturer = @manufacturer ,Price = @price ,Amount = @amount ,WithPrescription = @withprescription " +
                            "where ID = @id";
            update = GetUpdateStatement();

            
            using (var command = GetCommand(insert))
            {
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@name", DbType.String){Value = Name},
                    new SqlParameter("@manufacturer", DbType.String){Value = Manufacturer},
                    new SqlParameter("@price", DbType.Decimal){Value = Price},
                    new SqlParameter("@amount", DbType.Decimal){Value = Amount},
                    new SqlParameter("@withprescription", DbType.Boolean){Value = WithPrescription}
                });

                Open();

                if (ID == 0)
                {
                    //command.CommandText = insert;
                    ID = Convert.ToInt32(command.ExecuteScalar());
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("@id", DbType.Int32) { Value = ID });
                    command.CommandText = update;
                    command.ExecuteNonQuery();
                }

                Close();
            }
            
        }

        public override void Reload(int id = 0)
        {
            string select = "select top 1 ID, Name, Manufacturer, Price, Amount, WithPrescription from Medicines where ID = @id";
            select = GetSelectStatement();

            Open();
            using (var command = GetCommand(select))
            {
                command.Parameters.Add(new SqlParameter("@id", DbType.Int32) { Value = id != 0 ? id : ID });

                command.CommandText = select;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //ID = Convert.ToInt32(reader["ID"]);
                        ID = id != 0 ? id : ID;
                        Name = reader["Name"].ToString();
                        Manufacturer = reader["Manufacturer"].ToString();
                        Price = Convert.ToDecimal(reader["Price"]);
                        Amount = Convert.ToDecimal(reader["Amount"]);
                        WithPrescription = Convert.ToBoolean(reader["WithPrescription"]);
                        //--------------------------------------\\
                        _name = reader["Name"].ToString();
                        _manufacturer = reader["Manufacturer"].ToString();
                        _price = Convert.ToDecimal(reader["Price"]);
                        _amount = Convert.ToDecimal(reader["Amount"]);
                        _withPrescription = Convert.ToBoolean(reader["WithPrescription"]);
                    }
                }
            }
            Close();

            //sprawdzic mozliwosc zrobienia na return this
        }

        public override void Remove()
        {
            string delete = "delete from Medicines where ID = @id";
            delete = GetDeleteStatement();

            Open();
            using (var command = GetCommand(delete))
            {
                command.Parameters.Add(new SqlParameter("@id", DbType.Int32) { Value = ID });
                command.CommandText = delete;
                command.ExecuteNonQuery();

            }
            Close();


        }

        public Medicine(string name, string manufacturer, decimal price, decimal amount, bool withPrescription)
        {
            Name = name;
            Manufacturer = manufacturer;
            Price = price;
            Amount = amount;
            WithPrescription = withPrescription;
        }

        public Medicine()
        {
        }

        public static List<Medicine> LoadAll()
        {
            var medList = new List<Medicine>();
            string selectAll = "select ID, Name, Manufacturer, Price, Amount, WithPrescription from Medicines";

            using (var command = GetCommand(selectAll))
            {
                Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        medList.Add(new Medicine()
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Name = reader["Name"].ToString(),
                                Manufacturer = reader["Manufacturer"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Amount = Convert.ToDecimal(reader["Amount"]),
                                WithPrescription = Convert.ToBoolean(reader["WithPrescription"]),
                            }
                        );
                    }
                }
                Close();
            }
            
            return medList;
        }
        /*
        public override string ToString()
        {
            var properties = new StringBuilder();
            
            foreach (var propertyInfo in GetType().GetProperties())
            {
                properties.Append($", {propertyInfo.Name} = {GetType().GetProperty(propertyInfo.Name).GetValue(this)}");
            }
            
            var text = string.Join(", ", GetType().GetProperties().Select(p => p.Name.ToString() + " = {1}").ToArray());
            var param = GetType().GetProperties();
            
            //return string.Format("ID = {0}, Name = {1}, Manufacturer = {2}, Price = {3}, Amount = {4}, WithPrescription = {5}", ID, Name, Manufacturer, Price, Amount, WithPrescription);
            //zbudować od razu całego stringa z wartościami
            //return string.Format(text, param);
            return properties.ToString();
        }
        */
    }
}
