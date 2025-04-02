using System;

class Program
{
    static void Main()
    {
        Menu menu = new Menu();
        menu.MostrarMenu();
    }
}

class Menu
{
    public void MostrarMenu()
    {
        bool continuar = true;

        while (continuar)
        {
            Console.Clear();
            Console.WriteLine("*--*--*--* MENU DE OPCIONES *--*--*--*");
            Console.WriteLine("1️. Calculadora");
            Console.WriteLine("2️ Conversor de temperatura");
            Console.WriteLine("3️ Verificar si un número es par o impar");
            Console.WriteLine("4️ Salir");
            Console.Write("Elige una opción: ");

            if (int.TryParse(Console.ReadLine(), out int opcion))
            {
                switch (opcion)
                {
                    case 1:
                        Calculadora calculadora = new Calculadora();
                        calculadora.RealizarOperacion();
                        break;
                    case 2:
                        ConversorTemperatura conversor = new ConversorTemperatura();
                        conversor.ConvertirTemperatura();
                        break;
                    case 3:
                        VerificadorParImpar verificador = new VerificadorParImpar();
                        verificador.Verificar();
                        break;
                    case 4:
                        continuar = false;
                        Console.WriteLine("Saliendo del programa, gracias por utilizarlo.");
                        break;
                    default:
                        Console.WriteLine("Opción incorrecta. Intente de nuevo.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Error, por favor ingrese un numero.");
            }

            Console.WriteLine("\nPresiona ENTER para continuar...");
            Console.ReadLine();
        }
    }
}

class Calculadora
{
    private double num1, num2;

    public void RealizarOperacion()
    {
        Console.Clear();
        Console.Write("Ingrese el primer número: ");
        if (!double.TryParse(Console.ReadLine(), out num1))
        {
            Console.WriteLine("Entrada invalida.");
            return;
        }

        Console.Write("Ingrese el segundo número: ");
        if (!double.TryParse(Console.ReadLine(), out num2))
        {
            Console.WriteLine("Entrada invalida.");
            return;
        }

        Console.Write("Elige operación (+, -, *, /): ");
        char operacion = Console.ReadKey().KeyChar;
        Console.WriteLine();

        double resultado = operacion switch
        {
            '+' => num1 + num2,
            '-' => num1 - num2,
            '*' => num1 * num2,
            '/' => num2 != 0 ? num1 / num2 : double.NaN,
            _ => double.NaN
        };

        Console.WriteLine($"Resultado: {resultado}");
    }
}

class ConversorTemperatura
{
    private double temperatura;

    public void ConvertirTemperatura()
    {
        Console.Clear();
        Console.Write("Ingrese la temperatura: ");
        if (!double.TryParse(Console.ReadLine(), out temperatura))
        {
            Console.WriteLine("Error, coloque de nuevo la temperatura");
            return;
        }

        Console.WriteLine("Elige una opción:");
        Console.WriteLine("(C) Convertir a Celsius");
        Console.WriteLine("(F) Convertir a Fenheit");
        char unidad = char.ToUpper(Console.ReadKey().KeyChar);
        Console.WriteLine();

        if (unidad != 'C' && unidad != 'F')
        {
            Console.WriteLine("Error, coloque correctamente la opcion");
            return;
        }

        double resultado = unidad == 'C' ? (temperatura - 32) * 5 / 9 : (temperatura * 9 / 5) + 32;
        string tipo = unidad == 'C' ? "Celsius" : "Fahrenheit";

        Console.WriteLine($"Temperatura convertida: {resultado}° {tipo}");
    }
}


class VerificadorParImpar
{
    public void Verificar()
    {
        Console.Clear();
        Console.Write("Ingrese un número para verificar si es par o impar: ");

        if (int.TryParse(Console.ReadLine(), out int numero))
        {
            if (numero % 2 == 0)
            {
                Console.WriteLine($"El número {numero} es Par.");
            }
            else
            {
                Console.WriteLine($"El número {numero} es Impar.");
            }
        }
        else
        {
            Console.WriteLine("Erro en la entrada, ingrese un numero entero.");
        }
    }
}

