using System;

class program
{
    static void Main()
    {
        string nombre;
        int edad;
        Console.WriteLine("¿Cual es tu nombre?");
        nombre = Console.ReadLine();
        Console.WriteLine("¿Cual es tu edad?");
        edad = int.Parse(Console.ReadLine());

        Console.WriteLine("Te llamas: " + nombre + " y tienes: " + edad + " años.");
        Console.ReadLine();
    }   
   
}