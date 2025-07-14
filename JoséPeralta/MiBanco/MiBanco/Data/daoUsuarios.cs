using MiBanco.Models;

namespace MiBanco.Data
{
    public class daoUsuarios
    {

        // Lista de Usuarios para simular la base de datos en memoria
        private List<UsuariosViewModel> usuarios = new List<UsuariosViewModel>()
        {
            new UsuariosViewModel
            {
                Id = 1,
                Usuario = "Peghoste",
                Contraseña = "admin123",
                RolId = 1, // Asignamos el rol de empleado por defecto
                PersonaId = 1 // Asignar un empleado por defecto
            }
        };

        // Función (método) que obtiene una lista de los usuarios
        public List<UsuariosViewModel> ObtenerUsuarios()
        {
            return usuarios;
        }

        // Función (método) que verifica las credenciales de un usuario
        public UsuariosViewModel VerificarCredenciales(string usuario, string contraseña)
        {
            // Buscamos el usuario en la lista por su nombre de usuario y contraseña
            return usuarios.FirstOrDefault(u => u.Usuario == usuario && u.Contraseña == contraseña);
        }


    }
}
