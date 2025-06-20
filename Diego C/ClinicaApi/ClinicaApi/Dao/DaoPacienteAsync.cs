using Microsoft.EntityFrameworkCore;
using ClinicaApi.Models;
using ClinicaApi.Data;
using ClinicaApi.Exceptions;

namespace ClinicaApi.DAO
{
    public class daoPacienteAsyncEF
    {
        private readonly ClinicaContext _context;

        public daoPacienteAsyncEF(ClinicaContext context)
        {
            _context = context;
        }

        public async Task<List<Paciente>> ObtenerPacientesAsync()
        {
            return await _context.Pacientes.ToListAsync();
        }

        public async Task<Paciente> ObtenerPorIdAsync(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
                throw new NotFoundException($"Paciente con id {id} no encontrado.");
            return paciente;
        }

        public async Task CrearPacienteAsync(Paciente paciente)
        {
            paciente.FechaCreacion = DateTime.Now;
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarPacienteAsync(Paciente paciente)
        {
            var existente = await _context.Pacientes.FindAsync(paciente.Id);
            if (existente == null)
                throw new NotFoundException($"Paciente con id {paciente.Id} no encontrado.");

            existente.Nombre = paciente.Nombre;
            existente.Email = paciente.Email;
            existente.Telefono = paciente.Telefono;
            existente.FechaNacimiento = paciente.FechaNacimiento;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarPacienteAsync(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
                throw new NotFoundException($"Paciente con id {id} no encontrado.");

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
        }
    }
}
