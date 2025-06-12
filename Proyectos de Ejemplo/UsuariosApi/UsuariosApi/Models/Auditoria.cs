namespace UsuariosApi.Models
{
    public class Auditoria
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Metodo { get; set; }
        public string Ruta { get; set; }
        public string IpOrigen { get; set; }
        public DateTime Fecha { get; set; }
        public int Estado { get; set; } // 200, 401, etc.
        public string Mensaje { get; set; }
        public string Cuerpo { get; set; }
        public string TipoAccion { get; set; }
    }
}
