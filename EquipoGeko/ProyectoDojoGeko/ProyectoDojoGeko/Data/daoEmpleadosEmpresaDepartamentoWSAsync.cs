using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models.Empleados;

namespace ProyectoDojoGeko.Data
{
    public class daoEmpleadosEmpresaDepartamentoWSAsync
    {

        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoEmpleadosEmpresaDepartamentoWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de empleados de una empresa
        public async Task<List<EmpleadosDepartamentoViewModel>> ObtenerEmpleadosDepartamentoAsync()
        {
            // Declaración de la lista de empleados
            var empleados = new List<EmpleadosDepartamentoViewModel>();

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
                            empleados.Add(new EmpleadosDepartamentoViewModel
                            {
                                IdEmpleadosDepartamento = reader.GetInt32(reader.GetOrdinal("IdEmpleadoDepartamento")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado")),
                                FK_IdDepartamento = reader.GetInt32(reader.GetOrdinal("FK_IdDepartamento"))
                            });
                        }
                    }
                }
            }
            return empleados;
        }

        // Método para obtener un empleado de una empresa por su ID
        public async Task<List<EmpleadosDepartamentoViewModel>> ObtenerEmpleadoDepartamentoPorIdAsync(int idEmpleadoDepartamento)
        {

            // Declaración de la lista de empleados
            var empleados = new List<EmpleadosDepartamentoViewModel>();

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
                    cmd.Parameters.AddWithValue("@IdEmpleadoEmpresa", idEmpleadoDepartamento);

                    // Ejecuta el comando y obtiene un lector de datos asíncrono
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        // Recorre los resultados en caso de que haya más de un registro
                        while (await reader.ReadAsync())
                        {
                            empleados.Add(new EmpleadosDepartamentoViewModel
                            {
                                IdEmpleadosDepartamento = reader.GetInt32(reader.GetOrdinal("IdEmpleadoDepartamento")),
                                FK_IdEmpleado = reader.GetInt32(reader.GetOrdinal("FK_IdEmpleado")),
                                FK_IdDepartamento = reader.GetInt32(reader.GetOrdinal("FK_IdDepartamento"))
                            });
                        }

                    }
                }
            }

            return empleados; // Devuelve la lista de empleados encontrados

        }

        // Método para agregar una nueva asignación de un empleado a una empresa(s)
        public async Task<int> InsertarEmpleadoEmpresaAsync(EmpleadosEmpresaViewModel empleado)
        { 
           // Nombre del procedimiento almacenado que se va a ejecutar  
            string procedure = "sp_InsertarEmpleadosEmpresa";

            // Verifica que los parámetros necesarios estén presentes  
            var parametros = new[]
            {
               new SqlParameter("@FK_IdEmpresa", empleado.FK_IdEmpresa),
               new SqlParameter("@FK_IdEmpleado", empleado.FK_IdEmpleado)
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

        // Método para agregar una nueva asignación de un empleado a un departamento(s)  
        public async Task<int> InsertarEmpleadoDepartamentoAsync(EmpleadosDepartamentoViewModel empleadoDepartamento)
        {
            // Nombre del procedimiento almacenado que se va a ejecutar  
            string procedure = "sp_InsertarEmpleadosDepartamento";

            // Verifica que los parámetros necesarios estén presentes  
            var parametros = new[]
            {
               new SqlParameter("@FK_IdEmpleado", empleadoDepartamento.FK_IdEmpleado),
               new SqlParameter("@FK_IdDepartamento", empleadoDepartamento.FK_IdDepartamento)
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
        public async Task<int> ActualizarEmpleadoDepartamentoAsync(EmpleadosDepartamentoViewModel empleadoDepartamento)
        {
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ActualizarEmpleadoDepartamento";

            // Verifica que los parámetros necesarios estén presentes  
            var parametros = new[]
            {
               new SqlParameter("@IdEmpleadoDepartamento", empleadoDepartamento.IdEmpleadosDepartamento),
               new SqlParameter("@FK_IdEmpleado", empleadoDepartamento.FK_IdEmpleado),
               new SqlParameter("@FK_IdDepartamento", empleadoDepartamento.FK_IdDepartamento)
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
        public async Task<int> EliminarEmpleadoDepartamentoAsync(int idEmpleadoDepartamento)
        {
            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_EliminarEmpleadoDepartamento";

            // Verifica que los parámetros necesarios estén presentes  
            var parametros = new[]
            {
               new SqlParameter("@IdEmpleadoDepartamento", idEmpleadoDepartamento)
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
