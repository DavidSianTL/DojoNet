using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace consumoDeApisSOAPConsole
{
    public class Program
    {

        public static void Main(String[] args)
        {
            Banguat.TipoCambio cliente = new Banguat.TipoCambio();
            var resultado = ciente.TipoCambioDia();
            Console.WriteLine(resultado.CambioDolar.First().fecha);
            Console.WriteLine(resultado.CambioDolar.First().referencia);
            Console.ReadKey();
        }

    }
}
