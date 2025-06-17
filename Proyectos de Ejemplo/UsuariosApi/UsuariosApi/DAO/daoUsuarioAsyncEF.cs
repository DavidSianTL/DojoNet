using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsuariosApi.Models;
using UsuariosApi.Exceptions;
using UsuariosApi.Data;



namespace UsuariosApi.DAO
{
    public class daoUsuarioAsyncEF
    {
        private readonly AppDbContext _context;
        public const int ESTADO_ELIMINADO = 4;

        public daoUsuarioAsyncEF(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioEF> ObtenerUsuarioPorNombreAsync(string usuarioNombre)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.UsuarioLg == usuarioNombre);

            if (usuario == null)
                throw new NotFoundException($"Usuario '{usuarioNombre}' no encontrado.");

            return usuario;
        }

        public async Task<List<UsuarioEF>> ObtenerUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task InsertarUsuarioAsync(UsuarioEF usuario)
        {
            var contraseniaHash = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenia);
            usuario.Contrasenia = contraseniaHash;
            usuario.Fecha_creacion = DateTime.Now;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> ValidarContraseniaCambioAsync(string nuevaContrasenia, int idUsuario)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);

            if (usuario == null)
                throw new NotFoundException($"Usuario con id {idUsuario} no encontrado.");

            return !BCrypt.Net.BCrypt.Verify(nuevaContrasenia, usuario.Contrasenia);
        }

        public async Task ActualizarUsuarioAsync(UsuarioEF usuario)
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(usuario.IdUsuario);

            if (usuarioExistente == null)
                throw new NotFoundException($"Usuario con id {usuario.IdUsuario} no encontrado.");

            usuarioExistente.UsuarioLg = usuario.UsuarioLg;
            usuarioExistente.Nom_Completo = usuario.Nom_Completo;
            usuarioExistente.Fk_id_estado = usuario.Fk_id_estado;

            if (!string.IsNullOrWhiteSpace(usuario.Contrasenia))
            {
                var cambio = await ValidarContraseniaCambioAsync(usuario.Contrasenia, usuario.IdUsuario);
                if (cambio)
                {
                    usuarioExistente.Contrasenia = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenia);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task EliminarUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                throw new NotFoundException($"Usuario con id {id} no encontrado.");

            usuario.Fk_id_estado = ESTADO_ELIMINADO;

            await _context.SaveChangesAsync();
        }


    }
}
