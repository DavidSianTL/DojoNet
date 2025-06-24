using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using HojadeTrabajoAPI_REST.Models;
using HojadeTrabajoAPI_REST.DATA;
using HojadeTrabajoAPI_REST.Exceptions;
using System.Reflection;

namespace HojadeTrabajoAPI_REST.DAO
{
    public class daoCitaAsync
    {
        private readonly DbConnection _db;

        public daoCitaAsync(DbConnection db)
        {
            _db = db;

        }
        //Metodo para obtener todo el listado de citas programadas de la bd
        public async Task<List<Cita>> ObtenerCitasAsync()
        {
            var ListaCitas = new List<Cita>();
            string query = "";

            query = $@"SELECT c.Id as Id_Cita,
                        p.Id as Id_Paciente, 
                        m.Id as Id_Medico, 
                        c.Fecha as Fecha,
                        c.Hora as Hora
                    FROM Citas c
                    INNER JOIN Pacientes p ON c.Id_Paciente= p.Id
                    INNER JOIN Medicos m ON c.Id_Medico = m.Id";

            using var cnn = _db.GetConnection();
            try
            {
                await cnn.OpenAsync();
                var cmd = new SqlCommand(query, cnn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ListaCitas.Add(new Cita
                    {
                        IdCita = (int)reader["Id_Cita"],
                        FK_Id_Paciente = (int)reader["Id_Paciente"],
                        FK_Id_Medico = (int)reader["Id_Medico"],
                        Fecha = (DateTime)reader["Fecha"],
                        Hora = (TimeSpan)reader["Hora"]
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL OBTENER EL LISTADO DE CITAS." + ex.Message, ex);

            }
            return ListaCitas;
        }

        //Metodo para insertar una cita en la bd
        public async Task InsertarCitaAsync(Cita cita)
        {
            string query = "";

            query = $@"INSERT INTO Citas (
                        Id_Paciente
                        ,Id_Medico
                        ,Fecha
                        ,Hora)
                    VALUES (@IdPaciente
                        , @IdMedico
                        , @Fecha
                        , @Hora)";
            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdPaciente", cita.FK_Id_Paciente);
                cmd.Parameters.AddWithValue("@IdMedico", cita.FK_Id_Medico);
                cmd.Parameters.AddWithValue("@Fecha", cita.Fecha);
                cmd.Parameters.AddWithValue("@Hora", cita.Hora);

                await cmd.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL INSERTAR LA CITA" + ex.Message, ex);

            }
        }

        //Metodo para actualizar una cita en la bd
        public async Task ActualizarCitaAsync(Cita cita)
        {
            string query = "";
            int filasAfectadas = 0;

            query = $@"UPDATE Citas SET
                        Id_Paciente = @IdPaciente,
                        Id_Medico = @IdMedico,
                        Fecha = @Fecha,
                        Hora = @Hora
                    WHERE Id= @IdCita";

            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdCita", cita.IdCita);
                cmd.Parameters.AddWithValue("@IdPaciente", cita.FK_Id_Paciente);
                cmd.Parameters.AddWithValue("@IdMedico", cita.FK_Id_Medico);
                cmd.Parameters.AddWithValue("@Fecha", cita.Fecha);
                cmd.Parameters.AddWithValue("@Hora", cita.Hora);


                filasAfectadas = await cmd.ExecuteNonQueryAsync();
                if (filasAfectadas == 0)
                {
                    throw new NotFoundException($"La cita con id {cita.IdCita} no se encontró.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL ACTUALIZAR LAS CITAS." + ex.Message, ex);

            }
        }

        public async Task EliminarCitaAsync(int id)
        {
            string query = "";
            int filasAfectadas = 0;

            query = $@"DELETE FROM Citas WHERE Id = @IdCita";

            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdCita", id);

                filasAfectadas = await cmd.ExecuteNonQueryAsync();

                if (filasAfectadas == 0)
                {
                    throw new NotFoundException($"Cita con id {id} no encontrada.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL ELIMINAR CITAS." + ex.Message, ex);

            }
        }
    }
}
