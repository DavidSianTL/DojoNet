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

        public async Task<string> MantFeriadoFijo(FeriadoFijoViewModel model)
        {
            try
            {
                using (var con = new SqlConnection(cadenaSQL))
                {
                    await con.OpenAsync();
                    using (var cmd = new SqlCommand("sp_Mant_DiasFestivosFijos", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Determinar la operación basada en si tiene valores originales
                        string operacion = (!string.IsNullOrEmpty(model.Original_Dia?.ToString()) && 
                                          !string.IsNullOrEmpty(model.Original_Mes?.ToString()) && 
                                          model.Original_TipoFeriadoId.HasValue) ? "A" : "I";

                        // Parámetros principales
                        cmd.Parameters.AddWithValue("@i_op_operacion", operacion);
                        cmd.Parameters.AddWithValue("@Dia", model.Dia);
                        cmd.Parameters.AddWithValue("@Mes", model.Mes);
                        cmd.Parameters.AddWithValue("@TipoFeriadoId", model.TipoFeriadoId);
                        cmd.Parameters.AddWithValue("@Descripcion", model.Descripcion ?? (object)DBNull.Value);
                        
                        // Usar directamente ProporcionDia que ya es decimal
                        cmd.Parameters.AddWithValue("@ProporcionDia", model.ProporcionDia);
                        
                        cmd.Parameters.AddWithValue("@Usr_creacion", model.Usr_creacion ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Usr_modifica", model.Usr_modifica ?? (object)DBNull.Value);

                        // Parámetros para actualización (valores originales)
                        if (operacion == "A")
                        {
                            cmd.Parameters.AddWithValue("@Original_Dia", model.Original_Dia ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Original_Mes", model.Original_Mes ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Original_TipoFeriadoId", model.Original_TipoFeriadoId ?? (object)DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Original_Dia", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Original_Mes", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Original_TipoFeriadoId", DBNull.Value);
                        }

                        // Parámetro de salida
                        var outputParam = new SqlParameter("@MensajeSalida", SqlDbType.NVarChar, 200)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        await cmd.ExecuteNonQueryAsync();

                        // Retornar el mensaje del procedimiento almacenado
                        return outputParam.Value?.ToString() ?? "Operación completada";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

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
    }
}
