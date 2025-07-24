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
                                TipoContrato = reader.IsDBNull(reader.GetOrdinal("TipoContrato")) ? "" : reader.GetString(reader.GetOrdinal("TipoContrato")),
                                Pais = reader.IsDBNull(reader.GetOrdinal("Pais")) ? "" : reader.GetString(reader.GetOrdinal("Pais")),
                                Departamento = reader.IsDBNull(reader.GetOrdinal("Departamento")) ? "" : reader.GetString(reader.GetOrdinal("Departamento")),
                                Municipio = reader.IsDBNull(reader.GetOrdinal("Municipio")) ? "" : reader.GetString(reader.GetOrdinal("Municipio")),
                                Direccion = reader.IsDBNull(reader.GetOrdinal("Direccion")) ? "" : reader.GetString(reader.GetOrdinal("Direccion")),
                                Puesto = reader.IsDBNull(reader.GetOrdinal("Puesto")) ? "" : reader.GetString(reader.GetOrdinal("Puesto")),
                                CodigoEmpleado = reader.IsDBNull(reader.GetOrdinal("Codigo")) ? "" : reader.GetString(reader.GetOrdinal("Codigo")),
                                DPI = reader.IsDBNull(reader.GetOrdinal("DPI")) ? "" : reader.GetString(reader.GetOrdinal("DPI")),
                                Pasaporte = reader.IsDBNull(reader.GetOrdinal("Pasaporte")) ? "" : reader.GetString(reader.GetOrdinal("Pasaporte")),
                                NombresEmpleado = reader.IsDBNull(reader.GetOrdinal("NombresEmpleado")) ? "" : reader.GetString(reader.GetOrdinal("NombresEmpleado")),
                                ApellidosEmpleado = reader.IsDBNull(reader.GetOrdinal("ApellidosEmpleado")) ? "" : reader.GetString(reader.GetOrdinal("ApellidosEmpleado")),
                                CorreoPersonal = reader.IsDBNull(reader.GetOrdinal("CorreoPersonal")) ? "" : reader.GetString(reader.GetOrdinal("CorreoPersonal")),
                                CorreoInstitucional = reader.IsDBNull(reader.GetOrdinal("CorreoInstitucional")) ? "" : reader.GetString(reader.GetOrdinal("CorreoInstitucional")),
                                FechaIngreso = reader.GetDateTime(reader.GetOrdinal("FechaIngreso")),
                                FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                                Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? "" : reader.GetString(reader.GetOrdinal("Telefono")),
                                NIT = reader.IsDBNull(reader.GetOrdinal("NIT")) ? "" : reader.GetString(reader.GetOrdinal("NIT")),
                                Genero = reader.IsDBNull(reader.GetOrdinal("Genero")) ? "" : reader.GetString(reader.GetOrdinal("Genero")),
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
                                TipoContrato = reader.IsDBNull(reader.GetOrdinal("TipoContrato")) ? "" : reader.GetString(reader.GetOrdinal("TipoContrato")),
                                Pais = reader.IsDBNull(reader.GetOrdinal("Pais")) ? "" : reader.GetString(reader.GetOrdinal("Pais")),
                                Departamento = reader.IsDBNull(reader.GetOrdinal("Departamento")) ? "" : reader.GetString(reader.GetOrdinal("Departamento")),
                                Municipio = reader.IsDBNull(reader.GetOrdinal("Municipio")) ? "" : reader.GetString(reader.GetOrdinal("Municipio")),
                                Direccion = reader.IsDBNull(reader.GetOrdinal("Direccion")) ? "" : reader.GetString(reader.GetOrdinal("Direccion")),
                                Puesto = reader.IsDBNull(reader.GetOrdinal("Puesto")) ? "" : reader.GetString(reader.GetOrdinal("Puesto")),
                                CodigoEmpleado = reader.IsDBNull(reader.GetOrdinal("Codigo")) ? "" : reader.GetString(reader.GetOrdinal("Codigo")),
                                DPI = reader.IsDBNull(reader.GetOrdinal("DPI")) ? "" : reader.GetString(reader.GetOrdinal("DPI")),
                                Pasaporte = reader.IsDBNull(reader.GetOrdinal("Pasaporte")) ? "" : reader.GetString(reader.GetOrdinal("Pasaporte")),
                                NombresEmpleado = reader.IsDBNull(reader.GetOrdinal("NombresEmpleado")) ? "" : reader.GetString(reader.GetOrdinal("NombresEmpleado")),
                                ApellidosEmpleado = reader.IsDBNull(reader.GetOrdinal("ApellidosEmpleado")) ? "" : reader.GetString(reader.GetOrdinal("ApellidosEmpleado")),
                                CorreoPersonal = reader.IsDBNull(reader.GetOrdinal("CorreoPersonal")) ? "" : reader.GetString(reader.GetOrdinal("CorreoPersonal")),
                                CorreoInstitucional = reader.IsDBNull(reader.GetOrdinal("CorreoInstitucional")) ? "" : reader.GetString(reader.GetOrdinal("CorreoInstitucional")),
                                FechaIngreso = reader.GetDateTime(reader.GetOrdinal("FechaIngreso")),
                                FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                                Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? "" : reader.GetString(reader.GetOrdinal("Telefono")),
                                NIT = reader.IsDBNull(reader.GetOrdinal("NIT")) ? "" : reader.GetString(reader.GetOrdinal("NIT")),
                                Genero = reader.IsDBNull(reader.GetOrdinal("Genero")) ? "" : reader.GetString(reader.GetOrdinal("Genero")),
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
            // DEBUG: Registrar los valores que se van a insertar
            var debugInfo = $"DEBUG INSERTAR EMPLEADO: Pais={empleado.Pais}, DPI={empleado.DPI ?? "NULL"}, Pasaporte={empleado.Pasaporte ?? "NULL"}, Salario={empleado.Salario}, FechaIngreso={empleado.FechaIngreso:yyyy-MM-dd}";
            System.Diagnostics.Debug.WriteLine(debugInfo);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_InsertarEmpleado", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TipoContrato", empleado.TipoContrato));
                    cmd.Parameters.Add(new SqlParameter("@Pais", empleado.Pais));
                    cmd.Parameters.Add(new SqlParameter("@Departamento", empleado.Departamento));
                    cmd.Parameters.Add(new SqlParameter("@Municipio", empleado.Municipio));
                    cmd.Parameters.Add(new SqlParameter("@Direccion", empleado.Direccion));
                    cmd.Parameters.Add(new SqlParameter("@Puesto", empleado.Puesto));
                    cmd.Parameters.Add(new SqlParameter("@Codigo", empleado.CodigoEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@DPI", string.IsNullOrEmpty(empleado.DPI) ? DBNull.Value : empleado.DPI));
                    cmd.Parameters.Add(new SqlParameter("@Pasaporte", string.IsNullOrEmpty(empleado.Pasaporte) ? DBNull.Value : empleado.Pasaporte));
                    cmd.Parameters.Add(new SqlParameter("@NombresEmpleado", empleado.NombresEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@ApellidosEmpleado", empleado.ApellidosEmpleado));
                    cmd.Parameters.Add(new SqlParameter("@CorreoPersonal", empleado.CorreoPersonal));
                    cmd.Parameters.Add(new SqlParameter("@CorreoInstitucional", empleado.CorreoInstitucional));
                    cmd.Parameters.Add(new SqlParameter("@FechaIngreso", empleado.FechaIngreso));
                    cmd.Parameters.Add(new SqlParameter("@FechaNacimiento", empleado.FechaNacimiento));
                    cmd.Parameters.Add(new SqlParameter("@Telefono", empleado.Telefono));
                    cmd.Parameters.Add(new SqlParameter("@NIT", string.IsNullOrEmpty(empleado.NIT) ? DBNull.Value : empleado.NIT));
                    cmd.Parameters.Add(new SqlParameter("@Genero", string.IsNullOrEmpty(empleado.Genero) ? DBNull.Value : empleado.Genero));
                    cmd.Parameters.Add(new SqlParameter("@Salario", empleado.Salario));
                    cmd.Parameters.Add(new SqlParameter("@FK_IdEstado", empleado.Estado));

                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para actualizar un empleado existente
        public async Task<int> ActualizarEmpleadoAsync(EmpleadoViewModel empleado)
        {

            var datosActuales = await ObtenerEmpleadoPorIdAsync(empleado.IdEmpleado);

            var dpi = datosActuales.DPI;
            var fechaNacimiento = datosActuales.FechaNacimiento;

            var parametros = new[]
            {
                new SqlParameter("@IdEmpleado", empleado.IdEmpleado),
                new SqlParameter("@TipoContrato", empleado.TipoContrato),
                new SqlParameter("@Pais", empleado.Pais),
                new SqlParameter("@Departamento", empleado.Departamento),
                new SqlParameter("@Municipio", empleado.Municipio),
                new SqlParameter("@Direccion", empleado.Direccion),
                new SqlParameter("@Puesto", empleado.Puesto),
                new SqlParameter("@Codigo", empleado.CodigoEmpleado),
                new SqlParameter("@DPI", dpi),
                new SqlParameter("@Pasaporte", empleado.Pasaporte),
                new SqlParameter("@NombresEmpleado", empleado.NombresEmpleado),
                new SqlParameter("@ApellidosEmpleado", empleado.ApellidosEmpleado),
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
