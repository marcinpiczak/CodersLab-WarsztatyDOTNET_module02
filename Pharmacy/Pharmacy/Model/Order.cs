using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Pharmacy.Model
{
    internal class Order : ActiveRecord
    {
        public int? PrescriptionID { get; set; }
        public int MedicineID { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        private SqlParameter[] GetSqlParameters(SqlParamtersType type, int id = 0)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@prescriptionid", DbType.Int32) {Value = (object) PrescriptionID ?? DBNull.Value},
                new SqlParameter("@medicineid", DbType.Int32) {Value = MedicineID},
                new SqlParameter("@date", DbType.DateTime) {Value = Date},
                new SqlParameter("@amount", DbType.Decimal) {Value = Amount},
            };

            if (type == SqlParamtersType.AllWithIdValue)
            {
                sqlParameters.Add(new SqlParameter("@id", DbType.Int32) {Value = id});
                return sqlParameters.ToArray();
            }

            if (type == SqlParamtersType.OnlyIdWithValue)
            {
                return new SqlParameter[] { new SqlParameter("@id", DbType.Int32) { Value = id } };
            }

            //SqlParamtersType.AllWithoutId;
            return sqlParameters.ToArray();
        }

        private Order GetReaderSelectMaping(SqlDataReader reader)
        {
            ID = Convert.ToInt32(reader["ID"]);
            //PrescriptionID = Convert.ToInt32(reader["PrescriptionID"]);
            PrescriptionID = reader.IsDBNull(reader.GetOrdinal("PrescriptionID")) ? (int?)null : Convert.ToInt32(reader["PrescriptionID"]);
            MedicineID = Convert.ToInt32(reader["MedicineID"]);
            Date = Convert.ToDateTime(reader["Date"]);
            Amount = Convert.ToDecimal(reader["Amount"]);

            return this;
        }
        
        public override void Save()
        {
            string insert = "insert into Orders (PrescriptionID, MedicineID, Date, Amount) " +
                                  "values (@prescriptionid, @medicineid, @date, @amount); select scope_identity()";
            string update = "update Orders set PrescriptionID = @prescriptionid ,MedicineID = @medicineid ,Date = @date ,Amount = @amount " +
                            "where ID = @id";
            
            using (var command = GetCommand(insert))
            {
                command.Parameters.AddRange(GetSqlParameters(SqlParamtersType.AllWithoutId));

                Open();

                if (ID == 0)
                {
                    ID = Convert.ToInt32(command.ExecuteScalar());
                }
                else
                {
                    command.Parameters.AddRange(GetSqlParameters(SqlParamtersType.OnlyIdWithValue, ID));
                    command.CommandText = update;
                    command.ExecuteNonQuery();
                }

                Close();
            }
        }

        public override void Reload(int id = 0)
        {
            string select = "select top 1 ID, PrescriptionID, MedicineID, Date, Amount from Orders where ID = @id";

            using (var command = GetCommand(select))
            {
                command.Parameters.AddRange(GetSqlParameters(SqlParamtersType.OnlyIdWithValue, id != 0 ? id : ID));

                Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GetReaderSelectMaping(reader);
                    }
                }

                Close();
            }
        }

        public override void Remove()
        {
            string delete = "delete from Orders where ID = @id";

            using (var command = GetCommand(delete))
            {
                command.Parameters.AddRange(GetSqlParameters(SqlParamtersType.OnlyIdWithValue, ID));

                Open();

                command.ExecuteNonQuery();

                Close();
            }
        }

        public Order(int? prescriptionID, int medicineID, DateTime date, decimal amount)
        {
            PrescriptionID = prescriptionID;
            MedicineID = medicineID;
            Date = date;
            Amount = amount;
        }

        public Order()
        {

        }

        public static List<Order> LoadAll()
        {
            var orderList = new List<Order>();

            string selectAll = "select ID, PrescriptionID, MedicineID, Date, Amount from Orders";

            using (var command = GetCommand(selectAll))
            {

                Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orderList.Add(new Order().GetReaderSelectMaping(reader));
                    }
                }

                Close();
            }

            return orderList;
        }
    }
}
