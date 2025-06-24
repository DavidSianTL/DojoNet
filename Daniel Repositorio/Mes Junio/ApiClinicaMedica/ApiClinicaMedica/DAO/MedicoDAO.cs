using ApiClinicaMedica.Data;
using ApiClinicaMedica.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaMedica.Dao
{
    public class MedicoDAO
    {
        private readonly ClinicaDbContext _context;

        public MedicoDAO(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Medico>> ObtenerMedicosAsync()
        {
            return await _context.Medicos
                .Include(m => m.Especialidad)
                .ToListAsync();
        }

        public async Task<Medico?> ObtenerPorIdAsync(int id)
        {
            return await _context.Medicos
                .Include(m => m.Especialidad)
                .FirstOrDefaultAsync(m => m.IdMedico == id);
        }

        public async Task CrearAsync(Medico medico)
        {
            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ActualizarAsync(int id, Medico medico)
        {
            if (id != medico.IdMedico)
                return false;

            _context.Entry(medico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Medicos.Any(m => m.IdMedico == id))
                    return false;

                throw;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null) return false;

            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
