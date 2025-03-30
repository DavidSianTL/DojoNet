using System;
//Crea un programa que pida un número entre 1 y 10.Si el usuario ingresa algo 
// incorrecto (un número fuera de rango o letras),
//  debe mostrar un mensaje de error y pedir el número otra vez. 
class Program
{
    static void Main()
    {
        int numero;
        bool esValido;

        do
        {
            Console.Write("Por favor, ingresa un número entre 1 y 10: ");
            string entrada = Console.ReadLine();

            // Validar si la entrada es un número entero y está en el rango deseado
            esValido = int.TryParse(entrada, out numero) && numero >= 1 && numero <= 10;

            if (!esValido)
            {
                Console.WriteLine("Error: Debes ingresar un número válido entre 1 y 10.");
            }

        } while (!esValido);

        Console.WriteLine($"Número ingresado correctamente: {numero}");
    }
}


//Crea un programa que pida la edad del usuario.
// Si ingresa un número menor a 0 o mayor a 120, muestra un 
// mensaje de error y vuelve a pedir la edad hasta que ingrese 
// un valor válido.

using System;

class Program
{
    static void Main()
    {
        int edad;
        bool esValido;

        do
        {
            Console.Write("Por favor, ingresa tu edad: ");
            string entrada = Console.ReadLine();

            // Validar si la entrada es un número entero y está en el rango deseado
            esValido = int.TryParse(entrada, out edad) && edad >= 0 && edad <= 120;

            if (!esValido)
            {
                Console.WriteLine("Error: Debes ingresar una edad válida entre 0 y 120.");
            }

        } while (!esValido);

        Console.WriteLine($"Edad ingresada correctamente: {edad}");
    }
}

//Crea un programa que divida dos números y registre los errores en un log.
// Debe manejar estos errores:
//Si el usuario ingresa texto en lugar de un número.
//Si el usuario intenta dividir por 0.


using System;
using System.IO;

class Program
{
    static void Main()
    {
        double numero1, numero2;
        bool esValido1, esValido2;

        while (true)
        {
            try
            {
                // Solicitar el primer número
                Console.Write("Ingresa el primer número: ");
                string entrada1 = Console.ReadLine();
                esValido1 = double.TryParse(entrada1, out numero1);

                // Verificar si la entrada es válida
                if (!esValido1)
                {
                    throw new FormatException("El primer número ingresado no es válido.");
                }

                // Solicitar el segundo número
                Console.Write("Ingresa el segundo número: ");
                string entrada2 = Console.ReadLine();
                esValido2 = double.TryParse(entrada2, out numero2);

                // Verificar si la entrada es válida
                if (!esValido2)
                {
                    throw new FormatException("El segundo número ingresado no es válido.");
                }

                // Verificar división por cero
                if (numero2 == 0)
                {
                    throw new DivideByZeroException("No se puede dividir por cero.");
                }

                // Realizar la división y mostrar el resultado
                double resultado = numero1 / numero2;
                Console.WriteLine($"Resultado: {resultado}");
                break; // Salir del bucle si todo fue exitoso
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                RegistrarError(ex.Message);
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                RegistrarError(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                RegistrarError(ex.Message);
            }
        }
    }

    static void RegistrarError(string mensaje)
    {
        try
        {
            // Registrar el error en un archivo de log
            File.AppendAllText("log_errores.txt", $"{DateTime.Now}: {mensaje}{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al escribir en el log: {ex.Message}");
        }
    }
}
