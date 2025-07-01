using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExpress_Datos
{
	public interface IDbConnectionService
	{
		Task<DataSet> ExecuteStoredProcedureAsync(string sp);
		Task<DataSet> ExecuteStoredProcedureAsync(string sp, List<SqlParameter> parameters);
		Task<bool> ExecuteStoredProcedureNonQueryAsync(string sp, List<SqlParameter> parameters = null);
	}

	public class DbConnectionService : IDbConnectionService
	{
		
		private readonly string _connectionString = "Mi conexión";

		// Metodo para ejecutar un stored procedure que retorna datos pero no necesita parametros      
		public async Task<DataSet> ExecuteStoredProcedureAsync(string sp)
		{
			var ds = new DataSet();

			try
			{

				using (SqlConnection cnn = new SqlConnection(_connectionString))
				{ 
					await cnn.OpenAsync();
					SqlDataAdapter adapter = new SqlDataAdapter(sp, cnn);

					adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
					adapter.Fill(ds);
				}
			}
			catch (Exception ex)
			{
			
			}


			return ds;
		}

		// Metodo para ejecutar un stored procedure que retorna datos y necesita parametros
		public async Task<DataSet> ExecuteStoredProcedureAsync(string sp, List<SqlParameter> parameters)
		{
			var ds = new DataSet();
			try
			{
				using (SqlConnection cnn = new SqlConnection(_connectionString))
				{


					await cnn.OpenAsync();

					// Se crea un SqlDataAdapter para ejecutar el stored procedure
					SqlDataAdapter adapter = new SqlDataAdapter(sp, cnn);
					adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
					if (parameters != null && parameters.Count > 0)
					{
						adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
					}
					adapter.Fill(ds);

				}

			}
			catch (Exception ex)
			{
				
			}

			return ds;
		}

		// Metodo para ejecutar un stored procedure que no retorna datos y necesita parametros
		public async Task<bool> ExecuteStoredProcedureNonQueryAsync(string sp, List<SqlParameter> parameters = null)
		{
			try
			{
				using (SqlConnection cnn = new SqlConnection(_connectionString))
				{


					await cnn.OpenAsync();
					using (SqlCommand cmd = new SqlCommand(sp, cnn))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						if (parameters != null && parameters.Count > 0)
						{
							cmd.Parameters.AddRange(parameters.ToArray());
						}
						await cmd.ExecuteNonQueryAsync();
					}
				}

				return true;
			}
			catch (Exception ex)
			{
				
				return false;
			}
		}



	}
}
