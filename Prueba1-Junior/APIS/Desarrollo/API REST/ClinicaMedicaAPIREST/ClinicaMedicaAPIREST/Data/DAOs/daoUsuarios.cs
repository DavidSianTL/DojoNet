using ClinicaMedicaAPIREST.Models;
using ClinicaMedicaAPIREST.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicaMedicaAPIREST.Data.DAOs
{
    public class daoUsuarios
    {
        private readonly IDbConnectionService _connectionService;
        private readonly ILogger<daoUsuarios> _logger;
        public daoUsuarios(IDbConnectionService connectionService, ILogger<daoUsuarios> logger)
        {
            _logger = logger;
            _connectionService = connectionService;
        }



        #region Metodos de obtencion de datos



        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            var usuarios = new List<Usuario>();

            try
            {
                var dbSet = await _connectionService.ExecuteStoredProcedureAsync("sp_GetUsuarios");

                if (dbSet.Tables.Count > 0)
                {
                    foreach (DataRow row in dbSet.Tables[0].Rows)
                    {
                        var usuario = new Usuario()
                        {
                            Id = Convert.ToInt32(row["id"]),
                            Username = row["username"].ToString()!,
                            Email = row["email"].ToString()!,
                            Password = row["password"].ToString()!,
                            Role = row["rol"].ToString()!,
                            Estado = Convert.ToBoolean(row["estado"])
                        };
                        usuarios.Add(usuario);
                    }
                }
                return usuarios;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los usuarios");
                return usuarios;
            }
        }
        public async Task<List<Usuario>> GetUsuariosByIdAsync (int Id)
        {
            var usuarios = new List<Usuario>();

            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@id", Id),
                };
                var dbSet = await _connectionService.ExecuteStoredProcedureAsync("sp_GetUsuarioById", parameters);

                if (dbSet.Tables.Count > 0)
                {
                    foreach (DataRow row in dbSet.Tables[0].Rows)
                    {
                        var usuario = new Usuario()
                        {
                            Id = Convert.ToInt32(row["id"]),
                            Username = row["username"].ToString()!,
                            Email = row["email"].ToString()!,
                            Password = row["password"].ToString()!,
                            Role = row["role"].ToString()!,
                            Estado = Convert.ToBoolean(row["estado"])
                        };
                        usuarios.Add(usuario);
                    }
                }
                return usuarios;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con Id {Id}", Id);
                return usuarios;
            }
        }

        

        

        #endregion



    }
}
