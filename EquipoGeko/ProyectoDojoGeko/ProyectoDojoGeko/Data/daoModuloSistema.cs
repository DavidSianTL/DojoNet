using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoModuloSistema
    {
        private readonly string _connectionString;

        public daoModuloSistema(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Lista todos los módulos asignados a un sistema usando el SP
        public async Task<List<ModuloViewModel>> ObtenerModulosPorSistemaAsync(int idSistema)
        {
            var lista = new List<ModuloViewModel>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_ListarModulosPorSistema", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FK_IdSistema", idSistema);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new ModuloViewModel
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("IdModulo")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                            FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                        });
                    }
                }
            }
            return lista;
        }

        // Lista todas las asignaciones de módulos a sistemas usando el SP
        public async Task<DataTable> ListarModulosSistemaAsync()
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_ListarModulosSistema", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        // Asigna un módulo a un sistema usando el SP
        public async Task InsertarModuloSistemaAsync(ModuloSistemaViewModel model)
        {
            string procedure = "sp_AsignarModuloASistema";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FK_IdSistema", model.FK_IdSistema);
                    cmd.Parameters.AddWithValue("@FK_IdModulo", model.FK_IdModulo);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

        }

        // Elimina la asignación de un módulo a un sistema usando el SP
        public async Task EliminarModuloDeSistemaAsync(int idSistema, int idModulo)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_EliminarModuloDeSistema", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FK_IdSistema", idSistema);
                cmd.Parameters.AddWithValue("@FK_IdModulo", idModulo);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

       
    }
}
