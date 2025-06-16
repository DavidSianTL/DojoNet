using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoEmpleadosEmpresaWSAsync
    {

        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoEmpleadosEmpresaWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de empleados de una empresa
        public async Task<List<EmpleadosEmpresaViewModel>> ObtenerEmpleadosEmpresaAsync()
        {
            // Declaración de la lista de empleados
            var empleados = new List<EmpleadosEmpresaViewModel>();

            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ListarEmpleadosEmpresa";

            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await conn.OpenAsync();

                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Ejecuta el comando y obtiene un lector de datos asíncrono
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Recorre los resultados y los agrega a la lista de empleados
                        while (await reader.ReadAsync())
                        {
                            empleados.Add(new EmpleadosEmpresaViewModel
                            {
                                IdEmpleadoEmpresa = reader.GetInt32(reader.GetOrdinal("IdEmpleadoEmpresa")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado")),
                                FK_IdEmpresa = reader.GetInt32(reader.GetOrdinal("FK_IdEmpresa"))
                            });
                        }
                    }
                }
            }
            return empleados;
        }

        // Método para obtener un empleado de una empresa por su ID
        public async Task<List<EmpleadosEmpresaViewModel>> ObtenerEmpleadoEmpresaPorIdAsync(int idEmpleadoEmpresa)
        {

            // Declaración de la lista de empleados
            var empleados = new List<EmpleadosEmpresaViewModel>();

            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ObtenerEmpleadoEmpresaPorId";

            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await conn.OpenAsync();

                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agrega el parámetro de entrada para el ID del empleado de la empresa
                    cmd.Parameters.AddWithValue("@IdEmpleadoEmpresa", idEmpleadoEmpresa);

                    // Ejecuta el comando y obtiene un lector de datos asíncrono
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        // Recorre los resultados en caso de que haya más de un registro
                        while (await reader.ReadAsync())
                        {
                            empleados.Add(new EmpleadosEmpresaViewModel
                            {
                                IdEmpleadoEmpresa = reader.GetInt32(reader.GetOrdinal("IdEmpleadoEmpresa")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado")),
                                FK_IdEmpresa = reader.GetInt32(reader.GetOrdinal("FK_IdEmpresa"))
                            });
                        }

                    }
                }
            }

            return empleados; // Devuelve la lista de empleados encontrados

        }

        // Método para obtener un empleado de una empresa por su ID de empleado
        public async Task<List<EmpleadosEmpresaViewModel>> ObtenerEmpleadoEmpresaPorIdEmpleadoAsync(int idEmpleado)
        {
            // Declaración de la lista de empleados
            var empleados = new List<EmpleadosEmpresaViewModel>();
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ListarEmpleadoEmpresaPorEmpleado";
            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await conn.OpenAsync();
                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Agrega el parámetro de entrada para el ID del empleado
                    cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                    // Ejecuta el comando y obtiene un lector de datos asíncrono
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Recorre los resultados y los agrega a la lista de empleados
                        while (await reader.ReadAsync())
                        {
                            empleados.Add(new EmpleadosEmpresaViewModel
                            {
                                IdEmpleadoEmpresa = reader.GetInt32(reader.GetOrdinal("IdEmpleadoEmpresa")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado")),
                                FK_IdEmpresa = reader.GetInt32(reader.GetOrdinal("FK_IdEmpresa"))
                            });
                        }
                    }
                }
            }
            return empleados; // Devuelve la lista de empleados
        }

        // Método para obtener un empleado de una empresa por su ID de empresa
        public async Task<List<EmpleadosEmpresaViewModel>> ObtenerEmpleadoEmpresaPorIdEmpresaAsync(int idEmpresa)
        {
            // Declaración de la lista de empleados
            var empleados = new List<EmpleadosEmpresaViewModel>();
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ListarEmpleadoEmpresaPorEmpresa";
            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await conn.OpenAsync();
                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Agrega el parámetro de entrada para el ID de la empresa
                    cmd.Parameters.AddWithValue("@IdEmpresa", idEmpresa);
                    // Ejecuta el comando y obtiene un lector de datos asíncrono
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Recorre los resultados y los agrega a la lista de empleados
                        while (await reader.ReadAsync())
                        {
                            empleados.Add(new EmpleadosEmpresaViewModel
                            {
                                IdEmpleadoEmpresa = reader.GetInt32(reader.GetOrdinal("IdEmpleadoEmpresa")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado")),
                                FK_IdEmpresa = reader.GetInt32(reader.GetOrdinal("FK_IdEmpresa"))
                            });
                        }
                    }
                }
            }
            return empleados; // Devuelve la lista de empleados
        }

        // Método para agregar una nueva asignación de un empleado a una empresa  
        public async Task<int> InsertarEmpleadoEmpresaAsync(EmpleadosEmpresaViewModel empleadoEmpresa)
        {
            // Nombre del procedimiento almacenado que se va a ejecutar  
            string procedure = "sp_InsertarEmpleadosEmpresa";

            // Verifica que los parámetros necesarios estén presentes  
            var parametros = new[]
            {
               new SqlParameter("@FK_IdEmpleado", empleadoEmpresa.FK_IdEmpleado),
               new SqlParameter("@FK_IdEmpresa", empleadoEmpresa.FK_IdEmpresa)
            };

            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await conn.OpenAsync();

                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {

                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agrega los parámetros necesarios para el procedimiento almacenado
                    cmd.Parameters.AddRange(parametros);

                    // Ejecuta el comando y obtiene el número de filas afectadas
                    return await cmd.ExecuteNonQueryAsync();
                }
            };

        }

        // Método para actualizar una asignación de un empleado a una empresa
        public async Task<int> ActualizarEmpleadoEmpresaAsync(EmpleadosEmpresaViewModel empleadoEmpresa)
        {
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ActualizarEmpleadoEmpresa";

            // Verifica que los parámetros necesarios estén presentes  
            var parametros = new[]
            {
               new SqlParameter("@IdEmpleadoEmpresa", empleadoEmpresa.IdEmpleadoEmpresa),
               new SqlParameter("@FK_IdEmpleado", empleadoEmpresa.FK_IdEmpleado),
               new SqlParameter("@FK_IdEmpresa", empleadoEmpresa.FK_IdEmpresa)
            };

            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await conn.OpenAsync();
                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Agrega los parámetros necesarios para el procedimiento almacenado
                    cmd.Parameters.AddRange(parametros);
                    // Ejecuta el comando y obtiene el número de filas afectadas
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para eliminar una asignación de un empleado a una empresa
        public async Task<int> EliminarEmpleadoEmpresaAsync(int idEmpleadoEmpresa)
        {
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_EliminarEmpleadoEmpresa";

            // Verifica que los parámetros necesarios estén presentes  
            var parametros = new[]
            {
               new SqlParameter("@IdEmpleadoEmpresa", idEmpleadoEmpresa)
            };

            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Abre la conexión de forma asíncrona
                await conn.OpenAsync();
                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {

                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agrega el parámetro necesario para el procedimiento almacenado
                    cmd.Parameters.AddRange(parametros);

                    // Ejecuta el comando y obtiene el número de filas afectadas
                    return await cmd.ExecuteNonQueryAsync();

                }
            }
        }


    }
}
