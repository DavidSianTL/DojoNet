// See https://aka.ms/new-console-template for more information
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        // Información de la persona
        string nombres, apellidos;
        DateTime fechaNacimiento;
        double[] notas;
        int cantidadMaterias;

        // Solicitar datos
        Console.WriteLine("Ingrese los nombres de la persona:");
        nombres = Console.ReadLine();

        Console.WriteLine("Ingrese los apellidos de la persona:");
        apellidos = Console.ReadLine();

        fechaNacimiento = SolicitarFechaNacimiento();

        Console.WriteLine("Ingrese la cantidad de materias:");
        cantidadMaterias = SolicitarCantidadMaterias();

        // Inicializar el arreglo de notas
        notas = new double[cantidadMaterias];
        for (int i = 0; i < cantidadMaterias; i++)
        {
            Console.WriteLine($"Ingrese la nota del bimestre {i + 1}:");
            notas[i] = SolicitarNota();
        }

        // Cálculos
        int edad = CalcularEdad(fechaNacimiento);
        double promedio = CalcularPromedio(notas);
        double media = CalcularMedia(notas);
        double moda = CalcularModa(notas);

        // Mostrar resultados
        Console.WriteLine("\n--- Resultado ---");
        Console.WriteLine($"Nombre: {nombres} {apellidos}");
        Console.WriteLine($"Edad: {edad} años");
        Console.WriteLine($"Promedio de notas: {promedio:F2}");
        Console.WriteLine($"Media de notas: {media:F2}");
        Console.WriteLine($"Moda de notas: {moda:F2}");
    }

    // Solicitar y validar fecha de nacimiento
    static DateTime SolicitarFechaNacimiento()
    {
        DateTime fechaNacimiento;
        while (true)
        {
            Console.WriteLine("Ingrese la fecha de nacimiento (dd/mm/yyyy):");
            string input = Console.ReadLine();
            if (DateTime.TryParse(input, out fechaNacimiento))
            {
                return fechaNacimiento;
            }
            else
            {
                Console.WriteLine("Fecha no válida. Intente nuevamente.");
            }
        }
    }

    // Solicitar cantidad de materias
    static int SolicitarCantidadMaterias()
    {
        int cantidad;
        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out cantidad) && cantidad > 0)
            {
                return cantidad;
            }
            else
            {
                Console.WriteLine("Ingrese un número válido de materias.");
            }
        }
    }

    // Solicitar y validar una nota
    static double SolicitarNota()
    {
        double nota;
        while (true)
        {
            string input = Console.ReadLine();
            if (double.TryParse(input, out nota) && nota >= 0 && nota <= 10)
            {
                return nota;
            }
            else
            {
                Console.WriteLine("Nota no válida. Debe estar entre 0 y 10. Intente nuevamente.");
            }
        }
    }

    // Calcular edad a partir de la fecha de nacimiento
    static int CalcularEdad(DateTime fechaNacimiento)
    {
        int edad = DateTime.Now.Year - fechaNacimiento.Year;
        if (DateTime.Now.DayOfYear < fechaNacimiento.DayOfYear)
        {
            edad--;
        }
        return edad;
    }

    // Calcular promedio de notas
    static double CalcularPromedio(double[] notas)
    {
        return notas.Average();
    }

    // Calcular media (que en este caso es lo mismo que promedio)
    static double CalcularMedia(double[] notas)
    {
        return notas.Average();
    }

    // Calcular moda de notas
    static double CalcularModa(double[] notas)
    {
        var grouped = notas.GroupBy(n => n)
                           .OrderByDescending(g => g.Count())
                           .ThenBy(g => g.Key)
                           .FirstOrDefault();
        return grouped?.Key ?? 0;
    }
}
