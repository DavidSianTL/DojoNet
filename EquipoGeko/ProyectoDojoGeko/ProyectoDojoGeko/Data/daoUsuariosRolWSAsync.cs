using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models.Usuario;


namespace ProyectoDojoGeko.Data
{
    #region Interfaz de daoUsuariosRolWSAsync
    public interface IdaoUsuariosRolWSAsync
    {
        Task<List<UsuariosRolViewModel>> ObtenerUsuariosRolAsync();
        Task<bool> ActualizarUsuarioRolAsync(UsuariosRolViewModel usuarioRol);
        Task<bool> EliminarUsuarioRolAsync(int idUsuarioRol);
        Task<bool> InsertarUsuarioRolAsync(UsuariosRolViewModel usuarioRol);
        Task<List<UsuariosRolViewModel>> ObtenerUsuariosRolPorIdRolAsync(int idRol);
        Task<List<UsuariosRolViewModel>> ObtenerUsuariosRolPorIdUsuarioAsync(int idUsuario);
    }

    #endregion


    public class daoUsuariosRolWSAsync : IdaoUsuariosRolWSAsync
	{
		private readonly string _connectionString;
		public daoUsuariosRolWSAsync(string connectionString)
		{
			_connectionString = connectionString;
		}
		
		
		#region  Métodos de tipo SELECT
		public async Task<List<UsuariosRolViewModel>> ObtenerUsuariosRolAsync()
		{
			var usuarioRolList = new List<UsuariosRolViewModel>();

			try
			{
				string procedure = "sp_ListarUsuariosRol";

				using SqlConnection cnn = new SqlConnection(_connectionString);
				using SqlCommand cmd = new SqlCommand(procedure, cnn)
				{
					CommandType = System.Data.CommandType.StoredProcedure
				};

				await cnn.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();

				while (await reader.ReadAsync())
				{
					var usuarioRol = new UsuariosRolViewModel
					{
						IdUsuarioRol = reader.GetInt32(reader.GetOrdinal("IdUsuarioRol")),
						FK_IdUsuario = reader.GetInt32(reader.GetOrdinal("FK_IdUsuario")),
						FK_IdRol = reader.GetInt32(reader.GetOrdinal("FK_IdRol")),
						FK_IdSistema = reader.GetInt32(reader.GetOrdinal("FK_IdSistema"))
					};

					usuarioRolList.Add(usuarioRol);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener los usuarios y roles", ex);
			}

			return usuarioRolList;
		}


		public async Task<List<UsuariosRolViewModel>> ObtenerUsuariosRolPorIdRolAsync(int idRol)
		{
			var usuarioRolList = new List<UsuariosRolViewModel>();
			try
			{
				string procedure = "sp_ListarUsuariosRolPorIdRol";
				using SqlConnection cnn = new SqlConnection(_connectionString);
				using SqlCommand cmd = new SqlCommand(procedure, cnn)
				{
					CommandType = System.Data.CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@IdRol", idRol);
				await cnn.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					var usuarioRol = new UsuariosRolViewModel
					{
						IdUsuarioRol = reader.GetInt32(reader.GetOrdinal("IdUsuarioRol")),
						FK_IdUsuario = reader.GetInt32(reader.GetOrdinal("FK_IdUsuario")),
						FK_IdRol = reader.GetInt32(reader.GetOrdinal("FK_IdRol")),
						FK_IdSistema = reader.GetInt32(reader.GetOrdinal("FK_IdSistema"))
					};
					usuarioRolList.Add(usuarioRol);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener los usuarios y roles por Id de rol", ex);
			}
			return usuarioRolList;
		}

		public async Task<List<UsuariosRolViewModel>> ObtenerUsuariosRolPorIdUsuarioAsync(int idUsuario)
		{
			var usuarioRolList = new List<UsuariosRolViewModel>();
			try
			{
				string procedure = "sp_ListarUsuariosRolPorIdUsuario";
				using SqlConnection cnn = new SqlConnection(_connectionString);
				using SqlCommand cmd = new SqlCommand(procedure, cnn)
				{
					CommandType = System.Data.CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
				await cnn.OpenAsync();
				using SqlDataReader reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					var usuarioRol = new UsuariosRolViewModel
					{
						IdUsuarioRol = reader.GetInt32(reader.GetOrdinal("IdUsuarioRol")),
						FK_IdUsuario = reader.GetInt32(reader.GetOrdinal("FK_IdUsuario")),
						FK_IdRol = reader.GetInt32(reader.GetOrdinal("FK_IdRol")),
						FK_IdSistema = reader.GetInt32(reader.GetOrdinal("FK_IdSistema"))
					};
					usuarioRolList.Add(usuarioRol);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener los usuarios y roles por Id de usuario", ex);
			}
			return usuarioRolList;
		}

        #endregion


        #region Métodos de tipo INSERT, UPDATE y DELETE
        public async Task<bool> InsertarUsuarioRolAsync(UsuariosRolViewModel usuarioRol)
		{
			try
			{
				string procedure = "sp_InsertarUsuarioRol";
				using SqlConnection cnn = new SqlConnection(_connectionString);
				using SqlCommand cmd = new SqlCommand(procedure, cnn)
				{
					CommandType = System.Data.CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@FK_IdUsuario", usuarioRol.FK_IdUsuario);
				cmd.Parameters.AddWithValue("@FK_IdRol", usuarioRol.FK_IdRol);
				cmd.Parameters.AddWithValue("@FK_IdSistema", usuarioRol.FK_IdSistema);
				await cnn.OpenAsync();
				int rowsAffected = await cmd.ExecuteNonQueryAsync();

				// Verifica si se insertó al menos un registro
				return rowsAffected > 0; // Retorna true si se insertó al menos un registro
			}
			catch (Exception ex)
			{
				throw new Exception("Error al insertar el usuario y rol", ex);
			}
		}


		public async Task<bool> ActualizarUsuarioRolAsync(UsuariosRolViewModel usuarioRol)
		{
			try
			{
				string procedure = "sp_ActualizarUsuarioRol";
				using SqlConnection cnn = new SqlConnection(_connectionString);
				using SqlCommand cmd = new SqlCommand(procedure, cnn)
				{
					CommandType = System.Data.CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@IdUsuarioRol", usuarioRol.IdUsuarioRol);
				cmd.Parameters.AddWithValue("@FK_IdUsuario", usuarioRol.FK_IdUsuario);
				cmd.Parameters.AddWithValue("@FK_IdRol", usuarioRol.FK_IdRol);
				cmd.Parameters.AddWithValue("@FK_IdSistema", usuarioRol.FK_IdSistema);
				await cnn.OpenAsync();
				int rowsAffected = await cmd.ExecuteNonQueryAsync();

				// Verifica si se actualizó al menos un registro
				return rowsAffected > 0; // Retorna true si se actualizó al menos un registro
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actualizar el usuario y rol", ex);
			}
		}


		public async Task<bool> EliminarUsuarioRolAsync(int idUsuarioRol)
		{
			try
			{
				string procedure = "sp_EliminarUsuarioRol";

				using SqlConnection cnn = new SqlConnection(_connectionString);

				using SqlCommand cmd = new SqlCommand(procedure, cnn)
				{
					CommandType = System.Data.CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@IdUsuarioRol", idUsuarioRol);

				await cnn.OpenAsync();

				int rowsAffected = await cmd.ExecuteNonQueryAsync();

				// Verifica si se eliminó al menos un registro
				return rowsAffected > 0; // Retorna true si se eliminó al menos un registro

			}
			catch (Exception ex)
			{
				throw new Exception("Error al eliminar el usuario y rol", ex);
			}
		}


        #endregion


    }


}
