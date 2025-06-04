using Microsoft.EntityFrameworkCore;
using SistemaAutenticacion.Models;

namespace SistemaAutenticacion.Data.PermsosRol
{
    public interface IPermisosRolRepository
    {
        Task AsignarPermisosARol(string rolIdAsignar, int permisoIdAsignar);
        Task EliminarPermisoDeRol(string rolIdAsignado, int permisoIdAsignado);
        Task<List<Permiso>> GetPermisosByRol(string rolId);
    }

    public class PermisosRolRepository : IPermisosRolRepository
    {
        private readonly AppDbContext _context;

        public PermisosRolRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AsignarPermisosARol(string rolIdAsignar, int permisoIdAsignar)
        {
            //Buscar el rol y el permiso en la base de datos
            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Id == rolIdAsignar);
            var permiso = await _context.Permisos.FirstOrDefaultAsync(p => p.Id == permisoIdAsignar);

            //Validar existencia del rol y el permiso
            if (rol is null || permiso is null)
            {
                throw new Exception("El rol o el permiso no existen en la base de datos.");
            }

            //verificar si el permiso ya esta asignado al rol
            bool permisoYaAsignado = await _context.PermisoRol.AnyAsync(rp => rp.RolId == rolIdAsignar && rp.PermisoId == permisoIdAsignar);

            if (permisoYaAsignado)
            {
                throw new Exception("El permiso ya esta asignado a este rol");
            }

            //Crear una nueva instancia de PermisoRol
            var rolPermiso = new PermisoRol
            {
                RolId = rolIdAsignar,
                PermisoId = permisoIdAsignar
            };

            _context.PermisoRol.Add(rolPermiso!);
        }


        public async Task EliminarPermisoDeRol(string rolIdAsignado, int permisoIdAsignado)
        {
            var rolPermiso = await _context.PermisoRol.FirstOrDefaultAsync(rp => rp.RolId == rolIdAsignado && rp.PermisoId == permisoIdAsignado);

            if (rolPermiso is null)
            {
                throw new Exception("No se ha encontrado ningun permiso asociado al rol");
            }

            _context.PermisoRol.Remove(rolPermiso!);

        }

        public async Task<List<Permiso>> GetPermisosByRol(string rolId)
        {
            var permisos = await _context.PermisoRol.Where(rp => rp.RolId == rolId).Select(rp => rp.Permisos).ToListAsync();

            if (!permisos.Any())
            {
                throw new Exception("No se han encontrado permisos asociados a este rol");
            }

            return permisos!;
        }


    }
}
