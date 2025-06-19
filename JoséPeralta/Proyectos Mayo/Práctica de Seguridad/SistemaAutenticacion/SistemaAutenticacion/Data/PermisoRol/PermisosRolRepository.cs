using Microsoft.EntityFrameworkCore;
using SistemaAutenticacion.Models;

namespace SistemaAutenticacion.Data.PermisoRol
{

    public interface IPermisosRolRepository
    {
        // Podemos crear de una vez el método para asignar permisos a un rol
        Task AsignarPermisosRol(string IdRol, int IdPermiso);

        Task EliminarPermisosRol(string IdRol, int IdPermiso);
    }



    public class PermisosRolRepository : IPermisosRolRepository
    {

        // Inyección de dependencias para el contexto de la base de datos
        private readonly AppDbContext _appDbContext;

        // Constructor de la clase PermisosRepository que recibe el contexto de la base de datos
        public PermisosRolRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        // Entonces, ahora, podemos implementar la interfaz IPermisosRolRepository
        // y así es más rápido y fácil de implementar
        public async Task AsignarPermisosRol(string IdRol, int IdPermiso)
        {
            // Buscamos el Rol y el Permiso en la base de datos
            // Usamos FirstOrDefaultAsync para obtener el primer elemento que cumpla con la condición especificada
            var rol = _appDbContext.Roles.FirstOrDefaultAsync(r => r.Id == IdRol);
            var permiso = _appDbContext.Permisos.FirstOrDefaultAsync(p => p.Id == IdPermiso);

            // Validamos por separado que el rol y el permiso existen
            if (rol is null)
            {
                throw new Exception("El rol no existe");
            }

            if (permiso is null)
            {
                throw new Exception("El permiso no existe");
            }

            // Verificamos primero que el permiso no esté ya asignado al rol
            // Usamos AnyAsync para verificar si ya existe una relación entre el rol y el permiso
            // El AnySync se usa para verificar si existe al menos un elemento que cumpla con la condición especificada
            bool permisoRolAsignado = await _appDbContext.PermisosRol.AnyAsync(pr => pr.RolId == IdRol && pr.PermisoId == IdPermiso);

            if (permisoRolAsignado)
            {
                throw new Exception("El permiso ya está asignado al rol");
            }

            // Si el permiso no está asignado, lo agregamos a la tabla PermisosRol
            var nuevoPermisoRol = new PermisosRolViewModel
            {
                RolId = IdRol,
                PermisoId = IdPermiso
            };

            // Agregamos el nuevo permiso rol al contexto
            // Usamos el "!" para indicar que PermisosRol no es nulo, ya que lo hemos definido en el AppDbContext
            _appDbContext.PermisosRol!.Add(nuevoPermisoRol);


        }

        public async Task EliminarPermisosRol(string IdRol, int IdPermiso)
        {
            // Verificamos si el permiso está asignado al rol
            var existePermisoRol = await _appDbContext.PermisosRol.FirstOrDefaultAsync(pr => pr.RolId == IdRol && pr.PermisoId == IdPermiso);

            // Si el permiso no está asignado al rol, lanzamos una excepción
            if (existePermisoRol is null)
            {
                throw new Exception("No se ha encontrado ningún permiso asociado al rol");
            }

            // Si el permiso está asignado al rol, lo eliminamos de la tabla PermisosRol
            _appDbContext.PermisosRol!.Remove(existePermisoRol);
        }

        
        public async Task<List<PermisosViewModel>> ObtenerPermisosPorRol(string IdRol)
        {
            // Obtenemos los permisos asignados al rol
            var permisosRol = await _appDbContext.PermisosRol.Where(rp => rp.RolId == IdRol).Select(rp => rp.Permiso).ToListAsync();


            // Verificamos que existan permisos asignados al rol
            if (!permisosRol.Any())
            {
                throw new Exception("No se han encontrado permisos asignados al rol");
            }

            // Retornamos la lista de permisos asignados al rol
            return permisosRol!;

        }


    }
}
