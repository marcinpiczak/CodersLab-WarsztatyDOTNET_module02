using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pharmacy.Model
{
    internal class Prescription : ActiveRecord
    {
        public string CustomerName { get; set; }
        public string PESEL { get; set; }
        public string PrescriptionNumber { get; set; }

        private SqlParameter[] GetSqlParameters(SqlParamtersType type, int id = 0)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@customername", DbType.String) {Value = CustomerName},
                new SqlParameter("@pesel", DbType.String) {Value = PESEL},
                new SqlParameter("@prescriptionnumber", DbType.String) {Value = PrescriptionNumber},
            };

            if (type == SqlParamtersType.AllWithIdValue)
            {
                sqlParameters.Add(new SqlParameter("@id", DbType.Int32) { Value = id });
                return sqlParameters.ToArray();
            }

            if (type == SqlParamtersType.OnlyIdWithValue)
            {
                return new SqlParameter[] { new SqlParameter("@id", DbType.Int32) { Value = id } };
            }

            //SqlParamtersType.AllWithoutId;
            return sqlParameters.ToArray();
        }

        private Prescription GetReaderSelectMaping(SqlDataReader reader)
        {
            ID = Convert.ToInt32(reader["ID"]);
            CustomerName = reader["CustomerName"].ToString();
            PESEL = reader["PESEL"].ToString();
            PrescriptionNumber = reader["PrescriptionNumber"].ToString();

            return this;
        }
        
        public override void Save()
        {
            string insert = "insert into Prescriptions (CustomerName, PESEL, PrescriptionNumber) " +
                                  "values (@customername, @pesel, @prescriptionnumber); select scope_identity()";
            string update = "update Prescriptions set CustomerName = @customername ,PESEL = @pesel ,PrescriptionNumber = @prescriptionnumber " +
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
            string select = "select top 1 ID, CustomerName, PESEL, PrescriptionNumber from Prescriptions where ID = @id";

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
            string delete = "delete from Prescriptions where ID = @id";

            using (var command = GetCommand(delete))
            {
                command.Parameters.AddRange(GetSqlParameters(SqlParamtersType.OnlyIdWithValue, ID));

                Open();

                command.ExecuteNonQuery();

                Close();
            }
        }

        public Prescription(string customerName, string pesel, string prescriptionNumber)
        {
            CustomerName = customerName;
            PESEL = pesel;
            PrescriptionNumber = prescriptionNumber;
        }

        public Prescription()
        {

        }

        //public List<Prescription> LoadAll2()
        //{
        //    var prescList = new List<Prescription>();

        //    string selectAll = "select ID, CustomerName, PESEL, PrescriptionNumber from Prescriptions";

        //    using (var command = GetCommand(selectAll))
        //    {
        //        Open();

        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                prescList.Add(new Prescription().GetReaderSelectMaping(reader));
        //            }
        //        }

        //        Close();
        //    }

        //    return prescList;
        //}

        public static List<Prescription> LoadAll()
        {
            var prescList = new List<Prescription>();

            string selectAll = "select ID, CustomerName, PESEL, PrescriptionNumber from Prescriptions";

            using (var command = GetCommand(selectAll))
            {

                Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prescList.Add(new Prescription().GetReaderSelectMaping(reader));
                    }
                }

                Close();
            }

            return prescList;
        }
    }
}
