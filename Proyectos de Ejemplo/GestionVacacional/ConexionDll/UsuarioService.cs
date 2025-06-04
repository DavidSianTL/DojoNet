using ConexionDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class UsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public List<Usuario> GetAllUsuarios()
        {
            return _context.Usuarios.ToList();
        }

        public Usuario GetUsuarioById(int id)
        {
            return _context.Usuarios.Find(id);
        }

        public void AddUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }
    }
}
