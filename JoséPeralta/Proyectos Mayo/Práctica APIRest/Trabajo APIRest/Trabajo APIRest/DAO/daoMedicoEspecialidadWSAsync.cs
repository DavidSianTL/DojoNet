using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trabajo_APIRest.Data;
using Trabajo_APIRest.Models;

namespace Trabajo_APIRest.DAO
{
    public class daoMedicoEspecialidadWSAsync
    {
        private readonly AppDbContext _context;

        public daoMedicoEspecialidadWSAsync(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todas las relaciones
        public async Task<List<MedicoEspecialidad>> ObtenerMedicoEspecialidadAsync()
        {
            return await _context.MedicoEspecialidades.ToListAsync();
        }

        // Obtener todas las especialidades de un médico
        public async Task<List<EspecialidadViewModel>> ObtenerEspecialidadesPorMedicoAsync(int medicoId)
        {
            // Verificar si el médico existe
            var medico = await _context.Medicos
                .FirstOrDefaultAsync(m => m.IdMedico == medicoId);

            // Si el médico no existe, devolver null
            if (medico == null)
                return null;

            // Obtener las especialidades del médico
            return await _context.MedicoEspecialidades
                // Hacemos referencia a la tabla MedicoEspecialidad
                .Where(me => me.MedicoId == medicoId)
                // Hacemos referencia a la tabla Especialidad
                .Select(me => me.Especialidad)
                // Convertimos el resultado en una lista
                .ToListAsync();
        }

        // Agregar una especialidad a un médico
        public async Task<MedicoEspecialidad> AgregarEspecialidadAMedicoAsync(int medicoId, int especialidadId)
        {
            // Verificar si la relación ya existe
            var existe = await _context.MedicoEspecialidades
                .AnyAsync(me => me.MedicoId == medicoId && me.EspecialidadId == especialidadId);

            if (existe)
                return null;

            var relacion = new MedicoEspecialidad
            {
                MedicoId = medicoId,
                EspecialidadId = especialidadId
            };

            _context.MedicoEspecialidades.Add(relacion);
            await _context.SaveChangesAsync();

            // Incluir datos relacionados
            return await _context.MedicoEspecialidades
                .Include(me => me.Medico)
                .Include(me => me.Especialidad)
                .FirstOrDefaultAsync(me => me.IdMedicoEspecialidad == relacion.IdMedicoEspecialidad);
        }

        // Actualizar las especialidades de un médico (reemplaza todas las existentes)
        public async Task<MedicoEspecialidad> ActualizarRelacionMedicoEspecialidadAsync(int idRelacion, int nuevaEspecialidadId)
        {
            // Buscar la relación existente
            var relacion = await _context.MedicoEspecialidades
                .Include(me => me.Medico)
                .Include(me => me.Especialidad)
                .FirstOrDefaultAsync(me => me.IdMedicoEspecialidad == idRelacion);

            if (relacion == null)
                return null;

            // Verificar si ya existe la nueva relación para el mismo médico
            bool existe = await _context.MedicoEspecialidades
                .AnyAsync(me => me.MedicoId == relacion.MedicoId &&
                               me.EspecialidadId == nuevaEspecialidadId &&
                               me.IdMedicoEspecialidad != idRelacion);

            if (existe)
                return null;

            // Actualizar solo la especialidad
            relacion.EspecialidadId = nuevaEspecialidadId;
            await _context.SaveChangesAsync();

            // Recargar la entidad con los datos actualizados
            return await _context.MedicoEspecialidades
                .Include(me => me.Medico)
                .Include(me => me.Especialidad)
                .FirstOrDefaultAsync(me => me.IdMedicoEspecialidad == idRelacion);
        }

        // Eliminar una especialidad de un médico
        public async Task<bool> EliminarRelacionAsync(int id)
        {
            // Buscar la relación
            var relacion = await _context.MedicoEspecialidades
            .FirstOrDefaultAsync(me => me.IdMedicoEspecialidad == id);

            // Si la relación no existe, devolver false
            if (relacion == null)
                return false;

            // Eliminar la relación
            _context.MedicoEspecialidades.Remove(relacion);
            // Guardar los cambios
            await _context.SaveChangesAsync();
            return true;
        }
        
    }
}
