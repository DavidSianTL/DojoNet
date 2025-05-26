using System.Data;
using Microsoft.Data.SqlClient;
using SistemaSeguridadMVC.Models;
using SistemaSeguridadMVC.Services;
using wbSistemaSeguridadMVC.Services;

namespace SistemaSeguridadMVC.Data
{
    public class DaoSistemasWSAsync
    {

        private readonly cnnConexionWSAsync _cnnConexionWSAsync;

        public DaoSistemasWSAsync(string connectionString)
        {
            _cnnConexionWSAsync = new cnnConexionWSAsync(connectionString);
        }

        // Método para obtener todos los sistemas
        public async Task<List<SistemaWSViewModel>> ObtenerTodosLosSistemasAsync()
        {

            // Aquí se crea una lista para almacenar los sistemas
            var lista = new List<SistemaWSViewModel>();

            // Aquí se construye la consulta para obtener todos los sistemas
            string query = "SELECT * FROM Sistemas";

            // Se ejecuta la consulta y se obtiene el DataSet
            var dataSet = await _cnnConexionWSAsync.EjecutarSelectAsync(query);

            // Si el DataSet contiene tablas, se itera sobre las filas de la primera tabla
            if (dataSet.Tables.Count > 0)
            {

                // Iteramos sobre cada fila del DataSet
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {

                    // Creamos un nuevo objeto SistemaWSViewModel y lo llenamos con los datos de la fila
                    lista.Add(new SistemaWSViewModel
                    {
                        IdSistema = Convert.ToInt32(row["IdSistema"]),
                        NombreSistema = row["NombreSistema"].ToString(),
                        DescripcionSistema = row["DescripcionSistema"].ToString(),
                        IdEmpresa = Convert.ToInt32(row["IdEmpresa"])
                    });

                }

            }

            // Devolvemos la lista de sistemas
            return lista;

        }

        // Método para obtener un sistema por su ID
        public async Task<SistemaWSViewModel> ObtenerSistemaPorIdAsync(int Id)
        {

            // Aquí se construye la consulta para obtener un sistema por su ID
            string query = $"SELECT * FROM Sistemas WHERE IdSistema = {Id}";

            // Se ejecuta la consulta y se obtiene el DataSet
            var dataSet = await _cnnConexionWSAsync.EjecutarSelectAsync(query);

            // Si el DataSet no contiene tablas o la primera tabla no tiene filas, devolvemos null
            if (dataSet.Tables[0].Rows.Count == 0) return null;

            // Si hay filas, creamos un nuevo objeto SistemaWSViewModel y lo llenamos con los datos de la primera fila
            var row = dataSet.Tables[0].Rows[0];

            // Devolvemos el objeto SistemaWSViewModel con los datos del sistema
            return new SistemaWSViewModel
            {
                IdSistema = Convert.ToInt32(row["IdSistema"]),
                NombreSistema = row["NombreSistema"].ToString(),
                DescripcionSistema = row["DescripcionSistema"].ToString(),
                IdEmpresa = Convert.ToInt32(row["IdEmpresa"])
            };

        }

        // Método para insertar un nuevo sistema    
        public async Task<int> CrearSistemaAsync(SistemaWSViewModel sistema)
        {

            // Aquí se construye la consulta para insertar un nuevo sistema
            var parameteros = new[]
            {
                new SqlParameter("@NombreSistema", sistema.NombreSistema),
                new SqlParameter("@DescripcionSistema", sistema.DescripcionSistema),
                new SqlParameter("@IdEmpresa", sistema.IdEmpresa)
            };

        }


    }
}
