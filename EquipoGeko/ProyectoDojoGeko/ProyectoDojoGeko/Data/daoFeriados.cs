using ProyectoDojoGeko.Models;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoDojoGeko.Data
{
    public class daoFeriados
    {
        private readonly string cadenaSQL;

        public daoFeriados(string connectionString)
        {
            cadenaSQL = connectionString;
        }

        // Este método obtiene una lista de feriados fijos desde la base de datos.
        public async Task<List<FeriadoFijoViewModel>> ListarFeriadosFijos()
        {
            var lista = new List<FeriadoFijoViewModel>();
            using (var con = new SqlConnection(cadenaSQL))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT ff.*, tf.Nombre as TipoFeriadoNombre FROM DiasFestivosFijos ff JOIN TipoFeriado tf ON ff.TipoFeriadoId = tf.TipoFeriadoId", con);
                cmd.CommandType = CommandType.Text;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new FeriadoFijoViewModel
                        {
                            Dia = Convert.ToInt32(reader["Dia"]),
                            Mes = Convert.ToInt32(reader["Mes"]),
                            TipoFeriadoId = Convert.ToInt32(reader["TipoFeriadoId"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            ProporcionDia = Convert.ToDecimal(reader["ProporcionDia"]),
                            TipoFeriadoNombre = reader["TipoFeriadoNombre"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        // Este método obtiene un feriado fijo específico basado en el día, mes y tipo de feriado.
        public async Task<FeriadoFijoViewModel> ObtenerFeriadoFijo(int dia, int mes, int tipoFeriadoId)
        {
            FeriadoFijoViewModel feriado = null;
            using (var con = new SqlConnection(cadenaSQL))
            {
                await con.OpenAsync();
                string textoCmd = "SELECT f.*, t.Nombre AS TipoFeriadoNombre FROM DiasFestivosFijos f JOIN TipoFeriado t ON f.TipoFeriadoId = t.TipoFeriadoId WHERE f.Dia = @Dia AND f.Mes = @Mes AND f.TipoFeriadoId = @TipoFeriadoId";
                SqlCommand cmd = new SqlCommand(textoCmd, con);
                cmd.Parameters.AddWithValue("@Dia", dia);
                cmd.Parameters.AddWithValue("@Mes", mes);
                cmd.Parameters.AddWithValue("@TipoFeriadoId", tipoFeriadoId);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        feriado = new FeriadoFijoViewModel
                        {
                            Dia = Convert.ToInt32(reader["Dia"]),
                            Mes = Convert.ToInt32(reader["Mes"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            TipoFeriadoId = Convert.ToInt32(reader["TipoFeriadoId"]),
                            ProporcionDia = Convert.ToDecimal(reader["ProporcionDia"]),
                            TipoFeriadoNombre = reader["TipoFeriadoNombre"].ToString()
                        };
                    }
                }
            }
            return feriado;
        }

        // Este método obtiene un feriado variable específico basado en su ID.
        public async Task<FeriadoVariableViewModel> ObtenerFeriadoVariable(int id)
        {
            FeriadoVariableViewModel feriado = null;
            using (var con = new SqlConnection(cadenaSQL))
            {
                await con.OpenAsync();
                string textoCmd = "SELECT f.*, t.Nombre AS TipoFeriadoNombre FROM DiasFestivosVariables f JOIN TipoFeriado t ON f.TipoFeriadoId = t.TipoFeriadoId WHERE f.Id = @Id";
                SqlCommand cmd = new SqlCommand(textoCmd, con);
                cmd.Parameters.AddWithValue("@Id", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        feriado = new FeriadoVariableViewModel
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            TipoFeriadoId = Convert.ToInt32(reader["TipoFeriadoId"]),
                            ProporcionDia = Convert.ToDecimal(reader["ProporcionDia"]),
                            TipoFeriadoNombre = reader["TipoFeriadoNombre"].ToString()
                        };
                    }
                }
            }
            return feriado;
        }

        // Este método obtiene una lista de feriados variables desde la base de datos.
        public async Task<List<FeriadoVariableViewModel>> ListarFeriadosVariables()
        {
            var lista = new List<FeriadoVariableViewModel>();
            using (var con = new SqlConnection(cadenaSQL))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT dv.*, tf.Nombre as TipoFeriadoNombre FROM DiasFestivosVariables dv JOIN TipoFeriado tf ON dv.TipoFeriadoId = tf.TipoFeriadoId", con);
                cmd.CommandType = CommandType.Text;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new FeriadoVariableViewModel
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            TipoFeriadoId = Convert.ToInt32(reader["TipoFeriadoId"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            ProporcionDia = Convert.ToDecimal(reader["ProporcionDia"]),
                            TipoFeriadoNombre = reader["TipoFeriadoNombre"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        // Este método obtiene una lista de tipos de feriado desde la base de datos.
        public async Task<List<TipoFeriadoViewModel>> ListarTiposFeriado()
        {
            var lista = new List<TipoFeriadoViewModel>();
            using (var con = new SqlConnection(cadenaSQL))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT TipoFeriadoId, Nombre, Descripcion FROM TipoFeriado", con);
                cmd.CommandType = CommandType.Text;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new TipoFeriadoViewModel
                        {
                            TipoFeriadoId = Convert.ToInt32(reader["TipoFeriadoId"]),
                            Nombre = reader["Nombre"].ToString(),
                            Descripcion = reader["Descripcion"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        // Este método ejecuta un procedimiento almacenado y devuelve un mensaje de salida.
        private async Task<string> EjecutarSPConMensaje(string nombreSP, List<SqlParameter> parametros)
        {
            try
            {
                using (var con = new SqlConnection(cadenaSQL))
                {
                    await con.OpenAsync();
                    using (var cmd = new SqlCommand(nombreSP, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parametros != null)
                        {
                            cmd.Parameters.AddRange(parametros.ToArray());
                        }

                        var mensajeSalida = new SqlParameter("@MensajeSalida", SqlDbType.NVarChar, 200);
                        mensajeSalida.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(mensajeSalida);

                        await cmd.ExecuteNonQueryAsync();
                        return mensajeSalida.Value?.ToString() ?? "Operación completada sin mensaje de retorno.";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // Este método inserta un nuevo feriado variable en la base de datos.
        public async Task<string> InsertarFeriadoFijo(FeriadoFijoViewModel model)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@Dia", model.Dia),
                new SqlParameter("@Mes", model.Mes),
                new SqlParameter("@TipoFeriadoId", model.TipoFeriadoId),
                new SqlParameter("@Descripcion", model.Descripcion),
                new SqlParameter("@ProporcionDia", model.ProporcionDia),
                new SqlParameter("@Usr_creacion", model.Usr_creacion)
            };
            return await EjecutarSPConMensaje("sp_Insertar_FeriadoFijo", parametros);
        }

        // Este método actualiza un feriado fijo existente en la base de datos.
        public async Task<string> ActualizarFeriadoFijo(FeriadoFijoViewModel model)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@Dia", model.Dia),
                new SqlParameter("@Mes", model.Mes),
                new SqlParameter("@TipoFeriadoId", model.TipoFeriadoId),
                new SqlParameter("@Descripcion", model.Descripcion),
                new SqlParameter("@ProporcionDia", model.ProporcionDia),
                new SqlParameter("@Usr_modifica", model.Usr_modifica),
                new SqlParameter("@Original_Dia", model.Original_Dia),
                new SqlParameter("@Original_Mes", model.Original_Mes),
                new SqlParameter("@Original_TipoFeriadoId", model.Original_TipoFeriadoId)
            };
            return await EjecutarSPConMensaje("sp_Actualizar_FeriadoFijo", parametros);
        }

        // Este método elimina un feriado fijo de la base de datos.
        public async Task<string> EliminarFeriadoFijo(FeriadoFijoViewModel model)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@Dia", model.Dia),
                new SqlParameter("@Mes", model.Mes),
                new SqlParameter("@TipoFeriadoId", model.TipoFeriadoId)
            };
            return await EjecutarSPConMensaje("sp_Eliminar_FeriadoFijo", parametros);
        }

        // Este método inserta o actualiza un feriado variable en la base de datos.
        public async Task<string> MantFeriadoVariable(FeriadoVariableViewModel feriado)
        {
            using (var con = new SqlConnection(cadenaSQL))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_Mant_DiasFestivosVariables", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_op_operacion", feriado.IsEditMode ? "A" : "I");
                cmd.Parameters.AddWithValue("@Id", feriado.Id);
                cmd.Parameters.AddWithValue("@Fecha", feriado.Fecha);
                cmd.Parameters.AddWithValue("@Descripcion", feriado.Descripcion);
                cmd.Parameters.AddWithValue("@TipoFeriadoId", feriado.TipoFeriadoId);
                cmd.Parameters.AddWithValue("@ProporcionDia", feriado.ProporcionDia);
                cmd.Parameters.AddWithValue("@Usr_creacion", feriado.Usr_creacion ?? "System");
                cmd.Parameters.AddWithValue("@Usr_modifica", feriado.Usr_modifica ?? "System");

                SqlParameter mensajeSalida = new SqlParameter("@MensajeSalida", SqlDbType.NVarChar, 200);
                mensajeSalida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(mensajeSalida);

                await cmd.ExecuteNonQueryAsync();

                return mensajeSalida.Value.ToString();
            }
        }

        // Este método elimina un feriado variable de la base de datos.
        public async Task<string> EliminarFeriadoVariable(int id)
        {
            using (var con = new SqlConnection(cadenaSQL))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_Mant_DiasFestivosVariables", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@i_op_operacion", 'E'); // 'E' para Eliminar
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Fecha", DBNull.Value); // Parámetro requerido, se envía nulo
                cmd.Parameters.AddWithValue("@Descripcion", DBNull.Value); // Parámetro requerido, se envía nulo
                cmd.Parameters.AddWithValue("@TipoFeriadoId", DBNull.Value); // Parámetro requerido, se envía nulo
                cmd.Parameters.AddWithValue("@ProporcionDia", DBNull.Value); // Parámetro requerido, se envía nulo
                cmd.Parameters.AddWithValue("@Usr_creacion", DBNull.Value); // Parámetro requerido, se envía nulo
                cmd.Parameters.AddWithValue("@Usr_modifica", DBNull.Value); // Parámetro requerido, se envía nulo

                SqlParameter mensajeSalida = new SqlParameter("@MensajeSalida", SqlDbType.NVarChar, 200);
                mensajeSalida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(mensajeSalida);

                await cmd.ExecuteNonQueryAsync();

                return mensajeSalida.Value.ToString();
            }
        }
    }
}
