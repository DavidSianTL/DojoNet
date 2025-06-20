using ClinicaMedicaAPIREST.Models;
using ClinicaMedicaAPIREST.Services;
using System.Data;

namespace ClinicaMedicaAPIREST.Data.DAOs
{
    public class daoPacientes
    {
        private readonly IDbConnectionService _dbConnectionService;

        public daoPacientes(IDbConnectionService dbConnectionService)
        {
            _dbConnectionService = dbConnectionService;
        }


        public async Task<List<Paciente>> GetPacientesAsync()
        {
            var ds = await _dbConnectionService.ExecuteStoredProcedureAsync("sp_GetPacientes");
            var pacientes = new List<Paciente>();
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var paciente = new Paciente
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Nombre = row["nombre"].ToString()!,
                        Email = row["email"].ToString()!,
                        Telefono = row["telefono"].ToString()!,
                        FechaNacimiento = Convert.ToDateTime(row["fecha_nacimiento"])
                    };
                    pacientes.Add(paciente);
                }
            }
            return pacientes;
        }



    }
}
