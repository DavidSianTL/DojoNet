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

        public async Task<object> ObtenerPacientesAsync()
        {
            var pacientes = await _context.Pacientes
                .Include(p => p.Citas)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Email,
                    p.Telefono,
                    p.FechaNacimiento,
                    p.FechaCreacion,
                    Citas = p.Citas.Select(c => c.Id).ToList()
                })
                .ToListAsync();

            return pacientes;
        }

        public async Task<object> ObtenerPorIdAsync(int id)
        {
            var paciente = await _context.Pacientes
                .Include(p => p.Citas)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Email,
                    p.Telefono,
                    p.FechaNacimiento,
                    p.FechaCreacion,
                    Citas = p.Citas.Select(c => c.Id).ToList()
                })
                .FirstOrDefaultAsync();

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
                throw new NotFoundException("Paciente no encontrado");

            try
            {
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("DELETE") == true)
            {
                // Mensaje personalizado si tiene citas asociadas
                throw new InvalidOperationException("No se puede eliminar el paciente porque tiene una o más citas asociadas.");
            }
        }
    }
}
