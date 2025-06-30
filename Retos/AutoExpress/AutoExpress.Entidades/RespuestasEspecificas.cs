using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutoExpress.Entidades
{
    [Serializable]
    public class RespuestaCarroUnico
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public Carro Carro { get; set; }

        public RespuestaCarroUnico()
        {
            Exitoso = false;
            Mensaje = string.Empty;
        }

        public RespuestaCarroUnico(bool exitoso, string mensaje, Carro carro = null)
        {
            Exitoso = exitoso;
            Mensaje = mensaje;
            Carro = carro;
        }
    }

    [Serializable]
    [XmlRoot("RespuestaListaCarros")]
    public class RespuestaListaCarros
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }

        [XmlArray("Carros")]
        [XmlArrayItem("Carro", typeof(Carro))]
        public Carro[] Carros { get; set; }

        public RespuestaListaCarros()
        {
            Exitoso = false;
            Mensaje = string.Empty;
            Carros = new Carro[0];
        }

        public RespuestaListaCarros(bool exitoso, string mensaje, List<Carro> carros = null)
        {
            Exitoso = exitoso;
            Mensaje = mensaje;
            Carros = carros?.ToArray() ?? new Carro[0];
        }
    }

    [Serializable]
    public class RespuestaSimple
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }

        public RespuestaSimple()
        {
            Exitoso = false;
            Mensaje = string.Empty;
        }

        public RespuestaSimple(bool exitoso, string mensaje)
        {
            Exitoso = exitoso;
            Mensaje = mensaje;
        }
    }
}
