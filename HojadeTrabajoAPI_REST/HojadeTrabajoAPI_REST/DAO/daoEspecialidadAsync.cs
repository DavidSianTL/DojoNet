using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using HojadeTrabajoAPI_REST.Models;
using HojadeTrabajoAPI_REST.DATA;
using HojadeTrabajoAPI_REST.Exceptions;

namespace HojadeTrabajoAPI_REST.DAO
{
    public class daoEspecialidadAsync
    {
        private readonly DbConnection _db;

        public daoEspecialidadAsync(DbConnection db)
        {
            _db = db;

        }
        //Metodo para obtener todo el listado de especialides de la bd
        public async Task<List<Especialidad>> ObtenerEspecialidadesAsync()
        {
            var ListaEspecialidades = new List<Especialidad>();
            string query = "";

            query = "SELECT Id, Nombre FROM Especialidades";

            using var cnn = _db.GetConnection();
            try
            {
                await cnn.OpenAsync();
                var cmd = new SqlCommand(query, cnn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ListaEspecialidades.Add(new Especialidad
                    {
                        IdEspecialidad = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL OBTENER EL LISTADO DE ESPECIALIDADES." + ex.Message, ex);

            }
            return ListaEspecialidades;
        }

        //Metodo para insertar un paciente en la bd
        public async Task InsertarEspecialidadAsync(Especialidad especialidad)
        {
            string query = "";

            query = "INSERT INTO Especialidades (Nombre) VALUES (@Nombre)";

            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", especialidad.Nombre);
   
                await cmd.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL INSERTAR LA ESPECIALIDAD." + ex.Message, ex);

            }
        }

        //Metodo para actualizar un especialidad en la bd
        public async Task ActualizarEspecialidadAsync(Especialidad especialidad)
        {
            string query = "";
            int filasAfectadas = 0;

            query = "UPDATE Especialidades SET Nombre = @Nombre WHERE Id = @Id";

            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", especialidad.IdEspecialidad);
                cmd.Parameters.AddWithValue("@Nombre", especialidad.Nombre);
            
                filasAfectadas = await cmd.ExecuteNonQueryAsync();
                if (filasAfectadas == 0)
                {
                    throw new NotFoundException($"La especialidad con id {especialidad.IdEspecialidad} no se encontró.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL ACTUALIZAR LAS ESPECIALIDADES." + ex.Message, ex);

            }
        }

        public async Task EliminarEspecialidadAsync(int id)
        {
            string query = "";
            int filasAfectadas = 0;

            query = $@"DELETE FROM Especialidades WHERE Id = @Id";

            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                filasAfectadas = await cmd.ExecuteNonQueryAsync();

                if (filasAfectadas == 0)
                {
                    throw new NotFoundException($"Especialidad con id {id} no encontrada.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL ELIMINAR ESPECIALIDADES." + ex.Message, ex);

            }
        }
    }
}
