using ApiClinicaMedica.Data;
using ApiClinicaMedica.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaMedica.Daos
{
    public class CitaDao
    {
        private readonly ClinicaDbContext _context;

        public CitaDao(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cita>> ListarAsync()
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ToListAsync();
        }

        public async Task InsertarAsync(Cita c)
        {
            _context.Citas.Add(c);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ActualizarAsync(Cita c)
        {
            if (!_context.Citas.Any(x => x.IdCita == c.IdCita))
                return false;

            _context.Entry(c).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
                return false;

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
