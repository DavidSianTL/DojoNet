using ClinicaMedicaAPIREST.Data.DTO.MedicosDTOs;
using ClinicaMedicaAPIREST.Models;
using ClinicaMedicaAPIREST.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicaMedicaAPIREST.Data.DAOs
{
	public class daoMedicos
	{
		private ILogger<daoMedicos> _logger;
		private readonly IDbConnectionService _dbConnectionService;

		public daoMedicos(IDbConnectionService dbConnectionService, ILogger<daoMedicos> logger)
		{
			_logger = logger;
			_dbConnectionService = dbConnectionService;
		}

        #region Metodos SELECT

        // Listar medicos
        public async Task<List<Medico>> GetMedicosAsync()
		{
			var dataset = await _dbConnectionService.ExecuteStoredProcedureAsync("sp_GetMedicos");
			var medicos = new List<Medico>();
			if (dataset.Tables.Count > 0)
			{
				foreach (DataRow row in dataset.Tables[0].Rows)
				{
					var medico = new Medico
                    {
						Id = Convert.ToInt32(row["id"]),
						Nombre = row["nombre"].ToString()!,
						Email = row["email"].ToString()!,
						Estado = Convert.ToBoolean(row["estado"])
                    };
                    medicos.Add(medico);
				}
			}
			return medicos;
		}
		
		// Listar medicos por Id
		public async Task<List<Medico>> GetMedicosByIdAsync(int Id)
		{
            var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@id", Id),
                };
            var dataset = await _dbConnectionService.ExecuteStoredProcedureAsync("sp_GetMedicosById", parameters);
			var medicos = new List<Medico>();
			if (dataset.Tables.Count > 0)
			{
				foreach (DataRow row in dataset.Tables[0].Rows)
				{
					var medico = new Medico
                    {
						Id = Convert.ToInt32(row["id"]),
						Nombre = row["nombre"].ToString()!,
						Email = row["email"].ToString()!,
						Estado = Convert.ToBoolean(row["estado"])
                    };
                    medicos.Add(medico);
				}
			}
			return medicos;
		}


        #endregion


        #region Metodos INSERT, UPDATE y DELETE
        // Agregar medico
        public async Task<bool> AddMedicoAsync(MedicoRequestDTO medico)
		{
			try
			{
				var parameters = new List<SqlParameter>
				{
					new SqlParameter("@nombre", medico.Nombre),
					new SqlParameter("@email", medico.Email),
					new SqlParameter("@FK_IdEspecialidad", medico.Especialidad),

				};

				bool result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_InsertMedico", parameters);
				if (!result)
				{
					return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al agregar al paciente {medico.nombre}", medico.Nombre);
				return false;
			}

		}

		// Actualizar medico
		public async Task<bool> UpdateMedicoAsync(Medico medico)
		{
			try
			{
				var parameters = new List<SqlParameter>
				{
					new SqlParameter("@id", medico.Id),
					new SqlParameter("@nombre", medico.Nombre),
					new SqlParameter("@email", medico.Email),
					new SqlParameter("@FK_IdEspecialidad", medico.Especialidad),
                    new SqlParameter("@estado", SqlDbType.Bit) { Value = medico.Estado }

                };

				var result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_EditMedico", parameters);
				if (!result) return false;


				return true;
			}catch (Exception ex)
			{
				_logger.LogError(ex, "Error al actualizar el medico {medico.Nombre}", medico.Nombre);

				return false;
			}
		}

		// Eliminar medico
		public async Task<bool> DeleteMedicoAsync(Medico medico)
		{
			try
			{
				var parameters = new List<SqlParameter>()
				{
					new SqlParameter("@id", medico.Id)
				};

				var result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_DeleteMedico", parameters);
				if (!result)
				{
					return false;
					
				}

				return true;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error al eliminar al medico con id {medico.Id}", medico.Nombre);
				return false;
			}
		}


        #endregion

    }
}
