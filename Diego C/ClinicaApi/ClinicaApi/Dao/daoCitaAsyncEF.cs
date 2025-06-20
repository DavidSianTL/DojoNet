using ClinicaApi.Data;
using ClinicaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaApi.DAO
{
    public class daoCitaAsyncEF
    {
        private readonly ClinicaContext _context;

        public daoCitaAsyncEF(ClinicaContext context)
        {
            _context = context;
        }

        public async Task<List<Cita>> GetAllAsync()
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ThenInclude(m => m.Especialidad) // Para traer también la especialidad del médico
                .ToListAsync();
        }

        public async Task<Cita> GetByIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ThenInclude(m => m.Especialidad)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cita> CreateAsync(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return cita;
        }

        public async Task<bool> UpdateAsync(Cita cita)
        {
            var existing = await _context.Citas.FindAsync(cita.Id);
            if (existing == null)
                return false;

            existing.PacienteId = cita.PacienteId;
            existing.MedicoId = cita.MedicoId;
            existing.Fecha = cita.Fecha;
            existing.Hora = cita.Hora;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
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
