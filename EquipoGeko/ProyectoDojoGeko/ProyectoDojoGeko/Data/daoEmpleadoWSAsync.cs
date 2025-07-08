using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoEmpleadoWSAsync
    {
        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoEmpleadoWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de empleados
        public async Task<List<EmpleadoViewModel>> ObtenerEmpleadoAsync()
        {
            // Declaración de la lista de Empleado
            var empleados = new List<EmpleadoViewModel>();

            // Nombre del procedimiento almacenado que se va a ejecutar
            string procedure = "sp_ListarEmpleados";

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

                        // Mientras haya registros, los lee y los agrega a la lista de empleados
                        while (await reader.ReadAsync())
                        {

                            // Crea un nuevo objeto EmpleadoViewModel y lo llena con los datos del lector
                            empleados.Add(new EmpleadoViewModel
                            {

                                // Asigna los valores de las columnas del lector a las propiedades del modelo
                                IdEmpleado = reader.GetInt32(reader.GetOrdinal("IdEmpleado")),
                                DPI = reader.GetString(reader.GetOrdinal("DPI")),
                                NombreEmpleado = reader.GetString(reader.GetOrdinal("NombreEmpleado")),
                                ApellidoEmpleado = reader.GetString(reader.GetOrdinal("ApellidoEmpleado")),
                                CorreoPersonal = reader.GetString(reader.GetOrdinal("CorreoPersonal")),
                                CorreoInstitucional = reader.GetString(reader.GetOrdinal("CorreoInstitucional")),
                                FechaIngreso = reader.GetDateTime(reader.GetOrdinal("FechaIngreso")),
                                FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                                Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                                NIT = reader.GetString(reader.GetOrdinal("NIT")),
                                Genero = reader.GetString(reader.GetOrdinal("Genero")),
                                Salario = reader.GetDecimal(reader.GetOrdinal("Salario")),
                                Estado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                            });
                        }
                    }
                }
            }
            // Devuelve la lista de empleados obtenida
            return empleados;
        }

        // Método para buscar un Empleado por su ID
        public async Task<EmpleadoViewModel> ObtenerEmpleadoPorIdAsync(int Id)
        {
            string procedure = "sp_ListarEmpleadoId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdEmpleado", Id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new EmpleadoViewModel
                            {
                                IdEmpleado = reader.GetInt32(reader.GetOrdinal("IdEmpleado")),
                                DPI = reader.GetString(reader.GetOrdinal("DPI")),
                                NombreEmpleado = reader.GetString(reader.GetOrdinal("NombreEmpleado")),
                                ApellidoEmpleado = reader.GetString(reader.GetOrdinal("ApellidoEmpleado")),
                                CorreoPersonal = reader.GetString(reader.GetOrdinal("CorreoPersonal")),
                                CorreoInstitucional = reader.GetString(reader.GetOrdinal("CorreoInstitucional")),
                                FechaIngreso = reader.GetDateTime(reader.GetOrdinal("FechaIngreso")),
                                FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                                Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                                NIT = reader.GetString(reader.GetOrdinal("NIT")),
                                Genero = reader.GetString(reader.GetOrdinal("Genero")),
                                Salario = reader.GetDecimal(reader.GetOrdinal("Salario")),
                                Estado = reader.GetInt32(reader.GetOrdinal("FK_IdEstado"))
                            };
                        }
                    }
                }
            }

            return null;
        }


        // Método para agregar un nuevo empleado
        public async Task<int> InsertarEmpleadoAsync(EmpleadoViewModel empleado)
        {
            var parametros = new[]
            {
                new SqlParameter("@DPI", empleado.DPI),
                new SqlParameter("@NombreEmpleado", empleado.NombreEmpleado),
                new SqlParameter("@ApellidoEmpleado", empleado.ApellidoEmpleado),
                new SqlParameter("@CorreoPersonal", empleado.CorreoPersonal),
                new SqlParameter("@CorreoInstitucional", empleado.CorreoInstitucional),
                new SqlParameter("@FechaNacimiento", empleado.FechaNacimiento),
                new SqlParameter("@Telefono", empleado.Telefono),
                new SqlParameter("@NIT", empleado.NIT),
                new SqlParameter("@Genero", empleado.Genero),
                new SqlParameter("@Salario", empleado.Salario),
                new SqlParameter("@FK_IdEstado", empleado.Estado)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_InsertarEmpleado", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para actualizar un empleado existente
        public async Task<int> ActualizarEmpleadoAsync(EmpleadoViewModel empleado)
        {

            var datosActuales = ObtenerEmpleadoPorIdAsync(empleado.IdEmpleado);

            var dpi = datosActuales.Result.DPI;
            var fechaNacimiento = datosActuales.Result.FechaNacimiento;

            var parametros = new[]
            {
                new SqlParameter("@IdEmpleado", empleado.IdEmpleado),
                new SqlParameter("@DPI", dpi),
                new SqlParameter("@NombreEmpleado", empleado.NombreEmpleado),
                new SqlParameter("@ApellidoEmpleado", empleado.ApellidoEmpleado),
                new SqlParameter("@CorreoPersonal", empleado.CorreoPersonal),
                new SqlParameter("@CorreoInstitucional", empleado.CorreoInstitucional),
                new SqlParameter("@FechaNacimiento", fechaNacimiento),
                new SqlParameter("@Telefono", empleado.Telefono),
                new SqlParameter("@NIT", empleado.NIT),
                new SqlParameter("@Genero", empleado.Genero),
                new SqlParameter("@Salario", empleado.Salario),
                new SqlParameter("@FK_IdEstado", empleado.Estado)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarEmpleado", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para eliminar un empleado por su ID
        public async Task<int> EliminarEmpleadoAsync(int Id)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdEmpleado", Id)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_EliminarEmpleado", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

    }

}
