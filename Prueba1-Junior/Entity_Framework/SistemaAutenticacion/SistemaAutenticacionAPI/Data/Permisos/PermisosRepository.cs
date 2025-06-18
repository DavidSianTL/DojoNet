using Microsoft.EntityFrameworkCore;
using SistemaAutenticacionAPI.Data;
using SistemaAutenticacionAPI.Models;

namespace SistemaAutenticacionAPI.Data.Permisos
{
    public interface IPermisosRepository
    {
        Task<Permiso> CreatePermiso(Permiso permisosRegistro);
        Task<bool> EditarPermsos(int id, string nuevoNombre, string descripcion);
        Task<bool> EliminarPermiso(int id);
        Task<List<Permiso>> GetPermisos();
        Task<bool> SaveChanges();
    }

    public class PermisosRepository : IPermisosRepository
    {
        private readonly AppDbContext _context;
        public PermisosRepository(AppDbContext context)
        {
            _context = context;
        }

        //Clase para crear permisos
        public async Task<Permiso> CreatePermiso(Permiso permisosRegistro)
        {
            //verificar si el permiso ya existe con el mismo nombre
            var permisoExiste = await _context.Permisos.Where(x => x.NombrePermiso == permisosRegistro.NombrePermiso).AnyAsync();

            if (permisoExiste)
            {
                throw new Exception("El permiso ya existe");
            }

            var nuevoPermiso = new Permiso
            {
                NombrePermiso = permisosRegistro.NombrePermiso,
                Descripcion = permisosRegistro.Descripcion
            };

            _context.Permisos!.Add(nuevoPermiso);

            return nuevoPermiso;
        }

        public async Task<List<Permiso>> GetPermisos()
        {
            var Permisos = await _context.Permisos!.ToListAsync();

            if (Permisos.Count == 0)
            {
                throw new Exception("No se encontraron datos");
            }

            return Permisos;
        }

        public async Task<bool> EditarPermsos(int id, string nuevoNombre, string descripcion)
        {
            var Permiso = await _context.Permisos.FindAsync(id);

            if (Permiso is null)
            {
                throw new Exception("No se encontro el permiso");
            }

            //Actualizar los datos del permiso
            Permiso.NombrePermiso = nuevoNombre;
            Permiso.Descripcion = descripcion;

            _context.Permisos!.Update(Permiso);

            return true;
        }

        public async Task<bool> EliminarPermiso(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);

            if (permiso is null)
            {
                throw new Exception("No se encontro el permiso");
            }
            _context.Permisos!.Remove(permiso);

            return true;
        }

        //Metodo para manterner en memoria las operaciones que se hagan, y cuando se finalice se procedera a hacer un commit usando EF
        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

    }

}
