using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MVCdaoWS.Models;
using MVCdaoWS.Services;

namespace MVCdaoWS.Data
{
    public class daoEmpleadoAsyncWS
    {
        private readonly cnnConexionAsync _conexion;

        public daoEmpleadoAsyncWS(string connectionString)
        {
            _conexion = new cnnConexionAsync(connectionString);
        }

        public async Task<List<EmpleadoWS>> ObtenerTodosAsync()
        {
            var lista = new List<EmpleadoWS>();
            string query = "SELECT * FROM Empleados";
            var ds = await _conexion.EjecutarSelectAsync(query);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lista.Add(new EmpleadoWS
                {
                    EmpleadoID = Convert.ToInt32(row["EmpleadoID"]),
                    Nombre = row["Nombre"].ToString(),
                    Apellido = row["Apellido"].ToString(),
                    FechaNacimiento = Convert.ToDateTime(row["FechaNacimiento"]),
                    FechaIngreso = Convert.ToDateTime(row["FechaIngreso"]),
                    Puesto = row["Puesto"].ToString(),
                    SalarioBase = Convert.ToDecimal(row["SalarioBase"]),
                    Activo = Convert.ToBoolean(row["Activo"])
                });
            }
            return lista;
        }

        public async Task<EmpleadoWS> ObtenerPorIdAsync(int id)
        {
            string query = $"SELECT * FROM Empleados WHERE EmpleadoID = {id}";
            var ds = await _conexion.EjecutarSelectAsync(query);
            if (ds.Tables[0].Rows.Count == 0) return null;

            var row = ds.Tables[0].Rows[0];
            return new EmpleadoWS
            {
                EmpleadoID = Convert.ToInt32(row["EmpleadoID"]),
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                FechaNacimiento = Convert.ToDateTime(row["FechaNacimiento"]),
                FechaIngreso = Convert.ToDateTime(row["FechaIngreso"]),
                Puesto = row["Puesto"].ToString(),
                SalarioBase = Convert.ToDecimal(row["SalarioBase"]),
                Activo = Convert.ToBoolean(row["Activo"])
            };
        }

        public async Task<int> CrearAsync(EmpleadoWS empleado)
        {
            var parametros = new[]
            {
                new SqlParameter("@Nombre", empleado.Nombre),
                new SqlParameter("@Apellido", empleado.Apellido),
                new SqlParameter("@FechaNacimiento", empleado.FechaNacimiento),
                new SqlParameter("@FechaIngreso", empleado.FechaIngreso),
                new SqlParameter("@Puesto", empleado.Puesto),
                new SqlParameter("@SalarioBase", empleado.SalarioBase),
                new SqlParameter("@Activo", empleado.Activo)
            };

            return await _conexion.EjecutarProcedimientoAsync("sp_InsertarEmpleado", parametros);
        }

        public async Task<int> ActualizarAsync(EmpleadoWS empleado)
        {
            var parametros = new[]
            {
                new SqlParameter("@EmpleadoID", empleado.EmpleadoID),
                new SqlParameter("@Nombre", empleado.Nombre),
                new SqlParameter("@Apellido", empleado.Apellido),
                new SqlParameter("@FechaNacimiento", empleado.FechaNacimiento),
                new SqlParameter("@FechaIngreso", empleado.FechaIngreso),
                new SqlParameter("@Puesto", empleado.Puesto),
                new SqlParameter("@SalarioBase", empleado.SalarioBase),
                new SqlParameter("@Activo", empleado.Activo)
            };

            return await _conexion.EjecutarProcedimientoAsync("sp_ActualizarEmpleado", parametros);
        }

        public async Task<int> EliminarAsync(int id)
        {
            var parametros = new[] {
                new SqlParameter("@EmpleadoID", id)
            };

            return await _conexion.EjecutarProcedimientoAsync("sp_EliminarEmpleado", parametros);
        }
    }
}

