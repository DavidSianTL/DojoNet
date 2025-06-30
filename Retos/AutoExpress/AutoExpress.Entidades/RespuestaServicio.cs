using System;
using System.Xml.Serialization;

namespace AutoExpress.Entidades
{
    [Serializable]
    [XmlInclude(typeof(Carro))]
    [XmlInclude(typeof(Carro[]))]
    public class RespuestaServicio
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public object Datos { get; set; }

        public RespuestaServicio()
        {
            Exitoso = false;
            Mensaje = string.Empty;
        }

        public RespuestaServicio(bool exitoso, string mensaje, object datos = null)
        {
            Exitoso = exitoso;
            Mensaje = mensaje;
            Datos = datos;
        }
    }
}
