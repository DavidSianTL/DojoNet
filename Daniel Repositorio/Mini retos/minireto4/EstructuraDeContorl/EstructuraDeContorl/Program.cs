using System;

class program
{
    static void Main()
    {
        Console.Write("Ingrese un numero: ");
        int numero = int.Parse(Console.ReadLine());

        if (numero % 2 == 0)
        {
            Console.WriteLine($"El número {numero} es PAR.");
        }
        else
        {
            Console.WriteLine($"El número {numero} es IMPAR.");
        }

        Console.ReadLine();


    }


}