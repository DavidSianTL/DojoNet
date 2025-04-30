using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppFuturista.Models;

namespace AppFuturista.Services
{
    public class UsuarioService
    {
        private readonly ArchivoJsonService _archivoJsonService;
        private readonly BitacoraService _bitacoraService;
        private const string NombreArchivo = "usuarios.json";
        
        public UsuarioService(ArchivoJsonService archivoJsonService, BitacoraService bitacoraService)
        {
            _archivoJsonService = archivoJsonService;
            _bitacoraService = bitacoraService;
            InicializarDatosAsync().Wait();
        }
        
        private async Task InicializarDatosAsync()
        {
            var usuarios = await _archivoJsonService.LeerAsync<Usuario>(NombreArchivo);
            
            if (usuarios.Count == 0)
            {
                // Agregar usuario administrador predeterminado si no existen usuarios
                usuarios.Add(new Usuario
                {
                    Id = Guid.NewGuid().ToString(),
                    NombreUsuario = "admin",
                    Contraseña = "admin123", // En una app real, esto estaría hasheado
                    NombreCompleto = "Administrador",
                    Rol = "Administrador"
                });
                
                await _archivoJsonService.EscribirAsync(NombreArchivo, usuarios);
                await _bitacoraService.RegistrarEventoAsync("Sistema", "Usuario administrador predeterminado creado");
            }
        }
        
        public async Task<Usuario> AutenticarAsync(string nombreUsuario, string contraseña)
        {
            var usuarios = await _archivoJsonService.LeerAsync<Usuario>(NombreArchivo);
            var usuario = usuarios.FirstOrDefault(u => 
                u.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase) && 
                u.Contraseña == contraseña);
            
            if (usuario != null)
            {
                await _bitacoraService.RegistrarEventoAsync("Autenticación", $"Inicio de sesión exitoso: {usuario.NombreUsuario}");
            }
            else
            {
                await _bitacoraService.RegistrarEventoAsync("Autenticación", $"Intento de inicio de sesión fallido: {nombreUsuario}");
            }
            
            return usuario;
        }
        
        public async Task<Usuario> ObtenerPorIdAsync(string id)
        {
            var usuarios = await _archivoJsonService.LeerAsync<Usuario>(NombreArchivo);
            return usuarios.FirstOrDefault(u => u.Id == id);
        }
    }
}
