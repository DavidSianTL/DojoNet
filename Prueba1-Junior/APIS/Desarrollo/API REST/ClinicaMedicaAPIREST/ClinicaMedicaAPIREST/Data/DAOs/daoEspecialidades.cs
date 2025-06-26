using ClinicaMedicaAPIREST.Data.DTO.EspecialidadesDTOs;
using ClinicaMedicaAPIREST.Models;
using ClinicaMedicaAPIREST.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicaMedicaAPIREST.Data.DAOs
{
	public class daoEspecialidades
	{
		private ILogger<daoEspecialidades> _logger;
		private readonly IDbConnectionService _dbConnectionService;

		public daoEspecialidades(IDbConnectionService dbConnectionService, ILogger<daoEspecialidades> logger)
		{
			_logger = logger;
			_dbConnectionService = dbConnectionService;
		}

        #region Metodos SELECT

        // Listar especialidades
		public async Task<List<Especialidad>?> GetEspecialidadesAsync()
		{
			var dataset = await _dbConnectionService.ExecuteStoredProcedureAsync("sp_GetEspecialidades");

			var especialidades = new List<Especialidad>();

			if (dataset.Tables.Count > 0)
			{
				foreach (DataRow row in dataset.Tables[0].Rows)
				{
					var especialidad = new Especialidad
					{
						Id = Convert.ToInt32(row["id"]),
						Nombre = row["nombre"].ToString() ?? String.Empty,
						estado = Convert.ToBoolean(row["estado"])
                    };
					especialidades.Add(especialidad);
				}

				return especialidades;
			}
			return null;
		}
		
		// Listar especialidades por Id
		public async Task<Especialidad?> GetEspecialidadByIdAsync(int Id)
		{
			var parameters = new List<SqlParameter>
			{
				new SqlParameter("id", Id)
			};

			var dataset = await _dbConnectionService.ExecuteStoredProcedureAsync("sp_GetEspecialidadById", parameters);

			if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
			{
				var especialidad = new Especialidad
				{
					Id = Convert.ToInt32(dataset.Tables[0].Rows[0]["id"]),
					Nombre = dataset.Tables[0].Rows[0]["nombre"].ToString() ?? String.Empty,
					estado = Convert.ToBoolean(dataset.Tables[0].Rows[0]["estado"])
                };
				return especialidad;
            }
			return null;

        }



        #endregion


        #region Metodos INSERT, UPDATE y DELETE
        // Agregar especialidad
        public async Task<bool> AddEspecialidadync(EspecialidadRequestDTO especialidad)
		{
			try
			{
                var parameters = new List<SqlParameter>
				{ 
					new SqlParameter("@nombre", especialidad.Nombre)
				};

                bool result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_InsertEspecialidad", parameters);
				
				if (!result) return false;

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al agregar la especialidad");
				return false;
			}

		}

		// Actualizar especialidad
		public async Task<bool> UpdateEspecialidadAsync(Especialidad especialidad)
		{
			try
			{
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@id", especialidad.Id),
					new SqlParameter("@nombre", especialidad.Nombre),
					new SqlParameter("@estado", especialidad.estado)
                };

                var result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_EditEspecialidad", parameters);
				
				if (!result) return false;

				return true;
			}catch (Exception ex)
			{
				_logger.LogError(ex, "Error al actualizar la especialidad con Id: {Id}", especialidad.Id);

				return false;
			}
		}

		// Eliminar especialidad
		public async Task<bool> DeleteEspecialidadAsync(int Id)
		{
			try
			{
				var parameters = new List<SqlParameter>()
				{
					new SqlParameter("@id", Id)
				};

				var result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_DeleteEspecialidad", parameters);
				if (!result) return false;

				return true;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error al eliminar la especialidad con id {Id}", Id);
				return false;
			}
		}


        #endregion

    }
}
