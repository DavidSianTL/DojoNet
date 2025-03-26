using System;

class Program
{
    static void Main()
    {
        
        Console.Write("Ingrese su nombre: ");
        string nombre = Console.ReadLine();

        
        Console.Write("Ingrese su edad: ");
        int edad = int.Parse(Console.ReadLine());

        
        string mensajeEdad = (edad >= 18) ? "Eres mayor de edad." : "Eres menor de edad.";

        
        Console.Write("Ingrese su calificación: ");
        int calificacion = int.Parse(Console.ReadLine());

       
        string mensajeCalificacion = (calificacion >= 60) ? "Felicidades, has aprobado." : "Lo siento, has reprobado.";

        
        Console.WriteLine($"\n{mensajeEdad}");
        Console.WriteLine(mensajeCalificacion);
        Console.WriteLine($"Gracias por usar nuestro programa, {nombre}.");
    }
}

