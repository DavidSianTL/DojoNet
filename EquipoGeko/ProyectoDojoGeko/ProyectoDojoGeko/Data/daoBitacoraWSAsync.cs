using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoBitacoraWSAsync
    {

        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoBitacoraWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de bitácoras
        public async Task<List<BitacoraViewModel>> ObtenerBitacorasAsync()
        {
            // Declaración de la lista de bitácoras
            var bitacoras = new List<BitacoraViewModel>();
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ListarBitacoras";
            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await connection.OpenAsync();
                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand command = new SqlCommand(procedure, connection))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    command.CommandType = CommandType.StoredProcedure;
                    // Ejecuta el comando y obtiene un lector de datos asíncrono
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Mientras haya registros, los lee y los agrega a la lista de bitácoras
                        while (await reader.ReadAsync())
                        {
                            bitacoras.Add(new BitacoraViewModel
                            {
                                IdBitacora = reader.GetInt32(reader.GetOrdinal("IdBitacora")),
                                FechaEntrada = reader.GetDateTime(reader.GetOrdinal("FechaEntrada")),
                                Accion = reader.GetString(reader.GetOrdinal("Accion")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                FK_IdUsuario = reader.GetInt32(reader.GetOrdinal("FK_IdUsuario")),
                                FK_IdSistema = reader.GetInt32(reader.GetOrdinal("FK_IdSistema"))
                            });
                        }
                    }
                }
            }
            // Después de leer todos los registros, los devolvemos
            return bitacoras;
        }

        // Método para guardar una nueva bitácora
        public async Task<int> InsertarBitacoraAsync(BitacoraViewModel bitacora)
        {

            // Validación de los parámetros de entrada
            var parametros = new[]
            {
                new SqlParameter("@Accion", SqlDbType.NVarChar, 75) { Value = bitacora.Accion },
                new SqlParameter("@Descripcion", SqlDbType.NVarChar, 255) { Value = bitacora.Descripcion },
                new SqlParameter("@FK_IdUsuario", SqlDbType.Int) { Value = bitacora.FK_IdUsuario },
                new SqlParameter("@FK_IdSistema", SqlDbType.Int) { Value = bitacora.FK_IdSistema }
            };

            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await connection.OpenAsync();

                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand command = new SqlCommand("sp_InsertarBitacora", connection))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parametros);
                    // Ejecuta el comando y obtiene el ID de la nueva bitácora
                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
                }


            }
        }
        // Agregar este método a tu clase DaoBitacoraWSAsync existente
        public async Task<List<BitacoraViewModel>> ObtenerBitacorasFiltradas(int? idUsuario, string accion,
        string fechaDesde, string fechaHasta)
            {
                try
                {
                    var parametros = new List<SqlParameter>();
                    var condiciones = new List<string>();

                    // Construir consulta base
                    string sql = @"
                SELECT IdBitacora, FechaEntrada, Accion, Descripcion, FK_IdUsuario, FK_IdSistema
                FROM Bitacora 
                WHERE 1=1";

                    // Agregar filtros
                    if (idUsuario.HasValue)
                    {
                        condiciones.Add("FK_IdUsuario = @IdUsuario");
                        parametros.Add(new SqlParameter("@IdUsuario", idUsuario.Value));
                    }

                    if (!string.IsNullOrEmpty(accion))
                    {
                        condiciones.Add("Accion = @Accion");
                        parametros.Add(new SqlParameter("@Accion", accion));
                    }

                    if (!string.IsNullOrEmpty(fechaDesde))
                    {
                        condiciones.Add("CAST(FechaEntrada AS DATE) >= @FechaDesde");
                        parametros.Add(new SqlParameter("@FechaDesde", DateTime.Parse(fechaDesde)));
                    }

                    if (!string.IsNullOrEmpty(fechaHasta))
                    {
                        condiciones.Add("CAST(FechaEntrada AS DATE) <= @FechaHasta");
                        parametros.Add(new SqlParameter("@FechaHasta", DateTime.Parse(fechaHasta)));
                    }

                    // Combinar condiciones
                    if (condiciones.Any())
                    {
                        sql += " AND " + string.Join(" AND ", condiciones);
                    }

                    sql += " ORDER BY FechaEntrada DESC";

                    // Ejecutar consulta usando tu conexión existente
                    var registros = new List<BitacoraViewModel>();

                    using (var connection = new SqlConnection(_connectionString))
                    {
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddRange(parametros.ToArray());

                            await connection.OpenAsync();
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    registros.Add(new BitacoraViewModel
                                    {
                                        IdBitacora = reader.GetInt32("IdBitacora"),
                                        FechaEntrada = reader.GetDateTime("FechaEntrada"),
                                        Accion = reader.GetString("Accion"),
                                        Descripcion = reader.IsDBNull("Descripcion") ? "" : reader.GetString("Descripcion"),
                                        FK_IdUsuario = reader.GetInt32("FK_IdUsuario"),
                                        FK_IdSistema = reader.GetInt32("FK_IdSistema")
                                    });
                                }
                            }
                        }
                    }

                    return registros;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al obtener bitácoras filtradas: {ex.Message}");
                }
        }


    }


}

