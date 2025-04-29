using System;

class ejercicio
{
    static void Main()
    {
        string nombre;
            int edad;
        int nota;

        Console.Write("ingrese un nombre y apellido:  ");
        nombre = Console.ReadLine();

        Console.Write("Ingrese su edad: ");
        edad = int.Parse(Console.ReadLine());

        Console.Write("Ingrese su nota: ");
        nota = int.Parse(Console.ReadLine());


        Console.WriteLine(" Te llamas "+ nombre+ " y tienes " +  edad + " años");
        

        string mensajeEdad = edad >= 18 ? "Eres mayor de edad." : "Eres menor de edad.";
        Console.WriteLine(mensajeEdad);

        string mensajeNota = nota >= 61 ? "Aprobaste." : "Reprobaste";
        Console.WriteLine(mensajeNota);
        Console.ReadLine();
    }



}