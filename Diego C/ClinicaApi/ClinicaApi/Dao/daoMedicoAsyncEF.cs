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

        public async Task<List<Medico>> ObtenerMedicosAsync()
        {
            return await _context.Medicos
                .Include(m => m.Especialidad)
                .Include(m => m.Citas)
                    .ThenInclude(c => c.Paciente)
                //.Include(m => m.Citas) // esta línea que incluye el medico se quita
                //.ThenInclude(c => c.Medico) // esta línea también se quita
                .ToListAsync();
        }
        public async Task<Medico> ObtenerPorIdAsync(int id)
        {
            var medico = await _context.Medicos
                .Include(m => m.Especialidad)
                .FirstOrDefaultAsync(m => m.Id == id);

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
            existente.EspecialidadId = medico.EspecialidadId;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarMedicoAsync(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
                throw new NotFoundException($"Médico con id {id} no encontrado.");

            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
        }
    }
}
