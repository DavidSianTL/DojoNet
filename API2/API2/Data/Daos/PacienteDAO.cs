using API2.Models;
using API2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API2.Data.DAOs
{
    public class PacienteDAO
    {
        private readonly ClinicaDbContext _context;

        public PacienteDAO(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task<Object> ObtenerTodos()
        {
            var pacientes = await _context.Pacientes
            .Include(p => p.Citas)
            .Select(p => new
            {
                p.Id,
                p.Nombre,
                p.Email,
                p.Telefono,
                p.FechaNacimiento,
          
                Citas = p.Citas.Select(c => c.Id).ToList()
            })
            .ToListAsync();

            return pacientes;
        }

        public async Task<Object> ObtenerPorId(int id)
        {
            var paciente = await _context.Pacientes
        .Include(p => p.Citas)
        .Where(p => p.Id == id) 
        .Select(p => new
        {
            p.Id,
            p.Nombre,
            p.Email,
            p.Telefono,
            p.FechaNacimiento,
            Citas = p.Citas.Select(c => c.Id).ToList()
        })
        .FirstOrDefaultAsync(); 

            return paciente;
        }

        public async Task Crear(Paciente paciente)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Paciente paciente)
        {
            _context.Entry(paciente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
