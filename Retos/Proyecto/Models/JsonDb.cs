using System.Text.Json;
using System.Collections.Generic;
using System.IO;

namespace Proyecto.Models
{
    public static class JsonDb
    {
        private static readonly string rutaUsuarios = Path.Combine("App_Data", "usuarios.json");

        public static List<Usuario> ObtenerUsuarios()
        {
            if (!File.Exists(rutaUsuarios))
                return new List<Usuario>();

            string json = File.ReadAllText(rutaUsuarios);
            return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }
    }
}
