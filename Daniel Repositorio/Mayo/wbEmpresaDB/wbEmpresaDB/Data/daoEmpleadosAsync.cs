using Microsoft.Data.SqlClient;
using wbEmpresaDB.Models;

namespace wbEmpresaDB.Data
{
    public class daoEmpleadosAsync
    {
        private readonly string _connectionString;

        public daoEmpleadosAsync(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Empleado>> ObtenerTodosAsync()
        {
            var lista = new List<Empleado>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Empleados", conn);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new Empleado
                {
                    EmpleadoID = Convert.ToInt32(reader["EmpleadoID"]),
                    Nombre = reader["Nombre"].ToString(),
                    Apellido = reader["Apellido"].ToString(),
                    FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                    FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                    Puesto = reader["Puesto"].ToString(),
                    SalarioBase = Convert.ToDecimal(reader["SalarioBase"]),
                    Activo = Convert.ToBoolean(reader["Activo"])
                });
            }

            return lista;
        }

        public async Task<Empleado?> ObtenerPorIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Empleados WHERE EmpleadoID = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Empleado
                {
                    EmpleadoID = Convert.ToInt32(reader["EmpleadoID"]),
                    Nombre = reader["Nombre"].ToString(),
                    Apellido = reader["Apellido"].ToString(),
                    FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                    FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                    Puesto = reader["Puesto"].ToString(),
                    SalarioBase = Convert.ToDecimal(reader["SalarioBase"]),
                    Activo = Convert.ToBoolean(reader["Activo"])
                };
            }

            return null;
        }

        public async Task CrearAsync(Empleado emp)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"INSERT INTO Empleados 
                (Nombre, Apellido, FechaNacimiento, FechaIngreso, Puesto, SalarioBase, Activo) 
                VALUES (@Nombre, @Apellido, @FechaNacimiento, @FechaIngreso, @Puesto, @SalarioBase, @Activo)", conn);

            cmd.Parameters.AddWithValue("@Nombre", emp.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", emp.Apellido);
            cmd.Parameters.AddWithValue("@FechaNacimiento", emp.FechaNacimiento);
            cmd.Parameters.AddWithValue("@FechaIngreso", emp.FechaIngreso);
            cmd.Parameters.AddWithValue("@Puesto", emp.Puesto);
            cmd.Parameters.AddWithValue("@SalarioBase", emp.SalarioBase);
            cmd.Parameters.AddWithValue("@Activo", emp.Activo);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ActualizarAsync(Empleado emp)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"UPDATE Empleados SET 
                Nombre = @Nombre, 
                Apellido = @Apellido, 
                FechaNacimiento = @FechaNacimiento, 
                FechaIngreso = @FechaIngreso, 
                Puesto = @Puesto, 
                SalarioBase = @SalarioBase, 
                Activo = @Activo 
                WHERE EmpleadoID = @EmpleadoID", conn);

            cmd.Parameters.AddWithValue("@EmpleadoID", emp.EmpleadoID);
            cmd.Parameters.AddWithValue("@Nombre", emp.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", emp.Apellido);
            cmd.Parameters.AddWithValue("@FechaNacimiento", emp.FechaNacimiento);
            cmd.Parameters.AddWithValue("@FechaIngreso", emp.FechaIngreso);
            cmd.Parameters.AddWithValue("@Puesto", emp.Puesto);
            cmd.Parameters.AddWithValue("@SalarioBase", emp.SalarioBase);
            cmd.Parameters.AddWithValue("@Activo", emp.Activo);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("DELETE FROM Empleados WHERE EmpleadoID = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
