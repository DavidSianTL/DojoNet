using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using HojadeTrabajoAPI_REST.Models;
using HojadeTrabajoAPI_REST.DATA;
using HojadeTrabajoAPI_REST.Exceptions;

namespace HojadeTrabajoAPI_REST.DAO
{
    public class daoMedicoAsync
    {
        private readonly DbConnection _db;

        public daoMedicoAsync(DbConnection db)
        {
            _db = db;

        }
        //Metodo para obtener todo el listado de medicos de la bd
        public async Task<List<Medico>> ObtenerMedicosAsync()
        {
            var ListaMedicos = new List<Medico>();
            string query = "";

            query = $@"SELECT m.Id as Id_medico,
                        m.Nombre as Nombre, 
                        m.Id_Especialidad as Id_Especialidad, 
                        m.Email as Email
                    FROM Medicos m
                    INNER JOIN Especialidades e ON m.Id_Especialidad = e.Id";

            using var cnn = _db.GetConnection();
            try
            {
                await cnn.OpenAsync();
                var cmd = new SqlCommand(query, cnn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ListaMedicos.Add(new Medico
                    {
                        IdMedico= (int)reader["Id_Medico"],
                        Nombre = reader["Nombre"].ToString(),
                        FK_Id_Especialidad = (int)reader["Id_Especialidad"],
                        Email = reader["Email"].ToString()  
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL OBTENER EL LISTADO DE MEDICOS." + ex.Message, ex);

            }
            return ListaMedicos;
        }

        //Metodo para insertar un medico en la bd
        public async Task InsertarMedicoAsync(Medico medico)
        {
            string query = "";

            query = $@"INSERT INTO Medicos (
                        Nombre,
                        Id_Especialidad,
                        Email)
                    VALUES (@Nombre,
                        @Id_Especialidad,
                        @Email)";
            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", medico.Nombre);
                cmd.Parameters.AddWithValue("@Id_Especialidad", medico.FK_Id_Especialidad);
                cmd.Parameters.AddWithValue("@Email", medico.Email);

                await cmd.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL INSERTAR EL MEDICO." + ex.Message, ex);

            }
        }

        //Metodo para actualizar un medico en la bd
        public async Task ActualizarMedicoAsync(Medico medico)
        {
            string query = "";
            int filasAfectadas = 0;

            query = $@"UPDATE Medicos SET
                        Nombre = @Nombre,
                        Id_Especialidad = @Id_Especialidad,
                        Email = @Email
                    WHERE Id= @Id";

            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", medico.IdMedico);
                cmd.Parameters.AddWithValue("@Nombre", medico.Nombre);
                cmd.Parameters.AddWithValue("@Id_Especialidad", medico.FK_Id_Especialidad);
                cmd.Parameters.AddWithValue("@Email", medico.Email);

                filasAfectadas = await cmd.ExecuteNonQueryAsync();
                if (filasAfectadas == 0)
                {
                    throw new NotFoundException($"El medico con id {medico.IdMedico} no se encontró.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL ACTUALIZAR LOS MEDICOS." + ex.Message, ex);

            }
        }

        public async Task EliminarMedicoAsync(int id)
        {
            string query = "";
            int filasAfectadas = 0;

            query = $@"DELETE FROM Medicos WHERE Id = @Id";

            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                filasAfectadas = await cmd.ExecuteNonQueryAsync();

                if (filasAfectadas == 0)
                {
                    throw new NotFoundException($"Medico con id {id} no encontrado.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL ELIMINAR MEDICOS." + ex.Message, ex);

            }
        }

    }
}
