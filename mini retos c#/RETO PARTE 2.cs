// RETO 1 VALIDAR UN NUMERO
using System;

class Program
{
    static void Main()
    {
        int numero;
        bool esValido = false;
        
        do
        {
            Console.Write("Ingrese un número entre 1 y 10: ");
            string entrada = Console.ReadLine();
            
            if (int.TryParse(entrada, out numero) && numero >= 1 && numero <= 10)
            {
                esValido = true;
                Console.WriteLine($"Número válido ingresado: {numero}");
            }
            else
            {
                Console.WriteLine("Entrada no válida. Por favor, ingrese un número entre 1 y 10.");
            }
        } while (!esValido);
    }
}
// RETO 2 VALIDAD EDAD
using System;

class Program
{
    static void Main()
    {
        int edad;
        bool esValida = false;
        
        do
        {
            Console.Write("Ingrese su edad: ");
            string entrada = Console.ReadLine();
            
            if (int.TryParse(entrada, out edad) && edad >= 0 && edad <= 120)
            {
                esValida = true;
                Console.WriteLine($"Edad válida ingresada: {edad}");
            }
            else
            {
                Console.WriteLine("Entrada no válida. Por favor, ingrese una edad entre 0 y 120.");
            }
        } while (!esValida);
    }
}
// RETO 3  DIVISION DE NUMEROS Y MARCA DE ERRORES
using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            Console.Write("Ingrese el primer número: ");
            double num1 = Convert.ToDouble(Console.ReadLine());
            
            Console.Write("Ingrese el segundo número: ");
            double num2 = Convert.ToDouble(Console.ReadLine());
            
            if (num2 == 0)
            {
                throw new DivideByZeroException("No se puede dividir por cero.");
            }
            
            double resultado = num1 / num2;
            Console.WriteLine($"Resultado: {resultado}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Error: Entrada no válida. Por favor, ingrese solo números.");
            LogError(ex);
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine("Error: No se puede dividir por cero.");
            LogError(ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocurrió un error inesperado.");
            LogError(ex);
        }
    }

    static void LogError(Exception ex)
    {
        string rutaLog = "error_log.txt";
        File.AppendAllText(rutaLog, $"{DateTime.Now}: {ex.Message}\n");
    }
}


