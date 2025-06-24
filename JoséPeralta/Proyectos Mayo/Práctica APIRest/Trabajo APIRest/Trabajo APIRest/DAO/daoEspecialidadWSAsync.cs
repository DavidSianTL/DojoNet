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
            // Obtenemos todas las especialidades de la base de datos
            return await _context.Especialidades
                // Usamos AsNoTracking para evitar la carga de datos innecesarios
                .AsNoTracking()
                // Convertimos el resultado a una lista
                .ToListAsync();
        }

        // Obtiene una especialidad por su ID
        public async Task<EspecialidadViewModel> ObtenerEspecialidadPorIdAsync(int id)
        {
            var especialidad = await _context.Especialidades
                // Usamos AsNoTracking para evitar la carga de datos innecesarios
                .AsNoTracking()
                // Buscamos la especialidad por su ID
                .FirstOrDefaultAsync(e => e.IdEspecialidad == id);

            if (especialidad == null)
                throw new KeyNotFoundException($"Especialidad con ID {id} no encontrada.");

            return especialidad;
    }

        // Crea un nuevo especialidad
        public async Task CrearEspecialidadAsync(EspecialidadViewModel model)
        {
            // Mapeamos el modelo a la entidad especialidad
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

            // Recuperamos la especialidad actualizada
            var actualizado = await _context.Especialidades
                .FirstOrDefaultAsync(p => p.IdEspecialidad == model.IdEspecialidad);

            // Lo devolvemos ya mapeado a EspecialidadViewModel
            return actualizado!;
        }

        // Elimina una especialidad por su ID
        public async Task EliminarEspecialidadAsync(int id)
        {
            // Recuperamos la especialidad por su ID
            var especialidad = await _context.Especialidades
                .FirstOrDefaultAsync(e => e.IdEspecialidad == id);

            // Verificamos si la especialidad existe
            if (especialidad == null)
                throw new NotFoundException($"Especialidad con id {id} no encontrada.");

            // Procedemos con la eliminación
            _context.Especialidades.Remove(especialidad);
            await _context.SaveChangesAsync();
        }

    }
}
