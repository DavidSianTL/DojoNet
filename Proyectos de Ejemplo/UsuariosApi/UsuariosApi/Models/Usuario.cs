namespace UsuariosApi.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string UsuarioLg { get; set; }

        public string Nom_Completo { get; set; }
        public string Contrasenia { get; set; }

        public int Fk_id_estado { get; set; }
        public DateTime Fecha_creacion { get; set; }
        public string Descripcion { get; set; }


    }
}
