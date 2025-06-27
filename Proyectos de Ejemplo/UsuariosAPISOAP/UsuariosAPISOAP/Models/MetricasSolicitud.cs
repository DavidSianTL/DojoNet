
using System;
namespace UsuariosAPISOAP.Models
{
    public class MetricasSolicitud
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Metodo { get; set; }
        public string Ruta { get; set; }
        public string Usuario { get; set; }
        public long DuracionMilisegundos { get; set; }
        public int CodigoRespuesta { get; set; }
       

    }
}
