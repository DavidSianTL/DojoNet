using System;
class Program
{
    static void Main()
    {
        int edad;
        while (true)
        {
            try
            {
                Console.Write("Ingrese su edad (entre 0 y 120): ");
                edad = int.Parse(Console.ReadLine());

                if (edad < 0 || edad > 120)
                {
                    throw new ArgumentOutOfRangeException();
                }

                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Debe ingresar un número válido.");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Error: La edad debe estar entre 0 y 120.");
            }
        }

        Console.WriteLine($"Edad ingresada correctamente: {edad} años");
    }
}
