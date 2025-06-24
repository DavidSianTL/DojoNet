using ClinicaMedicaAPIREST.Models;
using ClinicaMedicaAPIREST.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ClinicaMedicaAPIREST.Data.DAOs
{
    public class daoUsuarios
    {
        private readonly IDbConnectionService _connectionService;

        public daoUsuarios(IDbConnectionService connectionService)
        {
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
                            Rol = row["rol"].ToString()!,
                            Estado = Convert.ToBoolean(row["estado"])
                        };
                        usuarios.Add(usuario);
                    }
                }
                return usuarios;
            }
            catch (Exception ex)
            {
                return usuarios;
            }
        }




        #endregion



    }
}
