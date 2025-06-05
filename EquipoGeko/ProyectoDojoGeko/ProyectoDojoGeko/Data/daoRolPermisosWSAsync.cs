using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;
using System.Security.Cryptography.Xml;

namespace ProyectoDojoGeko.Data
{
    public class daoRolPermisosWSAsync
    {

        private readonly string _connectionString;
        public daoRolPermisosWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Metodos de tipo SELECT

        
        public async Task<List<RolPermisosViewModel>> ObtenerRolPermisosAsync()
        {
            var rolPermisosList = new List<RolPermisosViewModel>();

            try
            {
                using SqlConnection cnn = new SqlConnection(_connectionString);
                using SqlCommand procedure = new SqlCommand("sp_ListarRolPermiso")
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                await cnn.OpenAsync();

                using SqlDataReader reader = await procedure.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var rolPermiso = new RolPermisosViewModel
                    {
                        IdRolPermiso = reader.GetInt32(reader.GetOrdinal("IdRolPermiso")),
                        FK_IdRol = reader.GetInt32(reader.GetOrdinal("FK_IdRol")),
                        FK_IdPermiso = reader.GetInt32(reader.GetOrdinal("FK_IdPermiso")),
                        FK_IdSistema = reader.GetInt32(reader.GetOrdinal("FK_IdSistema"))
                    };
                    rolPermisosList.Add(rolPermiso);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los roles y permisos: ", ex);
            }


            return rolPermisosList;

        }


        public async Task<List<RolPermisosViewModel>> ObtenerRolPermisosPorIdRolPermisosAsync(int idRolPermiso)
        {
            var rolPermisosList = new List<RolPermisosViewModel>();
            try
            {
                using SqlConnection cnn = new SqlConnection(_connectionString);
                using SqlCommand procedure = new SqlCommand("sp_ListarRolPermisoPorId", cnn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                procedure.Parameters.AddWithValue("@IdRolPermiso", idRolPermiso);
                await cnn.OpenAsync();
                using SqlDataReader reader = await procedure.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var rolPermiso = new RolPermisosViewModel
                    {
                        IdRolPermiso = reader.GetInt32(reader.GetOrdinal("IdRolPermiso")),
                        FK_IdRol = reader.GetInt32(reader.GetOrdinal("FK_IdRol")),
                        FK_IdPermiso = reader.GetInt32(reader.GetOrdinal("FK_IdPermiso")),
                        FK_IdSistema = reader.GetInt32(reader.GetOrdinal("FK_IdSistema"))
                    };
                    rolPermisosList.Add(rolPermiso);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el rol y permiso por ID: ", ex);
            }
            return rolPermisosList;
        }


        public async Task<List<RolPermisosViewModel>> ObtenerRolPermisosPorIdRolAsync(int FK_IdRol)
        {
            var rolPermisosList = new List<RolPermisosViewModel>();
            try
            {
                using SqlConnection cnn = new SqlConnection(_connectionString);
                using SqlCommand procedure = new SqlCommand("sp_ListarRolPermisoPorIdRol", cnn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                procedure.Parameters.AddWithValue("@FK_IdRol", FK_IdRol);
                await cnn.OpenAsync();

                using SqlDataReader reader = await procedure.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var rolPermiso = new RolPermisosViewModel
                    {
                        IdRolPermiso = reader.GetInt32(reader.GetOrdinal("IdRolPermiso")),
                        FK_IdRol = reader.GetInt32(reader.GetOrdinal("FK_IdRol")),
                        FK_IdPermiso = reader.GetInt32(reader.GetOrdinal("FK_IdPermiso")),
                        FK_IdSistema = reader.GetInt32(reader.GetOrdinal("FK_IdSistema"))
                    };
                    rolPermisosList.Add(rolPermiso);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los roles y permisos por ID de rol: ", ex);
            }
            return rolPermisosList;


        }

        public async Task<List<RolPermisosViewModel>> ObtenerRolPermisosPorIdPermisoAsync(int FK_IdPermiso)
        {
            var rolPermisosList = new List<RolPermisosViewModel>();

            try
            {

                using SqlConnection cnn = new SqlConnection(_connectionString);

                using SqlCommand procedure = new SqlCommand("sp_ListarRolPermisoPorIdPermiso", cnn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                procedure.Parameters.AddWithValue("@FK_IdPermiso", FK_IdPermiso);

                await cnn.OpenAsync();

                using SqlDataReader reader = await procedure.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var rolPermiso = new RolPermisosViewModel
                    {
                        IdRolPermiso = reader.GetInt32(reader.GetOrdinal("IdRolPermiso")),
                        FK_IdRol = reader.GetInt32(reader.GetOrdinal("FK_IdRol")),
                        FK_IdPermiso = reader.GetInt32(reader.GetOrdinal("FK_IdPermiso")),
                        FK_IdSistema = reader.GetInt32(reader.GetOrdinal("FK_IdSistema"))
                    };
                    rolPermisosList.Add(rolPermiso);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los roles y permisos por ID de permiso: ", ex);

            }

            return rolPermisosList;
        }

        //aún no existe el procedimiento almacenado 'sp_ListarRolPermisoPorIdSistema' en la DB, pero aquí está el método ya.
        public async Task<List<RolPermisosViewModel>> ObtenerRolPermisosPorIdSistemaAsync(int FK_IdSistema)
        {
            var rolPermisosList = new List<RolPermisosViewModel>();
            try
            {
                using SqlConnection cnn = new SqlConnection(_connectionString);
                using SqlCommand procedure = new SqlCommand("sp_ListarRolPermisoPorIdSistema", cnn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                procedure.Parameters.AddWithValue("@FK_IdSistema", FK_IdSistema);
                await cnn.OpenAsync();
                using SqlDataReader reader = await procedure.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var rolPermiso = new RolPermisosViewModel
                    {
                        IdRolPermiso = reader.GetInt32(reader.GetOrdinal("IdRolPermiso")),
                        FK_IdRol = reader.GetInt32(reader.GetOrdinal("FK_IdRol")),
                        FK_IdPermiso = reader.GetInt32(reader.GetOrdinal("FK_IdPermiso")),
                        FK_IdSistema = reader.GetInt32(reader.GetOrdinal("FK_IdSistema"))
                    };
                    rolPermisosList.Add(rolPermiso);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los roles y permisos por ID de sistema: ", ex);
            }
            return rolPermisosList;
        }

        #endregion




        #region Metodos de tipo INSERT, UPDATE y DELETE


        public async Task<bool> InsertarRolPermisoAsync(RolPermisosViewModel rolPermiso)
        {
            try
            {
                using SqlConnection cnn = new SqlConnection(_connectionString);
                using SqlCommand procedure = new SqlCommand("sp_InsertarRolPermiso", cnn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                procedure.Parameters.AddWithValue("@FK_IdRol", rolPermiso.FK_IdRol);
                procedure.Parameters.AddWithValue("@FK_IdPermiso", rolPermiso.FK_IdPermiso);
                procedure.Parameters.AddWithValue("@FK_IdSistema", rolPermiso.FK_IdSistema);
                await cnn.OpenAsync();
                int rowsAffected = await procedure.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el rol y permiso: ", ex);
            }
        }


        // El 'sp_ActualizarRolPermiso' aún no existe en la DB, pero aquí está el método ya.
        public async Task<bool> ActualizarRolPermisoAsync(RolPermisosViewModel rolPermiso)
        {
            try
            {
                using SqlConnection cnn = new SqlConnection(_connectionString);
                using SqlCommand procedure = new SqlCommand("sp_ActualizarRolPermiso", cnn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                procedure.Parameters.AddWithValue("@IdRolPermiso", rolPermiso.IdRolPermiso);
                procedure.Parameters.AddWithValue("@FK_IdRol", rolPermiso.FK_IdRol);
                procedure.Parameters.AddWithValue("@FK_IdPermiso", rolPermiso.FK_IdPermiso);
                procedure.Parameters.AddWithValue("@FK_IdSistema", rolPermiso.FK_IdSistema);
                await cnn.OpenAsync();
                int rowsAffected = await procedure.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el rol y permiso: ", ex);
            }
        }


        public async Task<bool> EliminarRolPermisoAsync(int idRolPermiso)
        {
            try
            {
                using SqlConnection cnn = new SqlConnection(_connectionString);
                using SqlCommand procedure = new SqlCommand("sp_EliminarRolPermiso", cnn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                procedure.Parameters.AddWithValue("@IdRolPermiso", idRolPermiso);
                await cnn.OpenAsync();
                int rowsAffected = await procedure.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el rol y permiso: ", ex);
            }
        }








        #endregion
               





    }
}
