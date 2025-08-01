using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoProyectoWSAsync
    {
        private readonly string _connectionString;

        public daoProyectoWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Obtener todos los proyectos
        public async Task<List<ProyectoViewModel>> ObtenerProyectosAsync()
        {
            var proyectos = new List<ProyectoViewModel>();
            string procedure = "sp_ListarProyectos";

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
                            proyectos.Add(new ProyectoViewModel
                            {
                                IdProyecto = reader.GetInt32(reader.GetOrdinal("IdProyecto")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                                FechaInicio = reader.IsDBNull(reader.GetOrdinal("FechaInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                            });
                        }
                    }
                }
            }
            return proyectos;
        }

        // Obtener proyecto por ID
        public async Task<ProyectoViewModel?> ObtenerProyectoPorIdAsync(int id)
        {
            string procedure = "sp_ListarProyectoId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdProyecto", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new ProyectoViewModel
                            {
                                IdProyecto = reader.GetInt32(reader.GetOrdinal("IdProyecto")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                                FechaInicio = reader.IsDBNull(reader.GetOrdinal("FechaInicio")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Insertar proyecto
        public async Task<int> InsertarProyectoAsync(ProyectoViewModel proyecto)
        {
            var parametros = new[]
            {
                new SqlParameter("@Nombre", proyecto.Nombre),
                new SqlParameter("@Descripcion", proyecto.Descripcion ?? (object)DBNull.Value),
                new SqlParameter("@FechaInicio", proyecto.FechaInicio),
                new SqlParameter("@FK_IdEstado", proyecto.FK_IdEstado)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_InsertarProyecto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Actualizar proyecto
        public async Task<int> ActualizarProyectoAsync(ProyectoViewModel proyecto)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdProyecto", proyecto.IdProyecto),
                new SqlParameter("@Nombre", proyecto.Nombre),
                new SqlParameter("@Descripcion", proyecto.Descripcion ?? (object)DBNull.Value),
                new SqlParameter("@FechaInicio", proyecto.FechaInicio),
                new SqlParameter("@FK_IdEstado", proyecto.FK_IdEstado)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarProyecto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Eliminar proyecto (soft delete)
        public async Task<int> EliminarProyectoAsync(int id)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdProyecto", id)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_EliminarProyecto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}