using ApiClinicaMedica.Data;
using ApiClinicaMedica.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaMedica.Dao
{
    public class PacienteDAO
    {
        private readonly ClinicaDbContext _context;

        public PacienteDAO(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Paciente>> ObtenerTodosAsync()
        {
            return await _context.Pacientes.ToListAsync();
        }

        public async Task<Paciente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Pacientes.FindAsync(id);
        }

        public async Task CrearAsync(Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ActualizarAsync(int id, Paciente paciente)
        {
            if (id != paciente.IdPaciente)
                return false;

            _context.Entry(paciente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pacientes.Any(p => p.IdPaciente == id))
                    return false;

                throw;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return false;

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
