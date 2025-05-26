using Microsoft.Data.SqlClient;
using RetoCRUDEmpleados.Models;

namespace RetoCRUDEmpleados.Data
{
    public class daoEmpleadoAsync
    {

        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoEmpleadoAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener todos los empleados
        public async Task<List<EmpleadoViewModel>> ObtenerEmpleadosAsync()
        {

            // Creamos una lista para almacenar los empleados
            var Sistemas = new List<EmpleadoViewModel>();

            // Creamos el quuery para obtener todos los empleados
            string query = "SELECT * FROM Empleados";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Abrimos nuestra conexión
                await connection.OpenAsync();

                // Creamos un SqlCommand para ejecutar el query
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    // Ejecutamos el comando asincrónicamente
                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        // Leemos los datos de cada fila
                        while (await reader.ReadAsync())
                        {

                            // Leemos los datos de cada fila
                            // y los agregamos a la lista de sistemas
                            // usando el modelo SistemaViewModel
                            Sistemas.Add(new EmpleadoViewModel()
                            {
                                IdEmpleado = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                FechaNacimiento = reader.GetDateTime(3),
                                FechaIngreso = reader.GetDateTime(4),
                                Puesto = reader.GetString(5),
                                SalarioBase = reader.GetDecimal(6),
                                Activo = reader.GetBoolean(7)
                            });

                        }

                    }


                }

            }

            // Devolvemos la lista de empleados
            return Sistemas;

        }

        // Método para insertar un nuevo empleado
        public async Task InsertarEmpleadoAsync(EmpleadoViewModel empleado)
        {
            // Creamos el query para insertar un nuevo empleado
            string query = "INSERT INTO Empleados (Nombre, Apellido, FechaNacimiento, FechaIngreso, Puesto, SalarioBase, Activo) VALUES (@Nombre, @Apellido, @FechaNacimiento, @FechaIngreso, @Puesto, @SalarioBase, @Activo)";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Abrimos nuestra conexión
                await connection.OpenAsync();
                // Creamos un SqlCommand para ejecutar el query
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                    command.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                    command.Parameters.AddWithValue("@FechaNacimiento", empleado.FechaNacimiento);
                    command.Parameters.AddWithValue("@FechaIngreso", empleado.FechaIngreso);
                    command.Parameters.AddWithValue("@Puesto", empleado.Puesto);
                    command.Parameters.AddWithValue("@SalarioBase", empleado.SalarioBase);
                    command.Parameters.AddWithValue("@Activo", empleado.Activo);
                    await command.ExecuteNonQueryAsync();

                }
            }
        }

        // Método para buscar un sistema por su ID
        public async Task<EmpleadoViewModel> ObtenerEmpleadoPorIdAsync(int Id)
        {

            EmpleadoViewModel empleado = null;

            string sql = "SELECT * FROM Empleados WHERE EmpleadoID = @Id";

            using (SqlConnection cnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    await cnn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            empleado = new EmpleadoViewModel
                            {
                                IdEmpleado = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                FechaNacimiento = reader.GetDateTime(3),
                                FechaIngreso = reader.GetDateTime(4),
                                Puesto = reader.GetString(5),
                                SalarioBase = reader.GetDecimal(6),
                                Activo = reader.GetBoolean(7)
                            };

                        }
                    }
                }
            }
            return empleado;
        }

        // Método para actualizar un empleado existente 
        public async Task ActualizarEmpleadoAsync(EmpleadoViewModel empleado)
        {
            // Creamos el query para actualizar un empleado
            string query = "UPDATE Empleados SET Nombre = @Nombre, Apellido = @Apellido, FechaNacimiento = @FechaNacimiento, FechaIngreso = @FechaIngreso, Puesto = @Puesto, SalarioBase = @SalarioBase, Activo = @Activo WHERE EmpleadoID = @Id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Abrimos nuestra conexión
                await connection.OpenAsync();
                // Creamos un SqlCommand para ejecutar el query
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregamos los parámetros al comando
                    command.Parameters.AddWithValue("@Id", empleado.IdEmpleado);
                    command.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                    command.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                    command.Parameters.AddWithValue("@FechaNacimiento", empleado.FechaNacimiento);
                    command.Parameters.AddWithValue("@FechaIngreso", empleado.FechaIngreso);
                    command.Parameters.AddWithValue("@Puesto", empleado.Puesto);
                    command.Parameters.AddWithValue("@SalarioBase", empleado.SalarioBase);
                    command.Parameters.AddWithValue("@Activo", empleado.Activo);
                    // Ejecutamos el comando asincrónicamente
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        //Método para eliminar un empleado por su ID
        public async Task EliminarEmpleadoAsync(int Id)
        {

            // Creamos el query para eliminar un empleado
            string query = "DELETE FROM Empleados WHERE EmpleadoID = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                // Abrimos nuestra conexión
                await connection.OpenAsync();

                // Creamos un SqlCommand para ejecutar el query
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    // Agregamos el parámetro al comando
                    command.Parameters.AddWithValue("@Id", Id);

                    // Ejecutamos el comando asincrónicamente
                    await command.ExecuteNonQueryAsync();

                }
            }

        }


    }
}
