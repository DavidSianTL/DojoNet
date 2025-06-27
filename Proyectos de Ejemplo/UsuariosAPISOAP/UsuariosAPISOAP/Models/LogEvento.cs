namespace UsuariosAPISOAP.Models
{
    public class LogEvento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Metodo { get; set; }
        public string Ruta { get; set; }
        public string Usuario { get; set; } // opcional
        public string Mensaje { get; set; }

    }
}
