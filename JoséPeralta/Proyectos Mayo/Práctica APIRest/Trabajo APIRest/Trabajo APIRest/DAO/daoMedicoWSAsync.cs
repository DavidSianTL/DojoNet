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

        // Obtiene una lista de médicos, incluyendo sus especialidades y citas
        public async Task<List<MedicoViewModel>> ObtenerMedicosAsync()
        {
            var resultados = await _context.Medicos
            .Include(m => m.Especialidad)
            .Include(m => m.Citas)
            .Select(m => new MedicoViewModel
            {
                // Propiedades del médico
                IdMedico = m.IdMedico,
                Nombre = m.Nombre,
                Fk_IdEspecialidad = m.Fk_IdEspecialidad,
                Email = m.Email,

                // Relación con Especialidad
                Especialidad = m.Especialidad == null ? null : new EspecialidadViewModel
                {
                    // Propiedades de la especialidad
                    IdEspecialidad = m.Especialidad.IdEspecialidad,
                    Nombre = m.Especialidad.Nombre
                },

                // Relación con Citas
                Citas = m.Citas == null ? null : m.Citas.Select(c => new CitaViewModel
                {
                    // Propiedades de la cita
                    IdCita = c.IdCita,
                    Fk_IdPaciente = c.Fk_IdPaciente,
                    Fk_IdMedico = c.Fk_IdMedico,
                    Fecha = c.Fecha,
                    Hora = c.Hora
                }).ToList()
            }).ToListAsync();

            if(resultados == null || resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron médicos en la base de datos.");
                return new List<MedicoViewModel>();
            }

            return resultados;
        }

        // Obtiene un médico por su ID, incluyendo sus especialidad y citas
        public async Task<MedicoViewModel> ObtenerMedicoPorIdAsync(int id)
        {
            // Vamos a buscar un médico por su ID, incluyendo sus especialidad y citas
            var medico = await _context.Medicos
                .Include(m => m.Especialidad)
                .Include(m => m.Citas)
                .FirstOrDefaultAsync(m => m.IdMedico == id);

            // Si no se encuentra el médico, retornamos null
            if (medico == null)
                throw new NotFoundException($"Médico con id {medico.IdMedico} no encontrado.");

            // Si se encuentra, mapeamos el resultado a MedicoViewModel
            return new MedicoViewModel
            {
                // Propiedades del médico
                IdMedico = medico.IdMedico,
                Nombre = medico.Nombre,
                Fk_IdEspecialidad = medico.Fk_IdEspecialidad,
                Email = medico.Email,

                // Relación con Especialidad
                Especialidad = medico.Especialidad == null ? null : new EspecialidadViewModel
                {
                    // Propiedades de la especialidad
                    IdEspecialidad = medico.Especialidad.IdEspecialidad,
                    Nombre = medico.Especialidad.Nombre
                },

                // Relación con Citas
                Citas = medico.Citas?.Select(c => new CitaViewModel
                {
                    // Propiedades de la cita
                    IdCita = c.IdCita,
                    Fk_IdPaciente = c.Fk_IdPaciente,
                    Fk_IdMedico = c.Fk_IdMedico,
                    Fecha = c.Fecha,
                    Hora = c.Hora
                }).ToList()

            };
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
            medicoExiste.Fk_IdEspecialidad = model.Fk_IdEspecialidad;
            medicoExiste.Email = model.Email;

            await _context.SaveChangesAsync();

            // Recuperamos el médico actualizado con sus relaciones
            var actualizado = await _context.Medicos
                .Include(m => m.Especialidad)
                .Include(m => m.Citas)
                .FirstOrDefaultAsync(m => m.IdMedico == model.IdMedico);

            // Lo devolvemos ya mapeado a MedicoViewModel
            return actualizado!;
        }

        // Elimina un médico por su ID
        public async Task EliminarMedicoAsync(int id)
        {
            // Recuperamos el médico por su ID incluyendo sus citas
            var medico = await _context.Medicos
                .Include(m => m.Citas)
                .FirstOrDefaultAsync(m => m.IdMedico == id);

            // Verificamos si el médico existe
            if (medico == null)
                throw new NotFoundException($"Médico con id {id} no encontrado.");

            // Verificamos si el médico tiene citas asignadas
            if (medico.Citas != null && medico.Citas.Any())
            {
                throw new InvalidOperationException(
                    $"No se puede eliminar el médico con ID {id} porque tiene citas asignadas. " +
                    "Por favor, elimine primero las citas asociadas a este médico.");
            }

            // Si no tiene citas, procedemos con la eliminación
            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
        }

    }
}
