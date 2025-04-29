using System;
using System.IO;

class Program
{
    static void Main()
    {
        double num1, num2;
        bool operacionExitosa = false;

        do
        {
            try
            {
                Console.Write("Ingresa el primer número: ");
                num1 = Convert.ToDouble(Console.ReadLine());

                Console.Write("Ingresa el segundo número: ");
                num2 = Convert.ToDouble(Console.ReadLine());

                if (num2 == 0)
                {
                    throw new DivideByZeroException("No se puede dividir por cero.");
                }

                double resultado = num1 / num2;
                Console.WriteLine($"Resultado: {num1} / {num2} = {resultado}");
                operacionExitosa = true;
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Ingresa solo números.");
                File.AppendAllText("log.txt", DateTime.Now + " - Error: Entrada no válida.\n");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                File.AppendAllText("log.txt", DateTime.Now + " - Error: División por cero.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inesperado: " + ex.Message);
                File.AppendAllText("log.txt", DateTime.Now + " - " + ex.Message + "\n");
            }
        } while (!operacionExitosa);
    }
}
