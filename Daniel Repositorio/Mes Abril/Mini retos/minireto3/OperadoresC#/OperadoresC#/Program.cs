using System;

class program
{
    static void Main()
    {
        double baseRectangulo;
        double altura;

        Console.Write("Ingrese la base del rectangulo: ");
        baseRectangulo = double.Parse(Console.ReadLine());

        Console.Write("Ingrese la altura del rectangulo: ");    
        altura = double.Parse(Console.ReadLine());

        double area = baseRectangulo * altura;

        Console.WriteLine();
        Console.WriteLine("la base del rectangulo es: " + baseRectangulo + " y altura: " + altura);
        Console.WriteLine("El area del rectangulo es: "+area); 

        Console.ReadLine();


    }

}