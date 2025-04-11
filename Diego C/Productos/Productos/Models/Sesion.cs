namespace Productos.Models
{
    public static class Sesion
    {
        // Guarda el usuario actualmente logueado
        public static Dictionary<string, string> UsuariosActivos = new Dictionary<string, string>();
    }
}
