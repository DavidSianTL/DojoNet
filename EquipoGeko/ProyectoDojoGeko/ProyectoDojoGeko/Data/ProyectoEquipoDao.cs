using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models.ProyectoEquipo;

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
                                FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
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
                    cmd.Parameters.AddWithValue("@FechaFin", DBNull.Value); // No manejado en modelo, se puede ajustar
                    cmd.Parameters.AddWithValue("@FK_IdEstado", proyecto.FK_IdEstado);

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
    }
}
