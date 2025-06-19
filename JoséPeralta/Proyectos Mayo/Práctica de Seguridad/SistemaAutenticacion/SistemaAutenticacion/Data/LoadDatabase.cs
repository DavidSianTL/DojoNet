using Microsoft.AspNetCore.Identity;
using SistemaAutenticacion.Models;

namespace SistemaAutenticacion.Data
{
    public class LoadDatabase
    {

        public static async Task InsertarDa(AppDbContext _AppDbContext, UserManager<UsuarioViewModel> userManager)
        {

            // Registro de Usuarios en caso de que no existan
            if (!userManager.Users.Any())
            {
                // Si no existe el usuario, lo creamos
                var usuario = new UsuarioViewModel
                {
                    Nombres = "José",
                    Apellidos = "Peralta",
                    Email = "holamundo@gmail.com",
                    UserName = "Peghoste",
                    Telefono = "12345678",
                    FechaCreacion = DateTime.Now
                };

                // Creamos el usuario
                userManager.CreateAsync(usuario, "holamundo").Wait();

                // Guardar los cambios en la base de datos
                _AppDbContext.SaveChanges();

            }

            // Registro de Roles en caso de que no existan
            if (!_AppDbContext.Permisos.Any())
            {
                // Si no existe el permiso, lo creamos
                _AppDbContext.Permisos!.AddRange(new List<PermisosViewModel>
                {
                    new PermisosViewModel
                    {
                        NombrePermiso = "Crear",
                        Descripcion = "Permite crear registros"
                    },
                    new PermisosViewModel
                    {
                        NombrePermiso = "Editar",
                        Descripcion = "Permite actualizar registros"
                    },
                    new PermisosViewModel
                    {
                        NombrePermiso = "Eliminar",
                        Descripcion = "Permite eliminar registros"
                    },
                    new PermisosViewModel
                    {
                        NombrePermiso = "Leer",
                        Descripcion = "Permite la lectura registros"
                    }
                });

                _AppDbContext.SaveChanges();

            }

            // Registro de Roles en caso de que no existan
            if (!_AppDbContext.Roles.Any())
            {
                // Si no existe el rol, lo creamos
                _AppDbContext.Roles!.AddRange(new List<CustomRolUsuarioViewModel>
                {
                    new CustomRolUsuarioViewModel
                    {
                        Name = "Administrador",
                        Descripcion = "Rol de administrador"
                    },
                    new CustomRolUsuarioViewModel
                    {
                        Name = "Usuario",
                        Descripcion = "Rol de usuario"
                    },
                    new CustomRolUsuarioViewModel
                    {
                        Name = "Cliente",
                        Descripcion = "Rol de cliente"
                    }
                });

                _AppDbContext.SaveChanges();

            }

            // Obtener roles ingresados y permisos
            var rolAdmin = _AppDbContext.Roles.FirstOrDefault(r => r.Name == "Administrador");
            var rolUsuario = _AppDbContext.Roles.FirstOrDefault(r => r.Name == "Usuario");
            var rolCliente = _AppDbContext.Roles.FirstOrDefault(r => r.Name == "Cliente");

            if (!_AppDbContext.PermisosRol.Any())
            {
                _AppDbContext.PermisosRol!.AddRange(new List<PermisosRolViewModel>
                {
                    new PermisosRolViewModel
                    {
                        // El "!" es un operador de supresión de nullabilidad, que indica que el valor no es nulo
                        // Así nos se evita el error de referencia nula
                        RolId = rolAdmin!.Id,
                        // Obtenemos el id del permiso por su nombre
                        PermisoId = _AppDbContext.Permisos.FirstOrDefault(p => p.NombrePermiso == "Crear")!.Id
                    },
                    new PermisosRolViewModel
                    {
                        RolId = rolAdmin!.Id,
                        PermisoId = _AppDbContext.Permisos.FirstOrDefault(p => p.NombrePermiso == "Editar")!.Id
                    },
                    new PermisosRolViewModel
                    {
                        RolId = rolAdmin!.Id,
                        PermisoId = _AppDbContext.Permisos.FirstOrDefault(p => p.NombrePermiso == "Eliminar")!.Id
                    },
                    new PermisosRolViewModel
                    {
                        RolId = rolAdmin!.Id,
                        PermisoId = _AppDbContext.Permisos.FirstOrDefault(p => p.NombrePermiso == "Leer")!.Id
                    }


                });

                // Guardar los cambios en la base de datos
                _AppDbContext.SaveChanges();

            }


            await _AppDbContext.SaveChangesAsync();

        }



    }
}
