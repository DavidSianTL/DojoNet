using System;

class program
{
    static void Main()
    {
        string nombre;
        int edad;
        int añosF;



        Console.WriteLine("¿Como te llamas?");
        nombre = Console.ReadLine();
        Console.WriteLine("¿cuantos años tienes?");
        edad = int.Parse(Console.ReadLine());

        añosF = edad + 10;

        Console.WriteLine($"te llamas: {nombre}, y en 10 años tendras {añosF}");
        Console.ReadLine();
    }

}