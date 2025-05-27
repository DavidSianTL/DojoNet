using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DatabaseConnection.Models;
using DatabaseConnection.Services;
using Microsoft.Data.SqlClient;

namespace DatabaseConnection.Data
{
	public interface IDaoEmpleadosAsync
	{
        Task<bool>	DeleteEmpleadoAsync(int Id);
        Task<List<Empleado>> GetEmpleadosAsync();
        Task<bool> InsertEmpleadoAsync(Empleado empleado);
        Task<bool> UpdateEmpleadoAsync(Empleado empleado);
    }





	public class DaoEmpleadosAsync : IDaoEmpleadosAsync
	{


		private readonly IConnectionServiceAsync _connection;
		public DaoEmpleadosAsync(IConnectionServiceAsync connection)
		{
			_connection = connection;
		}


		public async Task<List<Empleado>> GetEmpleadosAsync()
		{

			var empleadosList = new List<Empleado>();


			var query = $"SELECT EmpleadoID, Nombre, Apellido, Puesto, SalarioBase, FechaNacimiento, FechaIngreso, Activo FROM Empleados";


			using (SqlConnection conn = new SqlConnection(_connection.GetConnection()))
			{
				await conn.OpenAsync();

				using (SqlCommand cmd = new SqlCommand(query, conn))
				{
					using (var reader = await cmd.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							empleadosList.Add(new Empleado
							{
								EmpleadoID = reader.GetInt32(0),
								Nombre = reader.GetString(1),
								Apellido = reader.GetString(2),
								Puesto = reader.GetString(3),
								SalarioBase = reader.GetDecimal(4),
								FechaNacimiento = reader.GetDateTime(5),
								FechaIngreso = reader.GetDateTime(6),
								Activo = reader.GetBoolean(7)

                            });
						}

					}
				}


			}
			return empleadosList;


		}

		
		public async Task<bool> InsertEmpleadoAsync(Empleado empleado)
		{
			string query = $@"

				INSERT INTO Empleados
					(Nombre, Apellido, Puesto, SalarioBase, FechaNacimiento, FechaIngreso)
				VALUES 
					(@Nombre, @Apellido, @Puesto, @SalarioBase, @FechaNacimiento, @FechaIngreso

			)";


			using (var cnn = new SqlConnection(_connection.GetConnection()))
			{
				using (var cmd = new SqlCommand(query, cnn))
				{
					cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
					cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
					cmd.Parameters.AddWithValue("@Puesto", empleado.Puesto);
					cmd.Parameters.AddWithValue("@SalarioBase", empleado.SalarioBase);
					cmd.Parameters.AddWithValue("@FechaNacimiento", empleado.FechaNacimiento);
					cmd.Parameters.AddWithValue("@FechaIngreso", empleado.FechaIngreso);
					cmd.Parameters.AddWithValue("@Activo", empleado.Activo);


                    await cnn.OpenAsync();
					await cmd.ExecuteNonQueryAsync();
				}


			}


			return true;


		}



		public async Task<bool> UpdateEmpleadoAsync(Empleado empleado)
		{
			string query = $@"
				UPDATE Empleados
				SET 
					Nombre = @Nombre,
					Apellido = @Apellido,
					Puesto = @Puesto,
					SalarioBase = @SalarioBase,
					FechaNacimiento = @FechaNacimiento,
					FechaIngreso = @FechaIngreso,
					Activo = @Activo
				WHERE EmpleadoID = @EmpleadoID";

			using (var cnn = new SqlConnection(_connection.GetConnection()))
			{
				using (var cmd = new SqlCommand(query, cnn))
				{
					cmd.Parameters.AddWithValue("@EmpleadoID", empleado.EmpleadoID);
					cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
					cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
					cmd.Parameters.AddWithValue("@Puesto", empleado.Puesto);
					cmd.Parameters.AddWithValue("@SalarioBase", empleado.SalarioBase);
					cmd.Parameters.AddWithValue("@FechaNacimiento", empleado.FechaNacimiento);
					cmd.Parameters.AddWithValue("@FechaIngreso", empleado.FechaIngreso);
					cmd.Parameters.AddWithValue("@Activo", empleado.Activo);
                    await cnn.OpenAsync();
					await cmd.ExecuteNonQueryAsync();
				}
			}
			return true;
        }






		public async Task<bool> DeleteEmpleadoAsync(int Id)
		{
			var empleados = await GetEmpleadosAsync();

			var empleado = empleados.FirstOrDefault(emp => emp.EmpleadoID == Id);

			if (empleado == null) return false;

			var  query = $@"
				
				DELETE FROM Empleados 
					WHERE EmpleadoId = @EmpleadoID

            ";

			using (var cnn = new SqlConnection(_connection.GetConnection()))
			{

				using (var cmd = new SqlCommand(query, cnn))
				{
					cmd.Parameters.AddWithValue("@EmpleadoID", empleado.EmpleadoID);

					await cnn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

				}


			}


			return true;



        }






	}
}


		












