using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ejerciciosPracticaDos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            menu();
        }


        static void menu()
        {

            funcPrimerReto();
            funcSegundoReto();


        }


        static void funcPrimerReto()
        {
            bool bolValidar = false;
            while (!bolValidar)
            {
                Console.WriteLine("Ingrese un número entre el 1-10");
                try
                {
                    int num = int.Parse(Console.ReadLine());

                    if (num >= 1 && num <= 10)
                    {
                        Console.WriteLine("El número está en el rango permitido");
                        bolValidar = true; // La entrada es válida
                    }
                    else
                    {
                        Console.WriteLine("Error: El número ingresado no está en el rango permitido");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: El valor ingresado no es un número");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            Console.ReadKey();
        }


        static void funcSegundoReto()
        {

            Console.Clear();

            bool bolValidar = false;
            while (!bolValidar)
            {
                Console.WriteLine("Ingresa tu edad");
                try
                {
                    int edad = int.Parse(Console.ReadLine());
                    if (edad < 0 && edad >= 120)
                    {
                        Console.WriteLine("Tu edad es valida");
                        bolValidar = true; // La entrada es válida
                    }
                    else
                    {
                        Console.WriteLine("Error: Dificilmente puedes tener esa edad");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: El valor ingresado no es un número");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }


            Console.ReadKey();


        }


        static void funcTercerReto()
        {

            Console.Clear();






            Console.ReadKey();

        }




    }
}
