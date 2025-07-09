using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Calvuladora
{

    public interface ICalculadora
    {
        double Sumar(double a, double b);
        double Restar(double a, double b);
        double Multiplicar(double a, double b);
        double Dividir(double a, double b);
    }

    public interface ILogger
    {
        void Log(string message);
    }


    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"Log: {message}");
        }
    }

    public class calculadoraBasica : ICalculadora
    {
        private readonly ILogger _logger;
        public calculadoraBasica(ILogger logger)
        {
            _logger = logger;
        }

        public double Sumar(double a, double b)
        {
            double result = a + b;
            _logger.Log($"Sumar: {a} + {b} = {result}");
            return result;
        }
        public double Restar(double a, double b)
        {
            double result = a - b;
            _logger.Log($"Restar: {a} - {b} = {result}");
            return result;
        }
        public double Multiplicar(double a, double b)
        {
            double result = a * b;
            _logger.Log($"Multiplicar: {a} * {b} = {result}");
            return result;
        }
        public double Dividir(double a, double b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("No se puede dividir por cero.");
            }
            double result = a / b;
            _logger.Log($"Dividir: {a} / {b} = {result}");
            return result;
        }
    
    
        
    }
}
