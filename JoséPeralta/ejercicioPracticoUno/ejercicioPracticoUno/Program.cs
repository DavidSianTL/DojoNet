using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ejercicioPracticoUno
{
    internal class Program
    {
        static void Main(string[] args)
        {
            menu();
        }


        static void menu()
        {
            Console.Clear();

            Console.WriteLine("Ingresa tu nombre: ");
            string nombre = Console.ReadLine();
            Console.WriteLine("Ingresa tu edad: ");
            int edad = int.Parse(Console.ReadLine());

            if (edad >= 18)
            {
                Console.WriteLine("Hola " + nombre + " eres mayor de edad");
            }
            else
            {
                Console.WriteLine("Hola " + nombre + " eres menor de edad");
            }

            Console.WriteLine("Ingresa tu calificacion: ");
            int calificacion = int.Parse(Console.ReadLine());

            if (calificacion >= 60)
            {
                Console.WriteLine("Felicidades " + nombre + " has aprobado");
            }
            else
            {
                Console.WriteLine("Lo siento " + nombre + " has reprobado");
            }

            Console.WriteLine("Gracias por usar nuestro programa, " + nombre);

            Console.ReadKey();

        }


    }
}
