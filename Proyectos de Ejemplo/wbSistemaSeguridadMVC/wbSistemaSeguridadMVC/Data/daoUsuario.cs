
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using wbSistemaSeguridadMVC.Models;
using wbSistemaSeguridadMVC.Services;

namespace wbSistemaSeguridadMVC.Data
{
    public class daoUsuario
    {
        private readonly cnnConexionAsync _conexion;

        public daoUsuario(string connectionString)
        {
            _conexion = new cnnConexionAsync(connectionString);
        }

        public async Task <List<Usuario>> ObtenerUsuarios()
        {
            var lista = new List<Usuario>();
            string sql = "SELECT u.id_usuario, u.usuario, u.nom_usuario, u.fk_id_estado, u.fecha_creacion, e.descripcion FROM Usuarios u INNER JOIN Estado_Usuario e ON u.fk_id_estado = e.id_estado";
            var ds = await _conexion.EjecutarSelectAsync(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lista.Add(new Usuario
                {
                    IdUsuario = Convert.ToInt32(row["id_usuario"]),
                    UsuarioLg = row["usuario"].ToString(),
                    NomUsuario = row["nom_usuario"].ToString(),
                    FkIdEstado = Convert.ToInt32(row["fk_id_estado"]),
                    FechaCreacion = Convert.ToDateTime(row["fecha_creacion"]),
                    Estado = new Estado_Usuario
                    {
                        IdEstado = Convert.ToInt32(row["fk_id_estado"]),
                        Descripcion = row["descripcion"].ToString()
                    }
                });
            }

            return lista;
        }

        public async Task<Usuario> ObtenerUsuarioPorId(int id)
        {
            string sql = $"SELECT id_usuario, usuario, nom_usuario, fk_id_estado, FORMAT(fecha_creacion,'dd/MM/yyyy HH:mm:ss') as fecha_creacion FROM Usuarios WHERE id_usuario = {id}";
            var ds = await _conexion.EjecutarSelectAsync(sql);
            if (ds.Tables[0].Rows.Count == 0) return null;

            var row = ds.Tables[0].Rows[0];
            return new Usuario
            {
                IdUsuario = Convert.ToInt32(row["id_usuario"]),
                UsuarioLg = row["usuario"].ToString(),
                NomUsuario = row["nom_usuario"].ToString(),
                FkIdEstado = Convert.ToInt32(row["fk_id_estado"]),
                FechaCreacion = Convert.ToDateTime(row["fecha_creacion"]),
                Estado = new Estado_Usuario() // Inicializa aunque sea vacío
            };
        }

        public async Task<int> InsertarUsuario(Usuario u)
        {
            string sql = $@"
            INSERT INTO Usuarios (usuario, nom_usuario, contrasenia, fk_id_estado, fecha_creacion)
            VALUES ('{u.UsuarioLg}', '{u.NomUsuario}','{u.Contrasenia}', {u.FkIdEstado},GETDATE())";
            return await _conexion.EjecutarComandoAsync(sql);
        }

        public async Task<int> ActualizarUsuario(Usuario u)
        {
            string sql = $@"
            UPDATE Usuarios SET 
                usuario = '{u.UsuarioLg}', 
                nom_usuario = '{u.NomUsuario}', 
                fk_id_estado = {u.FkIdEstado}
            WHERE id_usuario = {u.IdUsuario}";
            return await _conexion.EjecutarComandoAsync(sql);
        }

        public async Task<int> EliminarUsuario(int id)
        {
            string sql = $@"
            UPDATE Usuarios SET 
                fk_id_estado = 4
            WHERE id_usuario = {id}";

            // $"DELETE FROM Usuarios WHERE id_usuario = {id}";
            return await _conexion.EjecutarComandoAsync(sql);
        }

        public async Task<List<Estado_Usuario>> ObtenerEstados()
        {
            List<Estado_Usuario> estados = new List<Estado_Usuario>();
            var ds = await _conexion.EjecutarSelectAsync("SELECT id_estado, descripcion FROM Estado_Usuario");

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                estados.Add(new Estado_Usuario
                {
                    IdEstado = Convert.ToInt32(row["id_estado"]),
                    Descripcion = row["descripcion"].ToString()
                });
            }

            return estados;
        }



        public async Task<DataTable> ObtenerUsuariosDataTable()
        {
            DataTable dt = new DataTable();
            string sql = $@"SELECT u.id_usuario as ""Còdigo""
	                    , u.nom_usuario as ""Nombre""
	                    , e.descripcion as ""Estado""
	                    , convert(varchar(20),u.fecha_creacion, 120) as ""Fecha_creacion""
	                    , format(u.fecha_creacion, 'dd-MM-yyyy HH:mm:ss') as ""Fecha_creacion2""
                    FROM Usuarios u
                    INNER JOIN Estado_Usuario e ON(u.fk_id_estado = e.id_estado)";
            
            dt = await _conexion.EjecutarSelectDTAsync(sql);

           
            return dt;
        }
       



    }
}
