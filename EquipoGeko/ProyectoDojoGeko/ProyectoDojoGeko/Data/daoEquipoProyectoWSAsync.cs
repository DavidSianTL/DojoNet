using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models;
using System.Data;

namespace ProyectoDojoGeko.Data
{
    public class daoProyectoEquipoWSAsync
    {
        private readonly string _connectionString;

        public daoProyectoEquipoWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Listar todos los proyectos
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


        // Insertar un proyecto
        public async Task<int> InsertarProyectoAsync(ProyectoViewModel proyecto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_InsertarProyecto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombre", proyecto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", proyecto.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaInicio", proyecto.FechaInicio);
                    cmd.Parameters.AddWithValue("@FK_IdEstado", proyecto.FK_IdEstado);

                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<ProyectoViewModel?> ObtenerProyectoPorIdAsync(int id)
        {
            ProyectoViewModel? proyecto = null;
            string procedure = "sp_ListarProyectoPorId";

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
                            proyecto = new ProyectoViewModel
                            {
                                IdProyecto = reader.GetInt32(reader.GetOrdinal("IdProyecto")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion"))
                                    ? ""
                                    : reader.GetString(reader.GetOrdinal("Descripcion")),
                                FechaInicio = reader.IsDBNull(reader.GetOrdinal("FechaInicio"))
                                    ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                            };

                        }
                    }
                }
            }

            return proyecto;
        }

        public async Task<int> ActualizarProyectoAsync(ProyectoViewModel proyecto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarProyecto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdProyecto", proyecto.IdProyecto);
                    cmd.Parameters.AddWithValue("@Nombre", proyecto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", proyecto.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaInicio", proyecto.FechaInicio);
                    cmd.Parameters.AddWithValue("@FK_IdEstado", proyecto.FK_IdEstado);

                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> EliminarProyectoAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_EliminarProyecto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdProyecto", id);

                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        // Listar todos los equipos
        public async Task<List<EquipoViewModel>> ObtenerEquiposAsync()
        {
            var equipos = new List<EquipoViewModel>();
            string procedure = "sp_ListarEquipos";

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
                            equipos.Add(new EquipoViewModel
                            {
                                IdEquipo = reader.GetInt32(reader.GetOrdinal("IdEquipo")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                            });
                        }
                    }
                }
            }

            return equipos;
        }

        // Insertar un equipo
        public async Task<int> InsertarEquipoAsync(EquipoViewModel equipo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_InsertarEquipo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombre", equipo.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", equipo.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FK_IdEstado", equipo.FK_IdEstado);

                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        
        public async Task<EquipoViewModel?> ObtenerEquipoPorIdAsync(int id)
        {
            EquipoViewModel? equipo = null;
            string procedure = "sp_ObtenerEquipoPorId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdEquipo", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            equipo = new EquipoViewModel
                            {
                                IdEquipo = reader.GetInt32(reader.GetOrdinal("IdEquipo")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                            };
                        }
                    }
                }
            }

            return equipo;
        }

        public async Task<int> ActualizarEquipoAsync(EquipoViewModel equipo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarEquipo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdEquipo", equipo.IdEquipo);
                    cmd.Parameters.AddWithValue("@Nombre", equipo.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", equipo.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FK_IdEstado", equipo.FK_IdEstado);

                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> EliminarEquipoAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_EliminarEquipo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdEquipo", id);

                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        // Asignar equipo a proyecto
        public async Task<int> AsignarEquipoAProyectoAsync(int idProyecto, int idEquipo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_AsignarEquipoAProyecto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FK_IdProyecto", idProyecto);
                    cmd.Parameters.AddWithValue("@FK_IdEquipo", idEquipo);

                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Listar equipos asignados a un proyecto
        public async Task<List<EquipoViewModel>> ObtenerEquiposPorProyectoAsync(int idProyecto)
        {
            var equipos = new List<EquipoViewModel>();
            string procedure = "sp_ListarEquiposPorProyecto";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FK_IdProyecto", idProyecto);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            equipos.Add(new EquipoViewModel
                            {
                                IdEquipo = reader.GetInt32(reader.GetOrdinal("IdEquipo")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                            });
                        }
                    }
                }
            }

            return equipos;
        }

        public async Task<List<SelectListItem>> ListarEstadosComboAsync()
        {
            var lista = new List<SelectListItem>();
            string query = "SELECT IdEstado, Estado FROM Estados WHERE Activo = 1 AND Estado IN ('Activo', 'Inactivo') ORDER BY Estado";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new SelectListItem
                        {
                            Value = reader["IdEstado"].ToString(),
                            Text = reader["Estado"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
    }
}
