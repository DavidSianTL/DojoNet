//MENÚ PRINCIPAL
using System;
using System.IO;
class Programa
{
    static void Main()
    {
        Reto1 reto = new Reto1(); //Crear una instancia de la clase reto1
        Reto2 reto2 = new Reto2(); //Crear una instancia de la clase reto2
        Reto3 reto3 = new Reto3(); //Crear una instancia de la clase reto2
        bool salir = false; // variable para salir del programa

        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("------ MENÚ PRINCIPAL ------");
            Console.WriteLine("1. VALIDAR NÚMEROS - MINI RETO 1");
            Console.WriteLine("2. VALIDAR EDAD - MINI RETO 2");
            Console.WriteLine("3. USO DE LOG - MINI RETO 3");
            Console.WriteLine("4. SALIR ");
            Console.WriteLine("Por favor seleccione una opción: ");
            int opcion = Convert.ToInt32(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    Console.Clear();
                    reto.PedirNumero();
                    break;

                case 2:
                    Console.Clear();
                    reto2.PedirEdad();
                    break;

                case 3:
                    Console.Clear();
                    reto3.Log();
                    break;

                case 4:
                    salir = true;
                    break;
            }
        }
    }



    //MINI RETO1
    class Reto1
    {
        public void PedirNumero()
        {
            bool numValido = false;

            while (!numValido)
            {
                try
                {

                    Console.Write("Ingresa un número de 1 a 10: ");
                    int numero = Convert.ToInt32(Console.ReadLine());

                    if (numero >= 1 && numero <= 10)
                    {
                        Console.WriteLine("El número ingresado es válido: " + numero);
                        numValido = true;
                    }
                    else
                    {
                        Console.WriteLine("Por favor ingrese un número de 1 a 10");

                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: Solo se permiten numeros");
                }
            }
            Console.WriteLine("\nPresiona cualquier tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        
        }
    }
}

//MINI RETO2
class Reto2
{
    public void PedirEdad()
    {
        bool edadValido = false;

        do
        {
            try
            {
                Console.Write("Ingresa tu edad: ");
                int edad = Convert.ToInt32(Console.ReadLine());

                if (edad >= 0 && edad <= 120)
                {
                    Console.WriteLine("Edad válida, su edad es: " + edad);
                    edadValido = true;
                }
                else
                {
                    Console.WriteLine("Por favor ingrese su edad nuevamente (en un rango de 0 a 120)");

                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Debe estar en un rango entre 0 a 120 años");
            }
        } while (!edadValido);
        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
        Console.Clear();

    }
}
    //MINI RETO 3
class Reto3
    {
        public void Log()
        {
        try
        {
            Console.Write(" Ingresa un número: ");
            int numero = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Número ingresado: " + numero);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocurrió un error. Revisa el archivo 'log.txt'.");

            // Guardar error en un archivo
            File.AppendAllText("log.txt", DateTime.Now + " - " + ex.Message + Environment.NewLine);
        }
        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
    }
}











