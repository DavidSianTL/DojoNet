﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace UsuariosApi.Data
{
    public class DbConnection
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DbConnection(IConfiguration configuration) 
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");

        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);

        }

    }
}
