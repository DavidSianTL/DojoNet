using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExpress_Entidades
{
	public class Carro
	{
		public int Id { get; set; }
		public string Marca { get; set; }
		public string Modelo { get; set; }
		public int Anio { get; set; }
		public decimal Precio { get; set; }
		public bool Disponible { get; set; }
	}
}
