using ClinicaApi.Data;
using ClinicaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaApi.DAO
{
    public class daoEspecialidadAsyncEF
    {
        private readonly ClinicaContext _context;

        public daoEspecialidadAsyncEF(ClinicaContext context)
        {
            _context = context;
        }

        public async Task<List<Especialidad>> GetAllAsync()
        {
            return await _context.Especialidades
                .Include(e => e.Medicos)  // Incluye los médicos relacionados si quieres
                .ToListAsync();
        }

        public async Task<Especialidad> GetByIdAsync(int id)
        {
            return await _context.Especialidades
                .Include(e => e.Medicos)  // Opcional: incluir médicos
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Especialidad> CreateAsync(Especialidad especialidad)
        {
            _context.Especialidades.Add(especialidad);
            await _context.SaveChangesAsync();
            return especialidad;
        }

        public async Task<bool> UpdateAsync(Especialidad especialidad)
        {
            var existing = await _context.Especialidades.FindAsync(especialidad.Id);
            if (existing == null)
                return false;

            existing.Nombre = especialidad.Nombre;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var especialidad = await _context.Especialidades.FindAsync(id);
            if (especialidad == null)
                return false;

            _context.Especialidades.Remove(especialidad);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
