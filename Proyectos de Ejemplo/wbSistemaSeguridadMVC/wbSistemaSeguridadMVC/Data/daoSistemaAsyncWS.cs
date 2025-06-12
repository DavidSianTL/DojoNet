using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using wbSistemaSeguridadMVC.Models;
using wbSistemaSeguridadMVC.Services;




namespace wbSistemaSeguridadMVC.Data
{
    public class daoSistemaAsyncWS
    {

        private readonly cnnConexionAsync _conexion;

        public daoSistemaAsyncWS(string connectionString)
        {
            _conexion = new cnnConexionAsync(connectionString);
        }

        //public daoSistemaAsyncWS(cnnConexionAsync connectionString)
        //{
        //    _conexion = connectionString;
        //}

        public async Task<List<SistemaWS>> ObtenerTodosAsync()
        {
            var lista = new List<SistemaWS>();
            string query = "SELECT id_sistema, nombre_sistema, descripcion FROM Sistemas";
            var ds = await _conexion.EjecutarSelectAsync(query);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lista.Add(new SistemaWS
                    {
                        IdSistema = Convert.ToInt32(row["id_sistema"]),
                        NombreSistema = row["nombre_sistema"].ToString(),
                        Descripcion = row["descripcion"].ToString()
                    });
                }
            }

            return lista;
        }

        public async Task<SistemaWS> ObtenerPorIdAsync(int id)
        {
            string query = $"SELECT * FROM Sistemas WHERE id_sistema = {id}";
            var ds = await _conexion.EjecutarSelectAsync(query);
            if (ds.Tables[0].Rows.Count == 0) return null;

            var row = ds.Tables[0].Rows[0];
            return new SistemaWS
            {
                IdSistema = Convert.ToInt32(row["id_sistema"]),
                NombreSistema = row["nombre_sistema"].ToString(),
                Descripcion = row["descripcion"].ToString()
            };
        }

        public async Task<int> CrearAsync(SistemaWS sistema)
        {
            var parametros = new[]
            {
                new SqlParameter("@NombreSistema", sistema.NombreSistema),
                new SqlParameter("@Descripcion", sistema.Descripcion ?? "")
            };

            return await _conexion.EjecutarProcedimientoAsync("sp_InsertarSistema", parametros);
        }

        public async Task<int> ActualizarAsync(SistemaWS sistema)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdSistema", sistema.IdSistema),
                new SqlParameter("@NombreSistema", sistema.NombreSistema),
                new SqlParameter("@Descripcion", sistema.Descripcion ?? "")
            };

            return await _conexion.EjecutarProcedimientoAsync("sp_ActualizarSistema", parametros);
        }

        public async Task<int> EliminarAsync(int id)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdSistema", id)
            };

            return await _conexion.EjecutarProcedimientoAsync("sp_EliminarSistema", parametros);
        }




    }
}
