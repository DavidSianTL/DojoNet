using API2.Models;
using API2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API2.Data.DAOs
{
    public class MedicoDAO
    {
        private readonly ClinicaDbContext _context;

        public MedicoDAO(ClinicaDbContext context)
        {
            _context = context;
        }

        public async Task Crear(Medico medico)
        {
            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();
        }
    }
}
