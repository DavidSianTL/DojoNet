using Microsoft.EntityFrameworkCore;
using ClinicaApi.Models;
using ClinicaApi.Data;
using ClinicaApi.Exceptions;

namespace ClinicaApi.DAO
{
    public class daoMedicoAsyncEF
    {
        private readonly ClinicaContext _context;

        public daoMedicoAsyncEF(ClinicaContext context)
        {
            _context = context;
        }

        public async Task<object> ObtenerMedicosAsync()
        {
            var medicos = await _context.Medicos
                .Include(m => m.MedicoEspecialidades)
                    .ThenInclude(me => me.Especialidad)
                .Include(m => m.Citas)
                .Select(m => new
                {
                    m.Id,
                    m.Nombre,
                    m.Email,
                    Especialidades = m.MedicoEspecialidades
                        .Select(me => me.EspecialidadId)
                        .ToList(),
                    Citas = m.Citas != null
                        ? m.Citas.Select(c => c.Id).ToList()
                        : new List<int>()
                })
                .ToListAsync();

            return medicos;
        }

        public async Task<object> ObtenerPorIdAsync(int id)
        {
            var medico = await _context.Medicos
                .Include(m => m.MedicoEspecialidades)
                    .ThenInclude(me => me.Especialidad)
                .Include(m => m.Citas)
                .Where(m => m.Id == id)
                .Select(m => new
                {
                    m.Id,
                    m.Nombre,
                    m.Email,
                    Especialidades = m.MedicoEspecialidades
                        .Select(me => me.EspecialidadId)
                        .ToList(),
                    Citas = m.Citas.Select(c => c.Id).ToList()
                })
                .FirstOrDefaultAsync();

            if (medico == null)
                throw new NotFoundException($"Médico con id {id} no encontrado.");

            return medico;
        }

        public async Task CrearMedicoAsync(Medico medico)
        {
            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarMedicoAsync(Medico medico)
        {
            var existente = await _context.Medicos.FindAsync(medico.Id);
            if (existente == null)
                throw new NotFoundException($"Médico con id {medico.Id} no encontrado.");

            existente.Nombre = medico.Nombre;
            existente.Email = medico.Email;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarMedicoAsync(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
                throw new NotFoundException($"Médico con id {id} no encontrado.");

            try
            {
                _context.Medicos.Remove(medico);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("DELETE") == true)
            {
                throw new InvalidOperationException("No se puede eliminar el médico porque tiene una o más citas asociadas.");
            }
        }
    }
}
