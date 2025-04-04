using System;

//Ejemplo de try catch
class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Introduce un número:");
            int numero = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("El número es: " + numero);
        }
        catch (FormatException e)
        {
            Console.WriteLine("Error: Formato de número inválido. " + e.Message);
        }
        catch (OverflowException e)
        {
            Console.WriteLine("Error: El número es demasiado grande o pequeño. " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error inesperado: " + e.Message);
        }
    }
}