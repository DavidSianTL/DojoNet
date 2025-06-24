using API2.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API2.Data.DAOs
{
    public class EspecialidadDAO
    {
        private readonly ClinicaDbContext _context;

        public EspecialidadDAO(ClinicaDbContext context)
        {
            _context = context;
        }

        // Devuelve true si eliminó la especialidad, false si no existía
        public async Task<bool> Eliminar(int id)
        {
            var especialidad = await _context.Especialidades.FindAsync(id);
            if (especialidad != null)
            {
                _context.Especialidades.Remove(especialidad);
                await _context.SaveChangesAsync();
                return true; // Eliminado con éxito
            }
            return false; // No encontrado
        }
    }
}
