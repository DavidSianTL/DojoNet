using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using wbEjercicioEmpleadosMVC.Models;
using wbEjercicioEmpleadosMVC.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace wbEjercicioEmpleadosMVC.Data
{
    public class daoEmpleadoAsyncWS
    {
        private readonly cnnConexionAsync _conexion;

        public daoEmpleadoAsyncWS(string connectionString)
        {
            _conexion = new cnnConexionAsync(connectionString);
        }

        public async Task<List<EmpleadoModelWS>> ObtenerTodosEmpleadosAsync()
        {
            var lista = new List<EmpleadoModelWS>();
            string query = "SELECT empleadoId, nombre, apellido, fechaNacimiento, fechaIngreso, puesto, salarioBase, activo FROM Empleados";
            var ds = await _conexion.EjecutarSelectAsync(query);

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lista.Add(new EmpleadoModelWS
                    {
                        EmpleadoID = Convert.ToInt32(row["empleadoId"]),
                        Nombre = row["nombre"].ToString(),
                        Apellido = row["apellido"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(row["fechaNacimiento"]),
                        FechaIngreso = Convert.ToDateTime(row["fechaIngreso"]),
                        Puesto = row["puesto"].ToString(),
                        SalarioBase= Convert.ToDecimal(row["salarioBase"]),
                        Activo = Convert.ToBoolean(row["activo"])
                    });
                }
            }

            return lista;
        }

        public async Task<EmpleadoModelWS> ObtenerEPorIdAsync(int id)
        {
            string query = $"SELECT * FROM Empleados WHERE empleadoId = {id}";
            var ds = await _conexion.EjecutarSelectAsync(query);
            if (ds.Tables[0].Rows.Count == 0) return null;

            var row = ds.Tables[0].Rows[0];
            return new EmpleadoModelWS
            {
                EmpleadoID = Convert.ToInt32(row["empleadoId"]),
                Nombre = row["nombre"].ToString(),
                Apellido = row["apellido"].ToString(),
                FechaNacimiento = Convert.ToDateTime(row["fechaNacimiento"]),
                FechaIngreso = Convert.ToDateTime(row["fechaIngreso"]),
                Puesto = row["puesto"].ToString(),
                SalarioBase = Convert.ToDecimal(row["salarioBase"]),
                Activo = Convert.ToBoolean(row["activo"])
            };
        }

        public async Task<int> CrearEAsync(EmpleadoModelWS empleado)
        {
            var parametros = new[]
            {
                new SqlParameter("@nombre", empleado.Nombre),
                new SqlParameter("@apellido", empleado.Apellido),
                new SqlParameter("@fechaNacimiento", empleado.FechaNacimiento),
                new SqlParameter("@fechaIngreso", empleado.FechaIngreso),
                new SqlParameter("@puesto", empleado.Puesto),
                new SqlParameter("@salarioBase", empleado.SalarioBase),
                new SqlParameter("@activo", empleado.Activo)
            };

            return await _conexion.EjecutarProcedimientoAsync("sp_InsertarEmpleado", parametros);
        }

        public async Task<int> ActualizarEAsync(EmpleadoModelWS empleado)
        {
            var parametros = new[]
            {
                new SqlParameter("@empleadoId", empleado.EmpleadoID),
                new SqlParameter("@nombre", empleado.Nombre),
                new SqlParameter("@apellido", empleado.Apellido),
                new SqlParameter("@fechaNacimiento", empleado.FechaNacimiento),
                new SqlParameter("@fechaIngreso", empleado.FechaIngreso),
                new SqlParameter("@puesto", empleado.Puesto),
                new SqlParameter("@salarioBase", empleado.SalarioBase),
                new SqlParameter("@activo", empleado.Activo)
            };

            return await _conexion.EjecutarProcedimientoAsync("sp_ActualizarEmpleado", parametros);
        }

        public async Task<int> EliminarEAsync(int id)
        {
            var parametros = new[]
            {
                new SqlParameter("@empleadoId", id)
            };

            return await _conexion.EjecutarProcedimientoAsync("sp_EliminarEmpleado", parametros);
        }
    }
}
