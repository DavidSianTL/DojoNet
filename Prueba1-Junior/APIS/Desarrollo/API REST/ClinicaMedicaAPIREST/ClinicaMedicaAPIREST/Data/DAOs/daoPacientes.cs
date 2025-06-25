using ClinicaMedicaAPIREST.Data.DTO.PacientesDTOs;
using ClinicaMedicaAPIREST.Models;
using ClinicaMedicaAPIREST.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicaMedicaAPIREST.Data.DAOs
{
	public class daoPacientes
	{
		private ILogger<daoPacientes> _logger;
		private readonly IDbConnectionService _dbConnectionService;

		public daoPacientes(IDbConnectionService dbConnectionService, ILogger<daoPacientes> logger)
		{
			_logger = logger;
			_dbConnectionService = dbConnectionService;
		}

        #region Metodos SELECT

        // Listar pacientes
        public async Task<List<Paciente>> GetPacientesAsync()
		{
			var dataset = await _dbConnectionService.ExecuteStoredProcedureAsync("sp_GetPacientes");
			var pacientes = new List<Paciente>();
			if (dataset.Tables.Count > 0)
			{
				foreach (DataRow row in dataset.Tables[0].Rows)
				{
					var paciente = new Paciente
					{
						Id = Convert.ToInt32(row["id"]),
						Nombre = row["nombre"].ToString()!,
						Email = row["email"].ToString()!,
						Telefono = row["telefono"].ToString()!,
						FechaNacimiento = DateOnly.FromDateTime(Convert.ToDateTime(row["fecha_nacimiento"])),
						Estado = Convert.ToBoolean(row["estado"])
                    };
					pacientes.Add(paciente);
				}
			}
			return pacientes;
		}
		
		// Listar pacientes por Id
		public async Task<List<Paciente>> GetPacientesByIdAsync(int Id)
		{
            var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@id", Id),
                };
            var dataset = await _dbConnectionService.ExecuteStoredProcedureAsync("sp_GetPacienteById", parameters);
			var pacientes = new List<Paciente>();
			if (dataset.Tables.Count > 0)
			{
				foreach (DataRow row in dataset.Tables[0].Rows)
				{
					var paciente = new Paciente
					{
						Id = Convert.ToInt32(row["id"]),
						Nombre = row["nombre"].ToString()!,
						Email = row["email"].ToString()!,
						Telefono = row["telefono"].ToString()!,
						FechaNacimiento = DateOnly.FromDateTime(Convert.ToDateTime(row["fecha_nacimiento"])),
						Estado = Convert.ToBoolean(row["estado"])
                    };
					pacientes.Add(paciente);
				}
			}
			return pacientes;
		}


        #endregion


        #region Metodos INSERT, UPDATE y DELETE
        // Agregar paciente
        public async Task<bool> AddPacienteAsync(PacienteRequestDTO paciente)
		{
			try
			{
				var parameters = new List<SqlParameter>
				{
					new SqlParameter("@nombre", paciente.Nombre),
					new SqlParameter("@email", paciente.Email),
					new SqlParameter("@telefono", paciente.Telefono),
					new SqlParameter("@fecha_nacimiento", paciente.FechaNacimiento)
				};

				bool result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_InsertPaciente", parameters);
				if (!result)
				{
					return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al agregar al paciente {paciente.nombre}", paciente.Nombre);
				return false;
			}

		}

		// Actualizar paciente
		public async Task<bool> UpdatePacienteAsync(Paciente paciente)
		{
			try
			{
				var parameters = new List<SqlParameter>
				{
					new SqlParameter("@id", paciente.Id),
					new SqlParameter("@nombre", paciente.Nombre),
					new SqlParameter("@email", paciente.Email),
					new SqlParameter("@telefono", paciente.Telefono),
					new SqlParameter("@fecha_nacimiento", paciente.FechaNacimiento),
                    new SqlParameter("@estado", SqlDbType.Bit) { Value = paciente.Estado }

                };

				var result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_EditPaciente", parameters);
				if (!result)
				{
					return false;
				}

				return true;
			}catch (Exception ex)
			{
				_logger.LogError(ex, "Error al actualizar el paciente {paciente.Nombre}", paciente.Nombre);

				return false;
			}
		}

		// Eliminar paciente
		public async Task<bool> DeletePacienteAsync(Paciente paciente)
		{
			try
			{
				var parameters = new List<SqlParameter>()
				{
					new SqlParameter("@id", paciente.Id)
				};

				var result = await _dbConnectionService.ExecuteStoredProcedureNonQueryAsync("sp_DeletePaciente", parameters);
				if (!result)
				{
					return false;
					
				}

				return true;
			}
			catch(Exception ex)
			{
				_logger.LogError(ex, "Error al eliminar al paciente con id {paciente.Id}", paciente.Nombre);
				return false;
			}
		}


        #endregion

    }
}
