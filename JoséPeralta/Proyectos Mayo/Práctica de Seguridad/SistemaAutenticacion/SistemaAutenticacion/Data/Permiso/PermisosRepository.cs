using Microsoft.EntityFrameworkCore;
using SistemaAutenticacion.Models;

namespace SistemaAutenticacion.Data.Permiso
{

    // Interfaz para el repositorio de permisos
    public interface IPermisosRepository
    {

    }

    // Clase que implementa la interfaz IPermisosRepository
    public class PermisosRepository : IPermisosRepository
    {

        // Inyección de dependencias para el contexto de la base de datos
        private readonly AppDbContext _appDbContext;

        // Constructor de la clase PermisosRepository que recibe el contexto de la base de datos
        public PermisosRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // Método(función) para obtener todos los permisos
        public async Task<PermisosViewModel> InsertarPermiso(PermisosViewModel permiso)
        {
            // Verificar si el permiso ya existes
            var permisoExiste = await _appDbContext.Permisos.Where(x => x.NombrePermiso == permiso.NombrePermiso).AnyAsync();

            // Verificamos que el permiso no exista
            if (permisoExiste)
            {
                throw new Exception("El permiso ya existe");
            }

            // Agregar el permiso a la base de datos
            var nuevoPermiso = new PermisosViewModel
            {
                NombrePermiso = permiso.NombrePermiso,
                Descripcion = permiso.Descripcion
            };

            // Agregar el permiso al contexto
            _appDbContext.Permisos!.Add(nuevoPermiso);

            // Retornar el permiso agregado
            return nuevoPermiso;

        }

        // Método(función) para obtener todos los permisos
        public async Task<List<PermisosViewModel>> ObtenerPermisos()
        {

            // Obtenemos la lista de permisos desde la base de datos
            var permisos = await _appDbContext.Permisos!.ToListAsync();

            // Verificamos que existan permisos
            if (permisos.Count == 0)
            {
                throw new Exception("No existen permisos");
            }

            // Verificamos si la lista de permisos es nula
            if (permisos is null)
            {
                throw new Exception("No existen permisos");
            }

            // Retornamos la lista de permisos
            return permisos;

        }



    }
}
