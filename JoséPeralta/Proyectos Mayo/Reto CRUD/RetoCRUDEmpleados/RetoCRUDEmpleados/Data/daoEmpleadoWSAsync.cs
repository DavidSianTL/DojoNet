using System.Data;
using Microsoft.Data.SqlClient;
using RetoCRUDEmpleados.Models;
using wbSistemaSeguridadMVC.Services;

namespace RetoCRUDEmpleados.Data
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

        // Update the return type of the method to match the type of the list being returned.
        public async Task<List<EmpleadoWSViewModel>> ObtenerEmpleadosAsync()
        {
            var lista = new List<EmpleadoWSViewModel>();
            string query = "SELECT * FROM Empleados";
            var dataSet = await _connectionString.EjecutarSelectAsync(query);

            if (dataSet.Tables.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    lista.Add(new EmpleadoWSViewModel
                    {
                        IdEmpleado = Convert.ToInt32(row["EmpleadoID"]),
                        Nombre = row["Nombre"].ToString(),
                        Apellido = row["Apellido"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(row["FechaNacimiento"]),
                        FechaIngreso = Convert.ToDateTime(row["FechaIngreso"]),
                        Puesto = row["Puesto"].ToString(),
                        SalarioBase = Convert.ToDecimal(row["SalarioBase"]),
                        Activo = Convert.ToBoolean(row["Activo"])
                    });
                }
            }

            return lista;
        }

        // Método para buscar un sistema por su ID
        public async Task<EmpleadoWSViewModel> ObtenerEmpleadoPorIdAsync(int Id)
        {
            string query = $"SELECT * FROM Empleados WHERE EmpleadoID = {Id}";
            var ds = await _connectionString.EjecutarSelectAsync(query);
            if (ds.Tables[0].Rows.Count == 0) return null;

            var row = ds.Tables[0].Rows[0];
            return new EmpleadoWSViewModel
            {
                IdEmpleado = Convert.ToInt32(row["EmpleadoID"]),
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                FechaNacimiento = Convert.ToDateTime(row["FechaNacimiento"]),
                FechaIngreso = Convert.ToDateTime(row["FechaIngreso"]),
                Puesto = row["Puesto"].ToString(),
                SalarioBase = Convert.ToDecimal(row["SalarioBase"]),
                Activo = Convert.ToBoolean(row["Activo"])
            };
        }

        // Método para insertar un nuevo empleado
        public async Task<int> InsertarEmpleadoAsync(EmpleadoWSViewModel empleado)
        {
            var parametros = new[]
            {
                new SqlParameter("@Nombre", empleado.Nombre),
                new SqlParameter("@Apellido", empleado.Apellido),
                new SqlParameter("@FechaNacimiento", empleado.FechaNacimiento),
                new SqlParameter("@FechaIngreso", empleado.FechaIngreso),
                new SqlParameter("@Puesto", empleado.Puesto),
                new SqlParameter("@SalarioBase", empleado.SalarioBase)
            };

            return await _connectionString.EjecutarProcedimientoAsync("sp_InsertarEmpleado", parametros);

        }



        // Método para actualizar un empleado existente 
        public async Task<int> ActualizarEmpleadoAsync(EmpleadoWSViewModel empleado)
        {
            var parametros = new[]
            {
                new SqlParameter("@EmpleadoID", empleado.IdEmpleado),
                new SqlParameter("@Nombre", empleado.Nombre),
                new SqlParameter("@Apellido", empleado.Apellido),
                new SqlParameter("@FechaNacimiento", empleado.FechaNacimiento),
                new SqlParameter("@FechaIngreso", empleado.FechaIngreso),
                new SqlParameter("@Puesto", empleado.Puesto),
                new SqlParameter("@SalarioBase", empleado.SalarioBase),
                new SqlParameter("@Activo", empleado.Activo)
            };

            return await _connectionString.EjecutarProcedimientoAsync("sp_ActualizarEmpleado", parametros);

        }

        //Método para eliminar un empleado por su ID
        public async Task<int> EliminarEmpleadoAsync(int Id)
        {
            var parametros = new[]
            {
                new SqlParameter("@EmpleadoID", Id)
            };

            return await _connectionString.EjecutarProcedimientoAsync("sp_EliminarEmpleado", parametros);

        }


    }
}
