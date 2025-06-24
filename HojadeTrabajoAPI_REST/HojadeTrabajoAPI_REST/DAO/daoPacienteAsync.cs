using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using HojadeTrabajoAPI_REST.Models;
using HojadeTrabajoAPI_REST.DATA;
using HojadeTrabajoAPI_REST.Exceptions;

namespace HojadeTrabajoAPI_REST.DAO
{
    public class daoPacienteAsync
    {
        private readonly DbConnection _db;

        public daoPacienteAsync(DbConnection db)
        {
            _db = db;

        }
        //Metodo para obtener todo el listado de pacientes de la bd
        public async Task<List<Paciente>> ObtenerPacientesAsync()
        {
            var ListaPacientes = new List<Paciente>();
            string query = "";

            query = $@"SELECT Id,
                        Nombre, 
                        Email, 
                        Telefono, 
                        FechaNacimiento
                    FROM Pacientes";

            using var cnn = _db.GetConnection();
            try
            {
                await cnn.OpenAsync();
                var cmd = new SqlCommand(query, cnn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    ListaPacientes.Add(new Paciente
                    {
                        IdPaciente = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        FechaNacimiento= (DateTime)reader["FechaNacimiento"]
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL OBTENER EL LISTADO DE PACIENTES." + ex.Message, ex);

            }
            return ListaPacientes;
        }

        //Metodo para insertar un paciente en la bd
        public async Task InsertarPacienteAsync(Paciente paciente)
        {
            string query = "";

            query = $@"INSERT INTO Pacientes (
                        Nombre,
                        Email,
                        Telefono,
                        FechaNacimiento)
                    VALUES (@Nombre, @Email, @Telefono , @FechaNacimiento)";
            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", paciente.Nombre);
                cmd.Parameters.AddWithValue("@Email", paciente.Email);
                cmd.Parameters.AddWithValue("@Telefono", paciente.Telefono);
                cmd.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento.Date);

                await cmd.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL INSERTAR EL PACIENTE." + ex.Message, ex);

            }
        }

        //Metodo para actualizar un paciente en la bd
        public async Task ActualizarPacienteAsync(Paciente paciente)
        {
            string query = "";
            int filasAfectadas = 0;

            query = $@"UPDATE Pacientes SET
                        Nombre = @Nombre,
                        Email = @Email,
                        Telefono = @Telefono,
                        FechaNacimiento = @FechaNacimiento
                    WHERE Id= @Id";

            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", paciente.IdPaciente);
                cmd.Parameters.AddWithValue("@Nombre", paciente.Nombre);
                cmd.Parameters.AddWithValue("@Email", paciente.Email);
                cmd.Parameters.AddWithValue("@Telefono", paciente.Telefono);
                cmd.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento);

                filasAfectadas = await cmd.ExecuteNonQueryAsync();
                if (filasAfectadas == 0)
                {
                    throw new NotFoundException($"El paciente con id {paciente.IdPaciente} no se encontró.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL ACTUALIZAR LOS PACIENTES." + ex.Message, ex);

            }
        }

        public async Task EliminarPacienteAsync(int id)
        {
            string query = "";
            int filasAfectadas = 0;

            query = $@"DELETE FROM Pacientes WHERE Id = @Id";

            try
            {
                using var conn = _db.GetConnection();
                await conn.OpenAsync();
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                filasAfectadas = await cmd.ExecuteNonQueryAsync();

                if (filasAfectadas == 0)
                {
                    throw new NotFoundException($"Paciente con id {id} no encontrado.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR: AL ELIMINAR PACIENTES." + ex.Message, ex);

            }
        }
    }
}
