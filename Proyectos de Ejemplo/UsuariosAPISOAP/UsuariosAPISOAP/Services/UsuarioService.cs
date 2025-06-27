using System;
using System.Collections.Generic;
using System.Linq;
using UsuariosAPISOAP.Interfaces;
using UsuariosAPISOAP.Models;

namespace UsuariosAPISOAP.Services
{
    public class UsuarioService : IUsuarioService
    {
        private static List<Usuario> usuarios = new List<Usuario>();

        public List<Usuario> ObtenerUsuarios()
        {
            return usuarios;
        }

        public Usuario ObtenerUsuarioPorId(int id)
        {
            return usuarios.FirstOrDefault(u => u.IdUsuario == id);
        }

        public bool CrearUsuario(Usuario usuario)
        {
            if (usuario == null) return false;

            usuario.IdUsuario = usuarios.Count > 0 ? usuarios.Max(u => u.IdUsuario) + 1 : 1;
            usuario.Fecha_creacion = DateTime.Now;
            usuarios.Add(usuario);
            return true;
        }

        public bool ActualizarUsuario(Usuario usuario)
        {
            var existente = usuarios.FirstOrDefault(u => u.IdUsuario == usuario.IdUsuario);
            if (existente == null) return false;

            existente.UsuarioLg = usuario.UsuarioLg;
            existente.Nom_Completo = usuario.Nom_Completo;
            existente.Contrasenia = usuario.Contrasenia;
            existente.Fk_id_estado = usuario.Fk_id_estado;
            existente.Descripcion = usuario.Descripcion;
            // No actualizamos Fecha_creacion

            return true;
        }

        public bool EliminarUsuario(int id)
        {
            var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null) return false;

            usuarios.Remove(usuario);
            return true;
        }
    }
}
