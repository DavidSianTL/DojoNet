

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using wbSistemaSeguridad2.Models;
using wbSistemaSeguridad2.Services;


namespace wbSistemaSeguridad2.Data
{
    public class daoSistema
    {
        private readonly cnnConexionMS _conexion;

        public daoSistema(string connectionString)
        {
            _conexion = new cnnConexionMS(connectionString);
        }

        public List<Sistema> ObtenerSistemas()
        {
            var lista = new List<Sistema>();
            string sql = "SELECT id_sistema, nombre_sistema, descripcion FROM Sistemas";
            var ds = _conexion.EjecutarSelect(sql);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lista.Add(new Sistema
                {
                    id_sistema = Convert.ToInt32(row["id_sistema"]),
                    nombre_sistema = row["nombre_sistema"].ToString(),
                    descripcion = row["descripcion"].ToString(),
                 
                });
            }

            return lista;
        }

        public Sistema ObtenerSistemaPorId(int id)
        {
            string sql = $"SELECT id_sistema, nombre_sistema, descripcion FROM Sistemas WHERE id_sistema = {id}";
            var ds = _conexion.EjecutarSelect(sql);
            if (ds.Tables[0].Rows.Count == 0) return null;

            var row = ds.Tables[0].Rows[0];
            return new Sistema
            {
                id_sistema = Convert.ToInt32(row["id_sistema"]),
                nombre_sistema = row["nombre_sistema"].ToString(),
                descripcion = row["descripcion"].ToString(),
               
            };
        }


        public int InsertarSistema(Sistema sistema)
        {
            string sql = $@"
            INSERT INTO Sistemas (nombre_sistema, descripcion)
            VALUES ('{sistema.nombre_sistema}', '{sistema.descripcion}')";
            return _conexion.EjecutarComando(sql);
        }

        public int ActualizarSistema(Sistema  sistema)
        {
            string sql = $@"
            UPDATE Sistemas SET 
                nombre_sistema = '{sistema.nombre_sistema}', 
                descripcion = '{sistema.descripcion}', 
            WHERE id_sistema = {sistema.id_sistema}";
            return _conexion.EjecutarComando(sql);
        }

        public int EliminarSistema(int id)
        {
            string sql = $@"
            DELETE FROM Sistemas  
            WHERE id_sistema = {id}";
            return _conexion.EjecutarComando(sql);
        }

        public int CrearProc(Sistema sistema)
        {
            var parametros = new[]
            {
                new SqlParameter("@NombreSistema", sistema.nombre_sistema),
                new SqlParameter("@Descripcion", sistema.descripcion ?? "")
            };

            return _conexion.EjecutarProcedimiento("sp_InsertarSistema", parametros);
        }

        public int ActualizarProc(Sistema sistema)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdSistema", sistema.id_sistema),
                new SqlParameter("@NombreSistema", sistema.nombre_sistema),
                new SqlParameter("@Descripcion", sistema.descripcion ?? "")
            };

            return  _conexion.EjecutarProcedimiento("sp_ActualizarSistema", parametros);
        }

        public int EliminarProc(int id)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdSistema", id)
            };
            return _conexion.EjecutarProcedimiento("sp_EliminarSistema", parametros);
        }

    }
}
