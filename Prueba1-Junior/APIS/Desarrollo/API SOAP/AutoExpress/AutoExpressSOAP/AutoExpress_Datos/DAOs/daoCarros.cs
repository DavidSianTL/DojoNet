using AutoExpress_Entidades;
using AutoExpress_Entidades.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExpress_Datos
{
	public class daoCarros
	{
		private readonly IDbConnectionService _dbConnectionService;
		public daoCarros(IDbConnectionService dbConnectionService)
		{
			_dbConnectionService = dbConnectionService;
		}



	#region GET


		public async Task<List<Carro>> GetCarrosAsync()
		{
			var dataSet = await _dbConnectionService.ExecuteStoredProcedureAsync("sp_GetCarros");
			var carros = new List<Carro>();
			if (dataSet.Tables.Count > 0)
			{
				foreach (DataRow row in dataSet.Tables[0].Rows)
				{
					var carro = new Carro
					{
						Id = Convert.ToInt32(row["id"]),
						Marca = row["marca"].ToString(),
						Modelo = row["modelo"].ToString(),
						Anio = Convert.ToInt32(row["anio"]),
						Precio = Convert.ToDecimal(row["precio"]),
						Disponible = Convert.ToBoolean(row["disponible"])
					};
					carros.Add(carro);
				}
				return carros;
			}

			return null;
		}


		public async Task<Carro> GetCarroByIdAsync(int id)
		{
			var parameters = new List<SqlParameter>
			{
				new SqlParameter("@id", id),
			};

			var dataSet = await _dbConnectionService.ExecuteStoredProcedureAsync("sp_GetCarroById", parameters);

			if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
			{
				var row = dataSet.Tables[0].Rows[0];
				var carro = new Carro
				{
					Id = Convert.ToInt32(row["id"]),
					Marca = row["marca"].ToString(),
					Modelo = row["modelo"].ToString(),
					Anio = Convert.ToInt32(row["anio"]),
					Precio = Convert.ToDecimal(row["precio"]),
					Disponible = Convert.ToBoolean(row["disponible"])
				};
				return carro;
			}

			return null;
		}


		#endregion


	#region INSERT, UPDATE, "DELETE"

		// INSERT
		public async Task<bool> AddCarroAsync(CarroRequestDTO carro)
		{
			try
			{
				var parameters = new List<SqlParameter>
				{
					new SqlParameter("@marca", carro.Marca),
					new SqlParameter("@modelo", carro.Modelo),
					new SqlParameter("@anio", carro.Anio),
					new SqlParameter("@precio", carro.Precio)
				};

				bool result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_InsertCarro", parameters);
				if (!result)
				{
					return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}

		}

		// UPDATE
		public async Task<bool> UpdateCarroAsync(Carro carro)
		{
			try
			{
				var parameters = new List<SqlParameter>
				{
					new SqlParameter("@id", carro.Id),
					new SqlParameter("@marca", carro.Marca),
					new SqlParameter("@modelo", carro.Modelo),
					new SqlParameter("@anio", carro.Anio),
					new SqlParameter("@precio", carro.Precio),
					new SqlParameter("@disponible", carro.Disponible)
				};

				var result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_EditCarro", parameters);
				if (!result) return false;

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		// DELETE
		public async Task<bool> DeleteCarroAsync(int id)
		{
			try
			{
				var parameters = new List<SqlParameter>()
				{
					new SqlParameter("@id", id)
				};

				var result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_DeleteCarro", parameters);
				if (!result)
				{
					return false;

				}

				return true;
			}
			catch (Exception ex)
			{
				
				return false;
			}
		}


	#endregion

	}
}
