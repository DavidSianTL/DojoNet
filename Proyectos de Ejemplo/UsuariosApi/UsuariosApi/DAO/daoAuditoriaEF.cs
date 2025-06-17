
using System.Threading.Tasks;
using UsuariosApi.Models;
using Microsoft.EntityFrameworkCore;
using UsuariosApi.Data;

namespace UsuariosApi.DAO
{
    public class daoAuditoriaEF : IAuditoriaDAO
    {
        private readonly AppDbContext _context;

        public daoAuditoriaEF(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertarAuditoriaAsync(Auditoria auditoria)
        {
            // Establecer valores por defecto 
            auditoria.Usuario ??= "Anónimo";
            auditoria.Mensaje ??= "";
            auditoria.Cuerpo ??= null;
            auditoria.TipoAccion ??= null;

            _context.Auditorias.Add(auditoria);
            await _context.SaveChangesAsync();
        }


    }
}
