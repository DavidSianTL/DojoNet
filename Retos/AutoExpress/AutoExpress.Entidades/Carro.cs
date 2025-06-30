using System;
using System.Xml.Serialization;

namespace AutoExpress.Entidades
{
    [Serializable]
    [XmlType("Carro")]
    public class Carro
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Año { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }

        public Carro()
        {
            Disponible = true;
        }

        public Carro(string marca, string modelo, int año, decimal precio, bool disponible = true)
        {
            Marca = marca;
            Modelo = modelo;
            Año = año;
            Precio = precio;
            Disponible = disponible;
        }
    }
}
