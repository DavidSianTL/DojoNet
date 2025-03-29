using System;

class Program
{
    static void Main()
    {
        int numero;
        while (true)
        {
            try
            {
                Console.Write("Ingrese un numero de 1 al 10:  ");
                Console.Write("");
                numero = int.Parse(Console.ReadLine());

                if (numero < 1 || numero > 10)
                {
                    throw new ArgumentOutOfRangeException();
                }

                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Ingrese un numero valido por favor.");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("El numero debe ser del 1 al 10.");
            }
        }

        Console.WriteLine($"Numero correcto: {numero}");
    }
}
