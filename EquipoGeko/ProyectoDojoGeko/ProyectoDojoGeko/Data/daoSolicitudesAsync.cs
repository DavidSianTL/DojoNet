using ProyectoDojoGeko.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ProyectoDojoGeko.Data
{
    public class daoSolicitudesAsync
    {
        private readonly string _connectionString;

        public daoSolicitudesAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> InsertarSolicitudAsync(int idEmpleado, List<SolicitudDetalleViewModel> detalles)
        {
            // 1. Crear una tabla en memoria para el Parámetro con Valor de Tabla (TVP)
            var dtDetalles = new DataTable();
            dtDetalles.Columns.Add("FechaInicio", typeof(System.DateTime));
            dtDetalles.Columns.Add("FechaFin", typeof(System.DateTime));
            dtDetalles.Columns.Add("DiasHabiles", typeof(int));

            // 2. Llenar la tabla en memoria con los detalles de la solicitud
            foreach (var detalle in detalles)
            {
                dtDetalles.Rows.Add(detalle.FechaInicio, detalle.FechaFin, detalle.DiasHabilesTomados);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_InsertarSolicitud", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // 3. Añadir los parámetros, incluyendo el TVP
                    command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                    var tvpParam = command.Parameters.AddWithValue("@Detalles", dtDetalles);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "SolicitudDetalleType"; // Este nombre DEBE COINCIDIR con el tipo creado en SQL Server

                    // 4. Ejecutar y obtener el ID de la solicitud creada
                    var result = await command.ExecuteScalarAsync();
                    return (int)result;
                }
            }
        }

        // OTROS DESARROLLADORES PUEDEN AGREGAR SUS MÉTODOS AQUÍ
        // Por ejemplo:
        // public async Task<List<SolicitudEncabezadoViewModel>> ObtenerSolicitudesPorEmpleadoAsync(int idEmpleado) { ... }
        // public async Task AutorizarSolicitudAsync(int idSolicitud) { ... }



        /*ErickDev: Método para obtener detalle de solicitud*/
        /*--------*/
        public async Task<SolicitudViewModel> ObtenerDetalleSolicitudAsync(int idSolicitud)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ObtenerDetalleSolicitud", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdSolicitud", idSolicitud);

                    SolicitudViewModel solicitud = null;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            solicitud = new SolicitudViewModel
                            {
                                Encabezado = new SolicitudEncabezadoViewModel
                                {
                                    IdSolicitud = (int)reader["IdSolicitud"],
                                    IdEmpleado = (int)reader["IdEmpleado"],
                                    NombreEmpleado = null, // Se puede obtener el nombre del empleado si se une con la tabla de empleados
                                    DiasSolicitadosTotal = (int)reader["DiasSolicitadosTotal"],
                                    FechaIngresoSolicitud = (DateTime)reader["FechaIngresoSolicitud"],
                                    Estado = reader["Estado"].ToString()
                                }
                            };
                        }

                        if (solicitud == null) return null;

                        await reader.NextResultAsync();
                        while (await reader.ReadAsync())
                        {
                            solicitud.Detalles.Add(new SolicitudDetalleViewModel
                            {
                                IdSolicitudDetalle = (int)reader["IdSolicitudDetalle"],
                                FechaInicio = (DateTime)reader["FechaInicio"],
                                FechaFin = (DateTime)reader["FechaFin"],
                                DiasHabilesTomados = (int)reader["DiasHabilesTomados"]
                            });
                        }
                    }
                    return solicitud;
                }
            }
        }
        /*-------------*/
        /*End ErickDev*/

    }
}