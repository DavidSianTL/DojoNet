using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoSistemaWSAsync
    {
        private readonly string _connectionString;

        public daoSistemaWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<SistemaViewModel>> ObtenerSistemasAsync()
        {
            var sistemas = new List<SistemaViewModel>();
            string procedure = "sp_ListarSistemas";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            sistemas.Add(new SistemaViewModel
                            {
                                IdSistema = reader.GetInt32(reader.GetOrdinal("IdSistema")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                                FK_IdEmpresa = reader.GetInt32(reader.GetOrdinal("FK_IdEmpresa")),
                                Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
                            });
                        }
                    }
                }
            }

            return sistemas;
        }

        public async Task<SistemaViewModel> ObtenerSistemaPorIdAsync(int Id)
        {
            string procedure = "sp_ListarSistemaId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdSistema", Id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new SistemaViewModel
                            {
                                IdSistema = reader.GetInt32(reader.GetOrdinal("IdSistema")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                                FK_IdEmpresa = reader.GetInt32(reader.GetOrdinal("FK_IdEmpresa")),
                                Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<int> InsertarSistemaAsync(SistemaViewModel sistema)
        {
            var parametros = new[]
            {
                new SqlParameter("@Nombre", sistema.Nombre),
                new SqlParameter("@Descripcion", sistema.Descripcion),
                new SqlParameter("@Codigo", sistema.Codigo),
                new SqlParameter("@FK_IdEmpresa", sistema.FK_IdEmpresa)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_InsertarSistema", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> ActualizarSistemaAsync(SistemaViewModel sistema)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdSistema", sistema.IdSistema),
                new SqlParameter("@Nombre", sistema.Nombre),
                new SqlParameter("@Descripcion", sistema.Descripcion),
                new SqlParameter("@Codigo", sistema.Codigo),
                new SqlParameter("@FK_IdEmpresa", sistema.FK_IdEmpresa),
                new SqlParameter("@Estado", sistema.Estado)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarSistema", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> EliminarSistemaAsync(int Id)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdSistema", Id)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_EliminarSistema", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

