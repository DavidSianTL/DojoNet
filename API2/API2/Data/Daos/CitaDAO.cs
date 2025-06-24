using API2.Data;
using API2.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API2.Data.DAOs
{
    public class CitaDAO
    {
        private readonly ClinicaDbContext _context;

        public CitaDAO(ClinicaDbContext context)
        {
            _context = context;
        }

        // Nuevo método Actualizar que retorna bool
        public async Task<bool> Actualizar(Cita cita)
        {
            var citaExistente = await _context.Citas.FindAsync(cita.Id);
            if (citaExistente == null)
            {
                return false; // No existe la cita
            }

            // Actualizamos solo las propiedades necesarias
            citaExistente.Fecha = cita.Fecha;
            // Agrega aquí otras propiedades si es necesario

            await _context.SaveChangesAsync();
            return true; // Actualización exitosa
        }
    }
}

