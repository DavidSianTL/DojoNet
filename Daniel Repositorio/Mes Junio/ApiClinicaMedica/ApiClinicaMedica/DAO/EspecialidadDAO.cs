using ApiClinicaMedica.Data;
using ApiClinicaMedica.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiClinicaMedica.Dao
{
    public class EspecialidadDAO
    {
        private readonly ClinicaDbContext _context;

        public EspecialidadDAO(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Especialidad>> ObtenerTodosAsync()
        {
            return await _context.Especialidades
                .Include(e => e.Medicos)
                    .ThenInclude(m => m.Citas)
                .ToListAsync();
        }

        public async Task<Especialidad?> ObtenerPorIdAsync(int id)
        {
            return await _context.Especialidades
                .Include(e => e.Medicos)
                .FirstOrDefaultAsync(e => e.IdEspecialidad == id);
        }

        public async Task CrearAsync(Especialidad especialidad)
        {
            _context.Especialidades.Add(especialidad);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var especialidad = await _context.Especialidades.FindAsync(id);
            if (especialidad == null) return false;

            _context.Especialidades.Remove(especialidad);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarAsync(int id, Especialidad especialidadActualizada)
        {
            var especialidad = await _context.Especialidades.FindAsync(id);
            if (especialidad == null) return false;

            especialidad.Nombre = especialidadActualizada.Nombre;
            await _context.SaveChangesAsync();
            return true;
        }

    }

}
