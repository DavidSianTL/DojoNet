using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace wbEmpresaDB.Data
{
    public class cnnConexionAsync
    {
        private readonly string _connectionString;

        public cnnConexionAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
