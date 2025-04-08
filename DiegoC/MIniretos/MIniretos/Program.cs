using System;
using System.IO;

class Program
{
    static void Main()
    {
        int opcion = 0;

        do
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Menú de Minirretos:");
                Console.WriteLine("1. Ingresar un número entre 1 y 10");
                Console.WriteLine("2. Ingresar edad");
                Console.WriteLine("3. Dividir dos números y registrar errores");
                Console.WriteLine("4. Salir");
                Console.Write("Elige una opción: ");
                opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        RetoNumero();
                        break;

                    case 2:
                        RetoEdad();
                        break;

                    case 3:
                        RetoDivision();
                        break;

                    case 4:
                        Console.WriteLine("Saliendo...");
                        break;

                    default:
                        Console.WriteLine("Opción no válida, intenta de nuevo.");
                        break;
                }
            }

            catch (FormatException)
            {
                Console.WriteLine("Error: Por favor ingresa un número válido.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
            Console.ReadKey();

        }
        
        while (opcion != 4);
    }

    static void RetoNumero()
    {
        int numero;

        while (true)
        {
            Console.WriteLine("Por favor, ingresa un número entre 1 y 10:");
            numero = int.Parse(Console.ReadLine());
            try
            {
                if (numero < 1 || numero > 10)
                {
                    throw new ArgumentOutOfRangeException("El número debe estar entre 1 y 10.");
                }
                Console.WriteLine($"Número ingresado correctamente: {numero}");
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Por favor ingresa un número válido.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message} Intenta de nuevo.");
            }
        }
    }

    static void RetoEdad()
    {
        int edad;

        while (true)
        {
            Console.WriteLine("Por favor, ingresa tu edad:");

            edad = int.Parse(Console.ReadLine());
            try
            {
                if (edad <= 0 || edad > 120)
                {
                    Console.WriteLine("ingrese una edad entre 1 y 120 años.");
                    continue;
                }

                Console.WriteLine($"Edad ingresada correctamente: {edad} años");
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Por favor ingresa un número válido.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message} Intenta de nuevo.");

            }
        }
    }

    static void RetoDivision()
    {
        double num1, num2;

        while (true)
        {
            Console.WriteLine("Por favor, ingresa el primer número:");
            string entrada1 = Console.ReadLine();
            bool esNumero1 = double.TryParse(entrada1, out num1);
            if (!esNumero1 )
            {
                LogError($"Error: Usuario ingresó _{entrada1}_, no es un numero.");
                Console.WriteLine("Error: Por favor ingresa un número válido.");
                continue;
            }
            Console.WriteLine("Por favor, ingresa el segundo número:");
            string entrada2 = Console.ReadLine();
            bool esNumero2 = double.TryParse(entrada2, out num2);
            if (!esNumero2)
            {
                LogError($"Error: Usuario ingresó _{entrada2}_, no es un numero.");
                Console.WriteLine("Error: Por favor ingresa un número válido.");
                continue;
            }

            if (!esNumero1 || !esNumero2)
            {
                LogError("Error: Uno o ambos valores ingresados no son números.");
                Console.WriteLine("Error: Por favor ingresa dos números válidos.");
                continue;
            }

            if (num2 == 0)
            {
                LogError("Error: Intento de división por cero.");
                Console.WriteLine($"Error: No se puede dividir {num1} por cero. Intenta de nuevo.");
                continue;
            }

            double resultado = num1 / num2;
            Console.WriteLine($"El resultado de la división {num1}/{num2} es: {resultado}");
            break;
        }
    }

    static void LogError(string mensaje)
    {
        string path = "log.txt";
        using (StreamWriter sw = new StreamWriter(path, true))
        {
            sw.WriteLine($"{DateTime.Now}: {mensaje}");
        }
    }
}
