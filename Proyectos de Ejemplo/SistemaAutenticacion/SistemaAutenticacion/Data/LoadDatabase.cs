using Microsoft.AspNetCore.Identity;
using SistemaAutenticacion.Models;

namespace SistemaAutenticacion.Data
{
    public class LoadDatabase
    {
        public static async Task InsertarData(AppDbContext dbContext, UserManager<Usuarios> userManager)
        {
            //Registro de usuario Default
            if (!userManager.Users.Any())
            {
                var usuario = new Usuarios
                {
                    Nombre = "David",
                    Apellido = "Sian",
                    Email = "usuarioprueba123@gmail.com",
                    UserName = "UsuarioPrueba123",
                    Telefono = "123456789"
                };

                userManager.CreateAsync(usuario, "Password123$").Wait();

                dbContext.SaveChanges();
            }

            if (!dbContext.Permisos.Any())
            {
                dbContext.Permisos!.AddRange(
                    new Permiso //EJ: 1
                    {
                        NombrePermiso = "Crear",
                        Descripcion = "Permite crear registros"
                    },
                    new Permiso //EJ: 2
                    {
                        NombrePermiso = "Editar",
                        Descripcion = "Permite editar registros"
                    },
                    new Permiso //EJ: 3
                    {
                        NombrePermiso = "Eliminar",
                        Descripcion = "Permite eliminar registros"
                    },
                    new Permiso //EJ: 4
                    {
                        NombrePermiso = "Leer",
                        Descripcion = "Permite leer registros"
                    }
                );

                dbContext.SaveChanges();
            }

            if (!dbContext.Roles.Any())
            {
                dbContext.Roles!.AddRange(
                    new CustomRolUsuario
                    { 
                        Name = "Administrador", 
                        Descripcion = "Rol de Administrador",
                    },
                    new CustomRolUsuario
                    {
                        Name = "Usuario",
                        Descripcion = "Rol de Usuario",
                    },
                    new CustomRolUsuario
                    {
                        Name = "Supervisor",
                        Descripcion = "Rol de Supervisor",
                    }
                );

                dbContext.SaveChanges();
            }

            //Obtener roles ingresados y permisos
            var rolAdmin = dbContext.Roles.FirstOrDefault(r => r.Name == "Administrador"); //EJ: 1
            var rolUsuario = dbContext.Roles.FirstOrDefault(r => r.Name == "Usuario"); //EJ: 2
            var rolSupervisor = dbContext.Roles.FirstOrDefault(r => r.Name == "Supervisor"); //EJ: 3

            //Creacion de relaciones entre roles y Permiso
            if (!dbContext.PermisoRol.Any())
            {
                dbContext.PermisoRol!.AddRange(

                    //Asignacion de permisos al rol de Administrador
                    new PermisoRol
                    {
                        RolId = rolAdmin!.Id, //EJ: 1
                        PermisoId = dbContext.Permisos.FirstOrDefault(p => p.NombrePermiso == "Crear")!.Id //EJ: 1
                    },
                    new PermisoRol
                    {
                        RolId = rolAdmin!.Id, //EJ: 1
                        PermisoId = dbContext.Permisos.FirstOrDefault(p => p.NombrePermiso == "Editar")!.Id //EJ: 2
                    },
                    new PermisoRol
                    {
                        RolId = rolAdmin!.Id, //EJ: 1
                        PermisoId = dbContext.Permisos.FirstOrDefault(p => p.NombrePermiso == "Eliminar")!.Id //EJ: 3
                    },
                    new PermisoRol
                    {
                        RolId = rolAdmin!.Id, //EJ: 1
                        PermisoId = dbContext.Permisos.FirstOrDefault(p => p.NombrePermiso == "Leer")!.Id //EJ: 4
                    },

                    //Asignacion de permisos al rol de Usuario
                    new PermisoRol
                    {
                        RolId = rolUsuario!.Id, //EJ: 2
                        PermisoId = dbContext.Permisos.FirstOrDefault(p => p.NombrePermiso == "Leer")!.Id //EJ: 4
                    },

                    //Asignacion de permisos al rol de Supervisor
                    new PermisoRol
                    {
                        RolId = rolSupervisor!.Id, //EJ: 3
                        PermisoId = dbContext.Permisos.FirstOrDefault(p => p.NombrePermiso == "Crear")!.Id //EJ: 1
                    },
                    new PermisoRol
                    {
                        RolId = rolSupervisor!.Id, //EJ: 3
                        PermisoId = dbContext.Permisos.FirstOrDefault(p => p.NombrePermiso == "Editar")!.Id //EJ: 2
                    },
                    new PermisoRol
                    {
                        RolId = rolSupervisor!.Id, //EJ: 3
                        PermisoId = dbContext.Permisos.FirstOrDefault(p => p.NombrePermiso == "Leer")!.Id //EJ: 4
                    }

                );

                dbContext.SaveChanges();
            }



            await dbContext.SaveChangesAsync();
        }



    }
}
