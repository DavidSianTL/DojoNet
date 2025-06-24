using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trabajo_APIRest.Models;
using UsuariosApi.Exceptions;

namespace Trabajo_APIRest.Data
{
    public class daoCitaWSAsync
    {
        private readonly AppDbContext _context;
        public daoCitaWSAsync(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene una lista de citas
        public async Task<List<CitaViewModel>> ObtenerCitasAsync()
        {
            var resultados = await _context.Citas
            .Select(p => new CitaViewModel
            {
                // Propiedades del cita
                IdCita = p.IdCita,
                Fk_IdPaciente = p.Fk_IdPaciente,
                Fk_IdMedico = p.Fk_IdMedico,
                Fecha = p.Fecha,
                Hora = p.Hora

            }).ToListAsync();

            if (resultados == null || resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron citas en la base de datos.");
                return new List<CitaViewModel>();
            }

            return resultados;
        }

        // Obtiene una especialidad por su ID, incluyendo sus citas
        public async Task<CitaViewModel> ObtenerCitaPorIdAsync(int id)
        {
            // Vamos a buscar una especialidad por su ID, incluyendo sus citas
            var citas = await _context.Citas
                .FirstOrDefaultAsync(p => p.IdCita == id);

            // Si no se encuentra el paciente, retornamos null
            if (citas == null)
                throw new NotFoundException($"Cita con id {citas.IdCita} no encontrado.");

            // Si se encuentra, mapeamos el resultado a PacienteViewModel
            return new CitaViewModel
            {
                // Propiedades del paciente
                IdCita = citas.IdCita,
                Fk_IdPaciente = citas.Fk_IdPaciente,
                Fk_IdMedico = citas.Fk_IdMedico,
                Fecha = citas.Fecha,
                Hora = citas.Hora

            };
        }

        // Crea un nuevo cita
        public async Task CrearCitaAsync(CitaViewModel model)
        {
            // Mapeamos el modelo a la entidad Paciente
            _context.Citas.Add(model);

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();
        }

        // Actualiza una cita existente por su ID
        public async Task<CitaViewModel> ActualizarCitaAsync(CitaViewModel model)
        {
            var citaExiste = await _context.Citas.FindAsync(model.IdCita);

            if (citaExiste == null)
                throw new NotFoundException($"Cita con id {model.IdCita} no encontrado.");

            citaExiste.Fk_IdPaciente = model.Fk_IdPaciente;
            citaExiste.Fk_IdMedico = model.Fk_IdMedico;
            citaExiste.Fecha = model.Fecha;
            citaExiste.Hora = model.Hora;

            await _context.SaveChangesAsync();

            // Recuperamos el paciente actualizado con sus relaciones
            var actualizado = await _context.Citas
                .FirstOrDefaultAsync(p => p.IdCita == model.IdCita);

            // Lo devolvemos ya mapeado a CitaViewModel
            return actualizado!;
        }

        // Elimina una cita por su ID
        public async Task EliminarCitaAsync(int id)
        {
            // Recuperamos el paciente por su ID    
            var cita = await _context.Citas.FindAsync(id);

            // Verificamos si el paciente existe
            if (cita == null)
                throw new NotFoundException($"Cita con id {id} no encontrado.");

            // Si existe, lo eliminamos del DbSet
            _context.Citas.Remove(cita);

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();
        }

    }
}
