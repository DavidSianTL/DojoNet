using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trabajo_APIRest.Models;
using UsuariosApi.Exceptions;

namespace Trabajo_APIRest.Data
{
    public class daoEspecialidadWSASync
    {
        private readonly AppDbContext _context;
        public daoEspecialidadWSASync(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene una lista de especialidades
        public async Task<List<EspecialidadViewModel>> ObtenerEspecialidadesAsync()
        {
            var resultados = await _context.Especialidades
            .Include(p => p.Medicos)
            .Select(p => new EspecialidadViewModel
            {
                // Propiedades del paciente
                IdEspecialidad = p.IdEspecialidad,
                Nombre = p.Nombre,

                // Relación con Citas
                Medicos = p.Medicos == null ? null : p.Medicos.Select(c => new MedicoViewModel
                {
                    // Propiedades de la cita
                    IdMedico = c.IdMedico
                }).ToList()

            }).ToListAsync();

            if (resultados == null || resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron especialidades en la base de datos.");
                return new List<EspecialidadViewModel>();
            }

            return resultados;
        }

        // Obtiene una especialidad por su ID, incluyendo sus citas
        public async Task<EspecialidadViewModel> ObtenerEspecialidadPorIdAsync(int id)
        {
            // Vamos a buscar una especialidad por su ID, incluyendo sus citas
            var especialidad = await _context.Especialidades
                .Include(p => p.Medicos)
                .FirstOrDefaultAsync(p => p.IdEspecialidad == id);

            // Si no se encuentra el paciente, retornamos null
            if (especialidad == null)
                throw new NotFoundException($"Especialidad con id {especialidad.IdEspecialidad} no encontrado.");

            // Si se encuentra, mapeamos el resultado a PacienteViewModel
            return new EspecialidadViewModel
            {
                // Propiedades del paciente
                IdEspecialidad = especialidad.IdEspecialidad,
                Nombre = especialidad.Nombre,

                // Relaci�n con Citas
                Medicos = especialidad.Medicos?.Select(c => new MedicoViewModel
                {
                    // Propiedades de la cita
                    IdMedico = c.IdMedico
                }).ToList()

            };
        }

        // Crea un nuevo especialidad
        public async Task CrearEspecialidadAsync(EspecialidadViewModel model)
        {
            // Mapeamos el modelo a la entidad Paciente
            _context.Especialidades.Add(model);

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();
        }

        // Actualiza una especialidad existente por su ID
        public async Task<EspecialidadViewModel> ActualizarEspecialidadAsync(EspecialidadViewModel model)
        {
            var especialidadExiste = await _context.Especialidades.FindAsync(model.IdEspecialidad);

            if (especialidadExiste == null)
                throw new NotFoundException($"Especialidad con id {model.IdEspecialidad} no encontrado.");

            especialidadExiste.Nombre = model.Nombre;

            await _context.SaveChangesAsync();

            // Recuperamos el paciente actualizado con sus relaciones
            var actualizado = await _context.Especialidades
                .Include(p => p.Medicos)
                .FirstOrDefaultAsync(p => p.IdEspecialidad == model.IdEspecialidad);

            // Lo devolvemos ya mapeado a PacienteViewModel
            return actualizado!;
        }

        // Elimina una especialidad por su ID
        public async Task EliminarEspecialidadAsync(int id)
        {
            // Recuperamos la especialidad por su ID incluyendo los médicos asociados
            var especialidad = await _context.Especialidades
                .Include(e => e.Medicos)
                .FirstOrDefaultAsync(e => e.IdEspecialidad == id);

            // Verificamos si la especialidad existe
            if (especialidad == null)
                throw new NotFoundException($"Especialidad con id {id} no encontrada.");

            // Verificamos si la especialidad tiene médicos asignados
            if (especialidad.Medicos != null && especialidad.Medicos.Any())
            {
                throw new InvalidOperationException(
                    $"No se puede eliminar la especialidad con ID {id} porque tiene médicos asignados. " +
                    "Por favor, actualice o elimine primero los médicos asociados a esta especialidad.");
            }

            // Si no tiene médicos, procedemos con la eliminación
            _context.Especialidades.Remove(especialidad);
            await _context.SaveChangesAsync();
        }

    }
}
