using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data.Usuarios
{
    public class daoEmpleadoWSAsync
    {

        // Variable global para la conexión
        private readonly cnnConexionWSAsync _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoEmpleadoWSAsync(string connectionString)
        {
            _connectionString = new cnnConexionWSAsync(connectionString);
        }

        public async Task<List<UsuarioViewModel>> ObtenerUsuariosAsync()
        {
            var lista = new List<UsuarioViewModel>();
            string query = "SELECT * FROM Usuarios";
            var dataSet = await _connectionString.EjecutarSelectAsync(query);

            if (dataSet.Tables.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    lista.Add(new UsuarioViewModel
                    {
                        
                    });
                }
            }

            return lista;
        }


        // Método para buscar un usuario por su ID
        public async Task<int> ObtenerUsuarioPorIdAsync(int Id)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdUsuario", Id)
            };

            return await _connectionString.EjecutarProcedimientoAsync("sp_ListarUsuarioId", parametros);
        }

        // Método para insertar un nuevo usuario
        public async Task<int> InsertarUsuarioAsync(UsuarioViewModel usuario)
        {
            var parametros = new[]
            {
                new SqlParameter("@Nombre", usuario.Username)
            };

            return await _connectionString.EjecutarProcedimientoAsync("sp_InsertarUsuario", parametros);

        }


        // Método para actualizar un usuario existente 
        public async Task<int> ActualizarUsuarioAsync(UsuarioViewModel usuario)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdUsuario", usuario.IdUsuario)
            };

            return await _connectionString.EjecutarProcedimientoAsync("sp_ActualizarUsuario", parametros);

        }

        //Método para eliminar(cambiar su estado) un usuario por su ID
        public async Task<int> EliminarUsuarioAsync(int Id)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdUsuario", Id)
            };

            return await _connectionString.EjecutarProcedimientoAsync("sp_EliminarUsuario", parametros);

        }


    }
}