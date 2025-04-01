using System.Reflection.Metadata;

namespace Geko_MVC_Project.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string strNombre { get; set; }
        public string strApellido { get; set; }
        public string Username { get; set; }
        public string Puesto { get; set; }  
        public string FechaIngreso { get; set; }
        public string Status{ get; set; }
        public string roll { get; set; }

    }
}
