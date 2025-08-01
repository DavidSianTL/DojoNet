using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoEmpresaWSAsync
    {
        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoEmpresaWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de empresas
        public async Task<List<EmpresaViewModel>> ObtenerEmpresasAsync()
        {
            var empresas = new List<EmpresaViewModel>();
            string procedure = "sp_ListarEmpresas";

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
                            empresas.Add(new EmpresaViewModel
                            {
                                IdEmpresa = reader.GetInt32(reader.GetOrdinal("IdEmpresa")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                                Logo = reader.IsDBNull(reader.GetOrdinal("Logo")) ? "" : reader.GetString(reader.GetOrdinal("Logo")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
                            });
                        }
                    }
                }
            }

            return empresas;
        }

        // Método para obtener una empresa por su ID
        public async Task<EmpresaViewModel> ObtenerEmpresaPorIdAsync(int Id)
        {
            string procedure = "sp_ListarEmpresaId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdEmpresa", Id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new EmpresaViewModel
                            {
                                IdEmpresa = reader.GetInt32(reader.GetOrdinal("IdEmpresa")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                                Logo = reader.IsDBNull(reader.GetOrdinal("Logo")) ? "" : reader.GetString(reader.GetOrdinal("Logo")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        // Método para insertar una nueva empresa
        public async Task<int> InsertarEmpresaAsync(EmpresaViewModel empresa)
        {
            var parametros = new[]
            {
                new SqlParameter("@Nombre", empresa.Nombre),
                new SqlParameter("@Descripcion", empresa.Descripcion ?? ""),
                new SqlParameter("@Codigo", empresa.Codigo),
                new SqlParameter("@Logo", empresa.Logo),
                new SqlParameter("@FK_IdEstado", empresa.FK_IdEstado)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_InsertarEmpresa", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para actualizar una empresa existente
        public async Task<int> ActualizarEmpresaAsync(EmpresaViewModel empresa)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdEmpresa", empresa.IdEmpresa),
                new SqlParameter("@Nombre", empresa.Nombre),
                new SqlParameter("@Descripcion", empresa.Descripcion),
                new SqlParameter("@Codigo", empresa.Codigo),
                new SqlParameter("@Logo", empresa.Logo),
                new SqlParameter("@FK_IdEstado", empresa.FK_IdEstado)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarEmpresa", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para eliminar (cambiar su estado) una empresa por su ID
        public async Task<int> EliminarEmpresaAsync(int Id)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdEmpresa", Id)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_EliminarEmpresa", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

