using ProyectoDojoGeko.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Dtos.Solicitudes;

namespace ProyectoDojoGeko.Data
{
    public class daoSolicitudesAsync
    {
        private readonly string _connectionString;

        public daoSolicitudesAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para mapear encabezados de solicitud
        private SolicitudEncabezadoViewModel _mapearSolicitud(SqlDataReader reader)
        {
            return new SolicitudEncabezadoViewModel
            {
                IdSolicitud = Convert.ToInt32(reader["IdSolicitud"]),
                IdEmpleado = Convert.ToInt32(reader["FK_IdEmpleado"]),
                NombreEmpleado = reader["NombresEmpleado"].ToString() ?? "",
                DiasSolicitadosTotal = Convert.ToDecimal(reader["DiasSolicitadosTotal"]),
                FechaIngresoSolicitud = Convert.ToDateTime(reader["FechaIngresoSolicitud"])
            };
        }

        // Método para mapear encabezados de solicitud Result
        private SolicitudEncabezadoResult _mapearSolicitudResult(SqlDataReader reader)
        {
            return new SolicitudEncabezadoResult
            {
                IdSolicitud = Convert.ToInt32(reader["IdSolicitud"]),
                IdEmpleado = Convert.ToInt32(reader["FK_IdEmpleado"]),
                NombreEstado = reader["NombreEstado"].ToString() ?? "",
                NombreEmpleado = reader["NombresEmpleado"].ToString() ?? "",
                DiasSolicitadosTotal = Convert.ToDecimal(reader["DiasSolicitadosTotal"]),
                FechaIngresoSolicitud = Convert.ToDateTime(reader["FechaIngresoSolicitud"]),
                FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                NombreEmpresa = reader["NombreEmpresa"].ToString() ?? ""

            };
        }

        // Método para insertar una nueva solicitud de vacaciones
        public async Task<int> InsertarSolicitudAsync(SolicitudViewModel solicitud)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // 1. Insertar encabezado y obtener el ID generado
                int idSolicitud;
                using (var cmdEnc = new SqlCommand("sp_InsertarSolicitudEncabezado", connection))
                {
                    cmdEnc.CommandType = CommandType.StoredProcedure;
                    cmdEnc.Parameters.AddWithValue("@IdEmpleado", solicitud.Encabezado.IdEmpleado);
                    cmdEnc.Parameters.AddWithValue("@NombresEmpleado", solicitud.Encabezado.NombreEmpleado);
                    cmdEnc.Parameters.AddWithValue("@DiasSolicitadosTotal", solicitud.Encabezado.DiasSolicitadosTotal);
                    cmdEnc.Parameters.AddWithValue("@FechaIngresoSolicitud", solicitud.Encabezado.FechaIngresoSolicitud);
                    cmdEnc.Parameters.AddWithValue("@SolicitudLider", solicitud.Encabezado.SolicitudLider);
                    cmdEnc.Parameters.AddWithValue("@Observaciones", solicitud.Encabezado.Observaciones);
                    cmdEnc.Parameters.AddWithValue("@Estado", solicitud.Encabezado.Estado);

                    // SP retorna el ID con SELECT SCOPE_IDENTITY()
                    idSolicitud = Convert.ToInt32(await cmdEnc.ExecuteScalarAsync());
                }

                // 2. Insertar detalles usando el ID del encabezado
                foreach (var detalle in solicitud.Detalles)
                {
                    using (var cmdDet = new SqlCommand("sp_InsertarSolicitudDetalle", connection))
                    {
                        cmdDet.CommandType = CommandType.StoredProcedure;
                        cmdDet.Parameters.AddWithValue("@IdSolicitud", idSolicitud);
                        cmdDet.Parameters.AddWithValue("@FechaInicio", detalle.FechaInicio);
                        cmdDet.Parameters.AddWithValue("@FechaFin", detalle.FechaFin);
                        cmdDet.Parameters.AddWithValue("@DiasHabiles", detalle.DiasHabilesTomados);

                        await cmdDet.ExecuteNonQueryAsync();
                    }
                }

                return idSolicitud;
            }
        }

       
    #region Métodos GET de encabezado de solicitud


        //JuniorDev | Método para obtener encabezado de solicitud por autorizador (IdAutorizador)
        public async Task<List<SolicitudEncabezadoViewModel>> ObtenerSolicitudEncabezadoAutorizadorAsync(int? IdAutorizador)
        {
            var solicitudes = new List<SolicitudEncabezadoViewModel>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var procedure = new SqlCommand("sp_ListarSolicitudEncabezado_Autorizador", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                procedure.Parameters.AddWithValue("@FK_IdAutorizador", IdAutorizador);
                await connection.OpenAsync();
                using SqlDataReader reader = await procedure.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    solicitudes.Add(_mapearSolicitud(reader)); // Se añade la solicitudeEncabezado mapeada
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los encabezados de las solicitudes por IdAutorizador faltantes de autorizar", ex);
            }

            return solicitudes;
        }
        public async Task<List<SolicitudEncabezadoViewModel>> ObtenerSolicitudEncabezadoAsync()
        {
            var solicitudes = new List<SolicitudEncabezadoViewModel>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var procedure = new SqlCommand("sp_ListarSolicitudEncabezado", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                await connection.OpenAsync();
                using SqlDataReader reader = await procedure.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    solicitudes.Add(_mapearSolicitud(reader)); // Se añade la solicitudeEncabezado mapeada
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los encabezados de las solicitudes", ex);
            }

            return solicitudes;
        }
        public async Task<List<SolicitudEncabezadoResult>> ObtenerSolicitudEncabezadoCamposAsync()
        {
            var solicitudes = new List<SolicitudEncabezadoResult>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var procedure = new SqlCommand("sp_ListarSolicitudEncabezado_Campos", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                await connection.OpenAsync();
                using SqlDataReader reader = await procedure.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    solicitudes.Add(_mapearSolicitudResult(reader)); // Se añade la solicitudeEncabezado mapeada
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los encabezados de las solicitudes", ex);
            }

            return solicitudes;
        }
             

    #endregion


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
                                    IdSolicitud = Convert.ToInt32(reader["IdSolicitud"]),
                                    IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                                    NombreEmpleado = null, // Se asignará en el controlador si es necesario
                                    DiasSolicitadosTotal = (decimal)reader["DiasSolicitadosTotal"],
                                    FechaIngresoSolicitud = (DateTime)reader["FechaIngresoSolicitud"],
                                    Estado = reader["Estado"].ToString() == "Ingresada" ? 1 : 0 // Ajustar según los estados que existan
                                },
                                Detalles = new List<SolicitudDetalleViewModel>()
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
                                DiasHabilesTomados = (decimal)reader["DiasHabilesTomados"]
                            });
                        }
                    }
                    return solicitud;
                }
            }
        }
        /*-------------*/
        /*End ErickDev*/

        // Método para obtener encabezado de solicitud por empleado (IdEmpleado)
        public async Task<List<SolicitudViewModel>> ObtenerSolicitudesPorEmpleadoAsync(int idEmpleado)
        {
            var solicitudes = new List<SolicitudViewModel>();

            // 1. Obtener encabezados
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ObtenerSolicitudesPorEmpleado", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var solicitud = new SolicitudViewModel
                            {
                                Encabezado = new SolicitudEncabezadoViewModel
                                {
                                    IdSolicitud = Convert.ToInt32(reader["IdSolicitud"]),
                                    IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                                    NombreEmpleado = null,
                                    DiasSolicitadosTotal = (decimal)reader["DiasSolicitadosTotal"],
                                    FechaIngresoSolicitud = (DateTime)reader["FechaIngresoSolicitud"],
                                    Estado = Convert.ToInt32(reader["Estado"])
                                },
                                Detalles = new List<SolicitudDetalleViewModel>()
                            };
                            solicitudes.Add(solicitud);
                        }
                    }
                }

                // 2. Para cada solicitud, obtener los detalles
                foreach (var solicitud in solicitudes)
                {
                    using (var cmdDetalle = new SqlCommand("sp_ObtenerDetallesPorSolicitud", connection))
                    {
                        cmdDetalle.CommandType = CommandType.StoredProcedure;
                        cmdDetalle.Parameters.AddWithValue("@IdSolicitud", solicitud.Encabezado.IdSolicitud);

                        using (var readerDetalle = await cmdDetalle.ExecuteReaderAsync())
                        {
                            while (await readerDetalle.ReadAsync())
                            {
                                solicitud.Detalles.Add(new SolicitudDetalleViewModel
                                {
                                    IdSolicitudDetalle = (int)readerDetalle["IdSolicitudDetalle"],
                                    FechaInicio = (DateTime)readerDetalle["FechaInicio"],
                                    FechaFin = (DateTime)readerDetalle["FechaFin"],
                                    DiasHabilesTomados = (decimal)readerDetalle["DiasHabilesTomados"]
                                });
                            }
                        }
                    }
                }
            }

            return solicitudes;
        }



    }
}