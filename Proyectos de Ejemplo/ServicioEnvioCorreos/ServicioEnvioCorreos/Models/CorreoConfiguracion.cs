using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioEnvioCorreos.Models
{
    public class CorreoConfiguracion
    {
        public string Remitente { get; set; }
        public string ClaveApp { get; set; }
        public string ServidorSMTP { get; set; }
        public int PuertoSMTP { get; set; }
        public bool UsarSSL { get; set; }
    }
}
