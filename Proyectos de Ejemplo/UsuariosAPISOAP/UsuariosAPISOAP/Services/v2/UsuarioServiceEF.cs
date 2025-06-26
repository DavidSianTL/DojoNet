using System;
using System.Collections.Generic;
using System.Linq;
using UsuariosAPISOAP.Data;
using UsuariosAPISOAP.Interfaces.v2;
using UsuariosAPISOAP.Models;

namespace UsuariosAPISOAP.Services.v2
{
    public class UsuarioServiceEF : IUsuarioServiceEF
    {
        private readonly AppDbContext _context;

        public UsuarioServiceEF(AppDbContext context)   
        {
            _context = context; 
        }

        public List<UsuarioEF> ObtenerUsuarios()
        {
            return _context.UsuariosEF.ToList();
        }

        public UsuarioEF ObtenerUsuarioPorId(int id)
        {
            return _context.UsuariosEF.FirstOrDefault(u => u.id_usuario == id);
        }

        public bool CrearUsuario(UsuarioEF usuario)
        {
            if (usuario.usuario == null) return false;

            usuario.fecha_creacion = DateTime.Now;
            _context.UsuariosEF.Add(usuario);
            _context.SaveChanges();
            return true;
        }

        public bool ActualizarUsuario(UsuarioEF usuario)
        {
            var existente = _context.UsuariosEF.FirstOrDefault(u => u.id_usuario == usuario.id_usuario);
            if (existente == null) return false;

            existente.usuario = usuario.usuario;
            existente.nom_usuario = usuario.nom_usuario;
            existente.contrasenia = usuario.contrasenia;
            existente.fk_id_estado = usuario.fk_id_estado;
           

            _context.SaveChanges();
            return true;
        }

        public bool EliminarUsuario(int id)
        {
            var usuario = _context.UsuariosEF.FirstOrDefault(u => u.id_usuario == id);
            if (usuario == null) return false;
            usuario.fk_id_estado = 2;
            //_context.UsuariosEF.Remove(usuario);
            _context.SaveChanges();
            return true;
        }

       
    }
}
