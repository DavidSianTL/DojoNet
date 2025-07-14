using MiBanco.Models;

namespace MiBanco.Data
{
    public class daoRoles
    {

        // Lista de Roles para simular la base de datos en memoria
        private List<RolesViewModel> roles = new List<RolesViewModel>
        {
            new RolesViewModel{
                Id = 1,
                TipoRol = "Empleado"
            },
            new RolesViewModel
            {
                Id = 2,
                TipoRol = "Cliente"
            }
        };

        // Función (método) que obtiene una lista de los roles
        public List<RolesViewModel> ObtenerRoles()
        {
            return roles;
        }

        // Función (método) que obtiene un rol por su ID
        public RolesViewModel ObtenerRolPorId(int id)
        {
            // Buscamos el rol en la lista por su ID
            return roles.FirstOrDefault(r => r.Id == id);
        }

        // Función (método) que obtiene un rol por su tipo
        public RolesViewModel ObtenerRolPorNombre(string rol)
        {
            // Buscamos el rol en la lista por su tipo
            return roles.FirstOrDefault(r => r.TipoRol == rol);
        }


    }
}
