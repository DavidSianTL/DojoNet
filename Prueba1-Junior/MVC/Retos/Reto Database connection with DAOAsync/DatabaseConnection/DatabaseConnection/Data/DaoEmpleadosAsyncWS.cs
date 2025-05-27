using DatabaseConnection.Models;
using DatabaseConnection.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DatabaseConnection.Data
{
	public interface IDaoEmpleadosAsyncWS
	{
        Task<bool> DeleteEmpleadoAsync(int empleadoId);
        Task<List<Empleado>> GetEmpleadosListAsync();
	}

	public class DaoEmpleadosAsyncWS : IDaoEmpleadosAsyncWS
	{

		private readonly IConnectionServiceAsync _connectionAsync;

		public DaoEmpleadosAsyncWS( IConnectionServiceAsync connectionAsync)
		{
			_connectionAsync = connectionAsync;
		}




        #region Helpers methods

		// mapper method to convert DataRow to Empleado bject
		private Empleado MapDataTableToEmpleados(DataRow row)
		{
			return
			new Empleado
			{
				EmpleadoID = Convert.ToInt32(row["EmpleadoID"]),
				Nombre = row["Nombre"].ToString() ?? "unknow",
				Apellido = row["Apellido"].ToString() ?? "unknow",
				FechaIngreso = Convert.ToDateTime(row["FechaIngreso"]),
				Puesto = row["Puesto"].ToString() ?? "unknow",
				SalarioBase = Convert.ToDecimal(row["SalarioBase"])
			};

        }
        #endregion




        public async Task<List<Empleado>> GetEmpleadosListAsync()
		{

			var empleadosList = new List<Empleado>();

			try
			{
				var query = "SELECT EmpleadoID, Nombre, Apellido, FechaIngreso, Puesto, SalarioBase FROM Empleados";

				var ds = await _connectionAsync.ExecuteSelectAsync(query);

				if (ds == null || ds.Tables.Count == 0) return empleadosList;

				foreach (DataRow row in ds.Tables[0].Rows)
				{
					empleadosList.Add(
						MapDataTableToEmpleados(row)

					 );


				}

				
			}catch (Exception ex)
			{
				Console.WriteLine("Error al obtener la lista de empleados: " + ex.Message);
            }

			return empleadosList;

		}



		public async Task<bool> DeleteEmpleadoAsync( int empleadoId)
		{
			try
			{
				var query = $"DELETE FROM Empleados WHERE EmpleadoID = @EmpleadoID";

				var parameters = new SqlParameter[]
				{
					new SqlParameter("@EmpleadoID", empleadoId)
                };

                if(await _connectionAsync.ExecuteCommandAsync(query, parameters)) return true;

                // Si llegamos hasta aquí, la operación fue exitosa
                
            }
			catch (Exception ex)
			{
                Console.WriteLine($"Error al eliminar el empleado {ex}");
			}

				return false;
        }









	}
}
