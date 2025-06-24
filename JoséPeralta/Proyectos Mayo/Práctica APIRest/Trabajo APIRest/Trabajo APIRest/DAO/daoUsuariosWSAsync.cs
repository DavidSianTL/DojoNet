using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Trabajo_APIRest.Data;
using Trabajo_APIRest.Models;
using Microsoft.EntityFrameworkCore;

namespace UsuariosApi.DAO
{
    public class daoUsuariosWSAsync
    {
        public const int ESTADO_ELIMINADO = 4;
        private readonly AppDbContext _context;

        public daoUsuariosWSAsync(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<UsuarioViewModel>> ObtenerUsuariosAsync()
        {
            return await _context.Usuarios
                .Select(u => new UsuarioViewModel
                {
                    IdUsuario = u.IdUsuario,
                    NombreCompleto = u.NombreCompleto,
                    Usuario = u.Usuario,
                    Contrasenia = u.Contrasenia,
                    Token = u.Token
                }).ToListAsync();
        }

        public async Task<UsuarioViewModel> ActualizarToken(int? idUsuario, string token)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null) return null;
            usuario.Token = token;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return new UsuarioViewModel
            {
                IdUsuario = usuario.IdUsuario,
                NombreCompleto = usuario.NombreCompleto,
                Usuario = usuario.Usuario,
                Contrasenia = usuario.Contrasenia,
                Token = usuario.Token
            };
        }

    }
}
