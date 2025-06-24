using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trabajo_APIRest.Models;
using UsuariosApi.Exceptions;

namespace Trabajo_APIRest.Data
{
    public class daoMedicoWSAsync
    {
        private readonly AppDbContext _context;
        public daoMedicoWSAsync(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene una lista de médicos
        public async Task<List<MedicoViewModel>> ObtenerMedicosAsync()
        {
            var resultados = await _context.Medicos
            .Select(m => new MedicoViewModel
            {
                // Propiedades del médico
                IdMedico = m.IdMedico,
                Nombre = m.Nombre,
                Email = m.Email

            }).ToListAsync();

            if(resultados == null || resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron médicos en la base de datos.");
                return new List<MedicoViewModel>();
            }

            return resultados;
        }

        // Obtiene un médico por su ID
        public async Task<MedicoViewModel> ObtenerMedicoPorIdAsync(int id)
        {
            // Primero obtenemos solo el médico sin relaciones
            var medico = await _context.Medicos
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdMedico == id);

            if (medico == null) return null;

            // Luego cargamos manualmente las especialidades
            medico.MedicoEspecialidades = await _context.MedicoEspecialidades
                .AsNoTracking()
                .Include(me => me.Especialidad)
                .Where(me => me.MedicoId == id)
                .ToListAsync();
                
            // Y las citas
            medico.Citas = await _context.Citas
                .AsNoTracking()
                .Where(c => c.Fk_IdMedico == id)
                .ToListAsync();

            // Luego enviamos el médico mapeado a MedicoViewModel para devolver el resultado
            return medico;
        }

        // Crea un nuevo médico
        public async Task CrearMedicoAsync(MedicoViewModel model)
        {
            // Mapeamos el modelo a la entidad Medico
            _context.Medicos.Add(model);

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();
        }

        // Actualiza un médico existente por su ID
        public async Task<MedicoViewModel> ActualizarMedicoAsync(MedicoViewModel model)
        {
            var medicoExiste = await _context.Medicos.FindAsync(model.IdMedico);

            if (medicoExiste == null)
                throw new NotFoundException($"Médico con id {model.IdMedico} no encontrado.");

            medicoExiste.Nombre = model.Nombre;
            medicoExiste.Email = model.Email;

            await _context.SaveChangesAsync();

            // Recuperamos el médico actualizado con sus relaciones
            var actualizado = await _context.Medicos
                .FirstOrDefaultAsync(m => m.IdMedico == model.IdMedico);

            // Lo devolvemos ya mapeado a MedicoViewModel
            return actualizado!;
        }

        // Elimina un médico por su ID
        public async Task EliminarMedicoAsync(int id)
        {
            // Recuperamos el médico por su ID incluyendo sus citas y especialidades
            var medico = await _context.Medicos
                .FirstOrDefaultAsync(m => m.IdMedico == id);

            // Finalmente, eliminamos el médico
            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
        }

    }
}
