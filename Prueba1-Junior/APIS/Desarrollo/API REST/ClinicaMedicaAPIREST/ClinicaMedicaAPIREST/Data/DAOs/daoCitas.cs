using ClinicaMedicaAPIREST.Data.DTO.CitasDTOs;
using ClinicaMedicaAPIREST.Data.DTO.MedicosDTOs;
using ClinicaMedicaAPIREST.Models;
using ClinicaMedicaAPIREST.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicaMedicaAPIREST.Data.DAOs
{
	public class daoCitas
	{
		private ILogger<daoCitas> _logger;
		private readonly IDbConnectionService _dbConnectionService;

		public daoCitas(IDbConnectionService dbConnectionService, ILogger<daoCitas> logger)
		{
			_logger = logger;
			_dbConnectionService = dbConnectionService;
		}

        #region Metodos SELECT

        // Listar citas
        public async Task<List<Cita>> GetCitasAsync()
		{
			var dataset = await _dbConnectionService.ExecuteStoredProcedureAsync("sp_GetCitas");
			var Citas = new List<Cita>();
			if (dataset.Tables.Count > 0)
			{
				foreach (DataRow row in dataset.Tables[0].Rows)
				{
					var cita = new Cita
                    {
						Id = Convert.ToInt32(row["id"]),
						Medico_Id = Convert.ToInt32(row["FK_IdMedico"]),
						Paciente_Id = Convert.ToInt32(row["FK_IdPaciente"]),
						Fecha = DateOnly.FromDateTime(Convert.ToDateTime(row["fecha"])),
                        Hora = TimeOnly.FromTimeSpan((TimeSpan)row["hora"]),
                        Estado = Convert.ToBoolean(row["estado"])
                    };
                    Citas.Add(cita);
				}
			}
			return Citas;
		}
		
		// Listar citas por Id
		public async Task<List<Cita>> GetCitasByIdAsync(int Id)
		{
            var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@id", Id),
				};

            var dataset = await _dbConnectionService.ExecuteStoredProcedureAsync("sp_GetCitaById", parameters);
			var Citas = new List<Cita>();

			if (dataset.Tables.Count > 0)
			{
				foreach (DataRow row in dataset.Tables[0].Rows)
				{
                    var cita = new Cita
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Medico_Id = Convert.ToInt32(row["FK_IdMedico"]),
                        Paciente_Id = Convert.ToInt32(row["FK_IdPaciente"]),
                        Fecha = DateOnly.FromDateTime(Convert.ToDateTime(row["fecha"])),
                        Hora = TimeOnly.FromTimeSpan((TimeSpan)row["hora"]),
                        Estado = Convert.ToBoolean(row["estado"])
                    };
                    Citas.Add(cita);
                }
			}
			return Citas;
		}


        #endregion


        #region Metodos INSERT, UPDATE y DELETE
        // Agregar cita
        public async Task<bool> AddCitaAsync(CitaRequestDTO cita)
		{
			try
			{
                var parameters = new List<SqlParameter>
				{ 
					new SqlParameter("FK_IdPaciente", cita.Paciente_Id),
					new SqlParameter("FK_IdMedico", cita.Medico_Id),
					new SqlParameter("fecha", cita.Fecha),
					new SqlParameter("hora", cita.Hora)
                };

                bool result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_InsertCita", parameters);
				if (!result)
				{
					return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al agregar la cita");
				return false;
			}

		}

		// Actualizar cita
		public async Task<bool> UpdateCitaAsync(Cita cita)
		{
			try
			{
                var parameters = new List<SqlParameter>
                {
					new SqlParameter("id", cita.Id),
                    new SqlParameter("FK_IdPaciente", cita.Paciente_Id),
                    new SqlParameter("FK_IdMedico", cita.Medico_Id),
                    new SqlParameter("fecha", cita.Fecha),
                    new SqlParameter("hora", cita.Hora),
					new SqlParameter("estado", cita.Estado)
                };

                var result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_EditCita", parameters);
				if (!result) return false;


				return true;
			}catch (Exception ex)
			{
				_logger.LogError(ex, "Error al actualizar la cita con Id: {cita.Id}", cita.Id);

				return false;
			}
		}

		// Eliminar cita
		public async Task<bool> DeleteCitaAsync(Cita cita)
		{
			try
			{
				var parameters = new List<SqlParameter>()
				{
					new SqlParameter("@id", cita.Id)
				};

				var result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_DeleteCita", parameters);
				if (!result)
				{
					return false;
					
				}

				return true;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error al eliminar la cita con id {cita.Id}", cita.Id);
				return false;
			}
		}


        #endregion

    }
}
