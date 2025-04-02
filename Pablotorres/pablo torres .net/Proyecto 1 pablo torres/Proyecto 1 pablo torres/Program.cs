// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
using System;

class Program
{
    static void Main()
    {
        // Solicitar el nombre del usuario
        Console.Write("Ingrese su nombre: ");
        string nombre = Console.ReadLine();

        // Solicitar la edad del usuario
        Console.Write("Ingrese su edad: ");
        int edad = int.Parse(Console.ReadLine());

        // Verificar si es mayor o menor de edad
        string estadoEdad = (edad >= 18) ? "Eres mayor de edad." : "Eres menor de edad.";
        Console.WriteLine(estadoEdad);

        // Solicitar la calificación
        Console.Write("Ingrese su calificación: ");
        int calificacion = int.Parse(Console.ReadLine());

        // Determinar si aprobó o reprobó
        string resultado = (calificacion >= 60) ? "Felicidades, has aprobado." : "Lo siento, has reprobado.";
        Console.WriteLine(resultado);

        // Mensaje final
        Console.WriteLine($"Gracias por usar nuestro programa, {nombre}.");
    }
}
