using System.Data;
using Microsoft.Data.SqlClient;
using OfficeOpenXml.Style;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoModulo
    {
        private readonly string _connectionString; // Cadena de conexión a la base de datos

        /// Constructor que recibe la cadena de conexión
        public daoModulo(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de todos los módulos
        public async Task<List<ModuloViewModel>> ObtenerModulosAsync()
        {
            var modulos = new List<ModuloViewModel>();
            string procedure = "sp_ListarModulos";
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
                            modulos.Add(new ModuloViewModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("IdModulo")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                            });
                        }
                    }
                }
            }
            return modulos;
        }

        // Método para obtener los módulos asignados a un sistema específico
        public async Task<List<ModuloViewModel>> ObtenerModulosPorSistemaAsync(int idSistema)
        {
            var modulos = new List<ModuloViewModel>();
            string procedure = "sp_ListarModulosPorSistema";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FK_IdSistema", idSistema);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            modulos.Add(new ModuloViewModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("IdModulo")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                FK_IdEstado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                            });
                        }
                    }
                }
            }
            return modulos;
        }
       

        // Método para insertar un nuevo módulo en base de datos
        public async Task<int> InsertarModuloAsync(ModuloViewModel modulo)
        {
            int idModulo = 0; // Id del modulo para asignar a un sistema más tarde
            try
            {
                using var cnn = new SqlConnection(_connectionString);
                await cnn.OpenAsync();

                using var procedure = new SqlCommand("sp_InsertarModulo", cnn);
                procedure.CommandType = CommandType.StoredProcedure;

                procedure.Parameters.AddWithValue("@Nombre", modulo.Nombre ?? string.Empty);
                procedure.Parameters.AddWithValue("@Descripcion", modulo.Descripcion ?? string.Empty);
                procedure.Parameters.AddWithValue("@FK_IdEstado", modulo.FK_IdEstado);

                var result = await procedure.ExecuteScalarAsync();
                idModulo = Convert.ToInt32(result);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar módulo {ex}");
            }
                
            return idModulo;
        }
    }
}
