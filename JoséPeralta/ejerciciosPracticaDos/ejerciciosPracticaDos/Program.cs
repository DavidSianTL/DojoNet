using System;
using System.Collections.Generic;
using System.IO;
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
            funcTercerReto();

        }

        static void funcPrimerReto()
        {
            bool bolValidar = false;
            while (!bolValidar)
            {

                Console.Clear();

                try
                {
                    Console.WriteLine("Ingrese un número entre el 1-10");
                    int num = int.Parse(Console.ReadLine());

                    if (num >= 1 && num <= 10)
                    {
                        Console.WriteLine("El número está en el rango permitido");
                        bolValidar = true; // La entrada es válida y así se deja de ejecutar el while
                    }
                    else
                    {
                        Console.WriteLine("Error: El número ingresado no está en el rango permitido");
                        Console.ReadKey();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: El valor ingresado no es un número");
                    Console.ReadKey();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    Console.ReadKey();
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

                Console.Clear();

                try
                {
                    Console.WriteLine("Ingresa tu edad");
                    int edad = int.Parse(Console.ReadLine());
                    if (edad >= 0 && edad <= 120)
                    {
                        Console.WriteLine("Tu edad es valida");
                        bolValidar = true; // La entrada es válida
                    }
                    else
                    {
                        Console.WriteLine("Error: Dificilmente puedes tener esa edad");
                        Console.ReadKey();
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: El valor ingresado no es un número");
                    Console.ReadKey();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    Console.ReadKey();
                }
            }

            Console.ReadKey();

        }

        static void funcTercerReto()
        {

            Console.Clear();

            double resultado = 0.00;
            bool bolValidar = false;

            // Ruta para encontrar el archivo .txt para ver los Logs
            //C:\Users\josep\OneDrive\Documentos\DigitalGeko\DojoNet\JoséPeralta\ejerciciosPracticaDos\ejerciciosPracticaDos\bin\Debug

            while (!bolValidar)
            {
                try
                {

                    Console.Clear();

                    Console.WriteLine("Ingresa el primer número: ");
                    double num1 = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Ingresa el segundo número: ");
                    double num2 = Convert.ToDouble(Console.ReadLine());

                    resultado = num1 / num2;

                    if(resultado != double.NegativeInfinity && resultado != double.PositiveInfinity && resultado != double.NaN && resultado != 0)
                    {
                        Console.WriteLine("El resultado de la división es: " + resultado);
                        bolValidar = true;
                    }
                    else
                    {
                        Console.WriteLine("Error: No se puede dividir entre 0");

                        // Abrimos o guardamos en un archivo de texto los errores que se generen en el programa
                        // La línea de abajo guarda el error en un archivo de texto de la siguiente manera 
                        // 2021-09-29 12:00:00 - Error: No se puede dividir entre 0
                        File.AppendAllText("log.txt", DateTime.Now + " - Error: No se puede dividir entre 0" + Environment.NewLine);

                        Console.ReadKey();

                    }


                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: El valor ingresado no es un número");
                    
                    File.AppendAllText("log.txt", DateTime.Now + " - Error: El valor ingresado no es un número" + Environment.NewLine);

                    Console.ReadKey();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);

                    // Guardar error en un archivo
                    File.AppendAllText("log.txt", DateTime.Now + " - " + e.Message + Environment.NewLine);

                    Console.ReadKey();

                }
            }

            Console.ReadKey();

        }

        static void funcTercerRetoX()
        {
            Console.Clear();

            double resultado = 0.00;
            bool bolValidar = false;

            while (!bolValidar)
            {
                try
                {
                    Console.WriteLine("Ingresa el primer número: ");
                    double num1 = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Ingresa el segundo número: ");
                    double num2 = Convert.ToDouble(Console.ReadLine());

                    resultado = num1 / num2;

                    Console.WriteLine("El resultado de la división es: " + resultado);
                    bolValidar = true;
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("Error: No se puede dividir entre 0");
                    File.AppendAllText("log.txt", DateTime.Now + " - Error: No se puede dividir entre 0" + Environment.NewLine);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: El valor ingresado no es un número");
                    File.AppendAllText("log.txt", DateTime.Now + " - Error: El valor ingresado no es un número" + Environment.NewLine);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    File.AppendAllText("log.txt", DateTime.Now + " - " + e.Message + Environment.NewLine);
                }
            }

            Console.ReadKey();
        }



    }
}
