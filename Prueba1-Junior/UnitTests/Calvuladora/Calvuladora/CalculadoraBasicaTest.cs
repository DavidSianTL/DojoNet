using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Calvuladora
{
    internal class CalculadoraBasicaTest
    {
        class SubLogger : ILogger
        {
            public void Log(string message)
            {
                Console.WriteLine("Log: " + message);
            }
        }

        public class CalculadoraTestt
        {
            private readonly ICalculadora _calculadora;
            private readonly ILogger _logger;
            public CalculadoraTestt()
            {
                _logger = new SubLogger();
                _calculadora = new calculadoraBasica(_logger);
            }



            
            [FactAttribute(DisplayName = "La resta debe devolver la cantidad correcta")]
            public void TestSumar()
            {
                double result = _calculadora.Sumar(2, 3);
                Assert.Equal(5, result);
                Console.WriteLine($"Resultado de Sumar: {result}");
            }
            //[FactAttribute(DisplayName = "No divide por cero")]
            [Fact]
            public void TestDividirPorCero()
            {
                Assert.Throws<DivideByZeroException>(() => _calculadora.Dividir(10, 0));
                Console.WriteLine("Excepción de división por cero capturada correctamente.");
            }

            public void TestRestar()
            {
                double result = _calculadora.Restar(5, 2);
                Console.WriteLine($"Resultado de Restar: {result}");
            }
            public void TestMultiplicar()
            {
                double result = _calculadora.Multiplicar(4, 3);
                Console.WriteLine($"Resultado de Multiplicar: {result}");
            }
            public void TestDividir()
            {
                double result = _calculadora.Dividir(10, 2);
                Console.WriteLine($"Resultado de Dividir: {result}");
            }
        }
    }
}
