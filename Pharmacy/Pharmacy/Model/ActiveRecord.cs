using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Pharmacy.Model
{
    public abstract class ActiveRecord : IDisposable
    {
        public int ID { get; protected set; }

        private const string _connectionString = "Integrated Security=SSPI;" +
                                                "Initial Catalog=Pharmacy;" +
                                                "Data Source=.\\SQLEXPRESS;";

        private static readonly SqlConnection _conn = new SqlConnection(_connectionString);

        public abstract void Save();
        public abstract void Reload(int id = 0);
        public abstract void Remove();

        protected static void Open()
        {
            _conn.Open();
        }

        protected static void Close()
        {
            _conn.Close();
        }

        public void Dispose()
        {
            Close();
        }

        protected static SqlCommand GetCommand(string query)
        {
            return new SqlCommand(query, _conn);
        }

        protected string GetInsertStatement()
        {
            var columns = string.Join(", ", GetType().GetProperties().Where(p => p.Name.ToString() != "ID").Select(p => p.Name.ToString()).ToArray());
            var param = string.Join(", ",
                GetType().GetProperties().Where(p => p.Name.ToString() != "ID").Select(p => "@" + p.Name.ToString().ToLower()).ToArray());
            var table = GetType().Name;

            return $"insert into {table}s ({columns}) values ({param}); select scope_identity();";
        }

        protected string GetUpdateStatement()
        {
            var set = string.Join(", ", GetType().GetProperties().Where(p => p.Name.ToString() != "ID").Select(p => p.Name.ToString() + " = @"+ p.Name.ToString().ToLower()).ToArray());
            var table = GetType().Name;


            return $"update {table}s set {set} where ID = @id";
        }

        protected string GetSelectStatement()
        {
            var columns = string.Join(", ", GetType().GetProperties().Select(p => p.Name.ToString()).ToArray());
            var table = GetType().Name;

            return $"select TOP 1 {columns} from {table}s where ID = @id";
        }

        protected string GetSelectAllStatement()
        {
            var columns = string.Join(", ", GetType().GetProperties().Select(p => p.Name.ToString()).ToArray());
            var table = GetType().Name;

            return $"select {columns} from {table}s";
        }

        protected string GetDeleteStatement()
        {
            var table = GetType().Name;

            return $"delete from {table}s where ID = @id";
        }

        
        public string RecordToString()
        {
            var properties = new StringBuilder();

            foreach (var propertyInfo in GetType().GetProperties())
            {
                properties.Append($", {propertyInfo.Name} = {propertyInfo.GetValue(this)}");
            }

            return properties.ToString();
        }

    }
}
