using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trabajo_APIRest.Models;
using UsuariosApi.Exceptions;

namespace Trabajo_APIRest.Data
{
    public class daoPacienteWSAsync
    {
        private readonly AppDbContext _context;
        public daoPacienteWSAsync(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene una lista de pacientes
        public async Task<List<PacienteViewModel>> ObtenerPacientesAsync()
        {
            var resultados = await _context.Pacientes
            .Include(p => p.Citas)
            .Select(p => new PacienteViewModel
            {
                // Propiedades del paciente
                IdPaciente = p.IdPaciente,
                Nombre = p.Nombre,
                Email = p.Email,
                Telefono = p.Telefono,
                FechaNacimiento = p.FechaNacimiento,

                // Relación con Citas
                Citas = p.Citas == null ? null : p.Citas.Select(c => new CitaViewModel
                {
                    // Propiedades de la cita
                    IdCita = c.IdCita,
                    Fk_IdPaciente = c.Fk_IdPaciente,
                    Fk_IdMedico = c.Fk_IdMedico,
                    Fecha = c.Fecha,
                    Hora = c.Hora
                }).ToList()

            }).ToListAsync();

            if (resultados == null || resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron pacientes en la base de datos.");
                return new List<PacienteViewModel>();
            }

            return resultados;
        }

        // Obtiene un paciente por su ID, incluyendo sus citas
        public async Task<PacienteViewModel> ObtenerPacientePorIdAsync(int id)
        {
            // Vamos a buscar un paciente por su ID, incluyendo sus citas
            var paciente = await _context.Pacientes
                .Include(p => p.Citas)
                .FirstOrDefaultAsync(p => p.IdPaciente == id);

            // Si no se encuentra el paciente, retornamos null
            if (paciente == null)
                throw new NotFoundException($"Paciente con id {paciente.IdPaciente} no encontrado.");

            // Si se encuentra, mapeamos el resultado a PacienteViewModel
            return new PacienteViewModel
            {
                // Propiedades del paciente
                IdPaciente = paciente.IdPaciente,
                Nombre = paciente.Nombre,
                Email = paciente.Email,
                Telefono = paciente.Telefono,
                FechaNacimiento = paciente.FechaNacimiento,

                // Relación con Citas
                Citas = paciente.Citas?.Select(c => new CitaViewModel
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

        // Crea un nuevo paciente
        public async Task CrearPacienteAsync(PacienteViewModel model)
        {
            // Mapeamos el modelo a la entidad Paciente
            _context.Pacientes.Add(model);

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();
        }

        // Actualiza un paciente existente por su ID
        public async Task<PacienteViewModel> ActualizarPacienteAsync(PacienteViewModel model)
        {
            var pacienteExiste = await _context.Pacientes.FindAsync(model.IdPaciente);

            if (pacienteExiste == null)
                throw new NotFoundException($"Paciente con id {model.IdPaciente} no encontrado.");

            pacienteExiste.Nombre = model.Nombre;
            pacienteExiste.Email = model.Email;
            pacienteExiste.Telefono = model.Telefono;
            pacienteExiste.FechaNacimiento = model.FechaNacimiento;

            await _context.SaveChangesAsync();

            // Recuperamos el paciente actualizado con sus relaciones
            var actualizado = await _context.Pacientes
                .Include(p => p.Citas)
                .FirstOrDefaultAsync(p => p.IdPaciente == model.IdPaciente);

            // Lo devolvemos ya mapeado a PacienteViewModel
            return actualizado!;
        }

        // Elimina un paciente por su ID
        public async Task EliminarPacienteAsync(int id)
        {
            // Recuperamos el paciente por su ID incluyendo sus citas
            var paciente = await _context.Pacientes
                .Include(p => p.Citas)
                .FirstOrDefaultAsync(p => p.IdPaciente == id);

            // Verificamos si el paciente existe
            if (paciente == null)
                throw new NotFoundException($"Paciente con id {id} no encontrado.");

            // Verificamos si el paciente tiene citas asignadas
            if (paciente.Citas != null && paciente.Citas.Any())
            {
                throw new InvalidOperationException(
                    $"No se puede eliminar el paciente con ID {id} porque tiene citas asignadas. " +
                    "Por favor, elimine primero las citas asociadas a este paciente.");
            }

            // Si no tiene citas, procedemos con la eliminación
            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
        }

    }
}
