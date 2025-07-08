using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioEnvioCorreos.Models
{
    public class Correo
    {
        public int Id { get; set; }
        public string Destinatario { get; set; }
        public string CC { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public string RutaAdjunto { get; set; }
    }
}
