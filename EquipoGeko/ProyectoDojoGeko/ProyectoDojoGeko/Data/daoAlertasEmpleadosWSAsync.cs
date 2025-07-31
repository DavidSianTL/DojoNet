using System.Data;
using System.Data.SqlClient;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoAlertasEmpleadosWSAsync
    {
        private readonly string _connectionString;

        public daoAlertasEmpleadosWSAsync(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Obtiene las alertas de empleados relacionadas con vacaciones (más de 14 días disponibles)
        /// </summary>
        /// <returns>Lista de alertas de empleados</returns>
        public async Task<List<AlertaEmpleadoVacacionesViewModel>> ObtenerAlertasEmpleadosVacacionesAsync()
        {
            var alertas = new List<AlertaEmpleadoVacacionesViewModel>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("sp_ObtenerAlertasEmpleadosVacaciones_Completo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 30; // 30 segundos de timeout

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var alerta = new AlertaEmpleadoVacacionesViewModel
                                {
                                    IdEmpleado = reader.GetInt32("IdEmpleado"),
                                    NombresEmpleado = reader.IsDBNull("NombresEmpleado") ? string.Empty : reader.GetString("NombresEmpleado"),
                                    ApellidosEmpleado = reader.IsDBNull("ApellidosEmpleado") ? string.Empty : reader.GetString("ApellidosEmpleado"),
                                    Codigo = reader.IsDBNull("Codigo") ? string.Empty : reader.GetString("Codigo"),
                                    TipoNotificacion = reader.IsDBNull("TipoNotificacion") ? string.Empty : reader.GetString("TipoNotificacion"),
                                    FechaIngreso = reader.IsDBNull("FechaIngreso") ? DateTime.MinValue : reader.GetDateTime("FechaIngreso"),

                                    // Nuevos campos calculados
                                    AniosTrabajados = reader.IsDBNull("AniosTrabajados") ? 0 : reader.GetDecimal("AniosTrabajados"),
                                    DiasAcumuladosTotal = reader.IsDBNull("DiasAcumuladosTotal") ? 0 : reader.GetDecimal("DiasAcumuladosTotal"),
                                    DiasYaTomados = reader.IsDBNull("DiasYaTomados") ? 0 : reader.GetDecimal("DiasYaTomados"),
                                    DiasDisponibles = reader.IsDBNull("DiasDisponibles") ? 0 : reader.GetDecimal("DiasDisponibles")
                                };

                                alertas.Add(alerta);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log específico para errores de SQL
                Console.WriteLine($"Error SQL en ObtenerAlertasEmpleadosVacacionesAsync: {sqlEx.Message}");
                Console.WriteLine($"Número de error SQL: {sqlEx.Number}");
                throw new Exception($"Error al obtener alertas de empleados desde la base de datos: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Log general para otros errores
                Console.WriteLine($"Error general en ObtenerAlertasEmpleadosVacacionesAsync: {ex.Message}");
                throw new Exception($"Error inesperado al obtener alertas de empleados: {ex.Message}", ex);
            }

            return alertas;
        }

        /// <summary>
        /// Obtiene el conteo total de alertas de empleados
        /// </summary>
        /// <returns>Número total de alertas</returns>
        public async Task<int> ObtenerConteoAlertasEmpleadosAsync()
        {
            try
            {
                var alertas = await ObtenerAlertasEmpleadosVacacionesAsync();
                return alertas.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerConteoAlertasEmpleadosAsync: {ex.Message}");
                return 0; // Retorna 0 en caso de error para evitar romper el dashboard
            }
        }
    }
}