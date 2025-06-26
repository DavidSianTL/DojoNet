using System;
using System.Collections.Generic;
using System.Linq;
using UsuariosAPISOAP.Interfaces.v1;
using UsuariosAPISOAP.Models;

namespace UsuariosAPISOAP.Services.v1
{
    public class UsuarioServiceM : IUsuarioServiceM
    {
        private static List<Usuario> usuariosM = new List<Usuario>();

        public List<Usuario> ObtenerUsuarios()
        {
            return usuariosM;
        }

        public Usuario ObtenerUsuarioPorId(int id)
        {
            return usuariosM.FirstOrDefault(u => u.IdUsuario == id);
        }

        public bool CrearUsuario(Usuario usuario)
        {
            if (usuario == null) return false;

            usuario.IdUsuario = usuariosM.Count > 0 ? usuariosM.Max(u => u.IdUsuario) + 1 : 1;
            usuario.Fecha_creacion = DateTime.Now;
            usuariosM.Add(usuario);
            return true;
        }

        public bool ActualizarUsuario(Usuario usuario)
        {
            var existente = usuariosM.FirstOrDefault(u => u.IdUsuario == usuario.IdUsuario);
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
            var usuario = usuariosM.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null) return false;

            usuariosM.Remove(usuario);
            return true;
        }
    }
}
