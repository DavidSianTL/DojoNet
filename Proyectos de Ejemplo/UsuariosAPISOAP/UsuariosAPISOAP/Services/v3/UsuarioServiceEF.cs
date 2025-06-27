using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using UsuariosAPISOAP.Data;
using UsuariosAPISOAP.Interfaces.v3;
using UsuariosAPISOAP.Models;

namespace UsuariosAPISOAP.Services.v3
{
    public class UsuarioServiceEF : IUsuarioServiceEF
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsuarioServiceEF> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioServiceEF(AppDbContext context, ILogger<UsuarioServiceEF> logger, IHttpContextAccessor httpContextAccessor)
          
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<UsuarioDTO> ObtenerUsuarios()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los usuarios.");
                var usuarios = _context.UsuariosEF
                    .Where(u => u.fk_id_estado == EstadoUsuario.Activo)
                    .ToList();
                
                return usuarios.Select(UsuarioMapper.ToDTO).ToList();
             
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: al consultar los usuarios" + ex.Message);
                return null;
            }

        }

        public UsuarioDTO ObtenerUsuarioPorId(int id)
        {

            try 
            {
                _logger.LogInformation($"El usuario  con {id} fue consultado.");
                var usuario = _context.UsuariosEF.FirstOrDefault(u => u.id_usuario == id);

                if (usuario == null)
                {
                    _logger.LogWarning($"No se encontró ningún usuario con ID {id}.");
                    return null;
                }

                return UsuarioMapper.ToDTO(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: al consultar el usuario con id = {id}" + ex.Message);
                return null;
            }

         }

        public bool CrearUsuario(UsuarioEF usuario)
        {
            //var usuarioAutenticado = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
            //if (!usuarioAutenticado)
            //{
            //    _logger.LogWarning("Intento de crear usuario sin autenticación.");
            //    return false;

            //}

            if (usuario.usuario == null) return false;
            
            
            if (_context.UsuariosEF.Any(u => u.usuario == usuario.usuario))
            {

                _logger.LogWarning($"El usuario  con {usuario.nom_usuario} ya existe, no puede duplicarse.");
                return false; // Ya existe un usuario con ese nombre
            }
            usuario.contrasenia = BCrypt.Net.BCrypt.HashPassword(usuario.contrasenia);
            usuario.fecha_creacion = DateTime.Now;
            _context.UsuariosEF.Add(usuario);
            try
            {
                _context.SaveChanges();
                _logger.LogInformation($"El usuario  con {usuario.nom_usuario} fue creado.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: al guardar el nuevo usuario" + ex.Message);
                return false;
            }
           
        }

        public bool ActualizarUsuario(UsuarioEF usuario)
        {
            var existente = _context.UsuariosEF.FirstOrDefault(u => u.id_usuario == usuario.id_usuario);
            if (existente == null)
            {
                _logger.LogWarning($"El usuario  con {usuario.nom_usuario} no existe.");
                return false;
            }
                          

            existente.usuario = usuario.usuario;
            existente.nom_usuario = usuario.nom_usuario;
            existente.contrasenia = BCrypt.Net.BCrypt.HashPassword(usuario.contrasenia);//usuario.contrasenia;
            existente.fk_id_estado = usuario.fk_id_estado;
            try
            {
                _context.SaveChanges();
                _logger.LogInformation($"El usuario  con {usuario.id_usuario} fue modificado.");
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error: al actualizar el usuario" + ex.Message);
                return false;
            }

           
        }

        public bool EliminarUsuario(int id)
        {
            var usuario = _context.UsuariosEF.FirstOrDefault(u => u.id_usuario == id);
            if (usuario == null)
            {
                _logger.LogWarning($"El usuario  con {usuario.nom_usuario} no existe.");
                return false;
            }
                
                
            usuario.fk_id_estado = EstadoUsuario.Eliminado;
            //_context.UsuariosEF.Remove(usuario);
            try 
            {
                _context.SaveChanges();
                _logger.LogInformation($"El usuario  con {id} fue eliminado.");
                return true;
            
            } catch (Exception ex)
            {
                _logger.LogError("Error: al eliminar el usuario" + ex.Message);
                return false;
            }
           
        }

       
    }
}
