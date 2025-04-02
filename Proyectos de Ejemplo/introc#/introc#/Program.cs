//menu en c# con funciones 
using System;
using System.ComponentModel;
using System.Xml;

class Program
{
    static void Main()
    {
        //aqui va la del menu 
        Menu();
        
        //funciones o metodos 
    }
    static void Menu () {
            int Opcion = 0;

            do {
                Console.Clear();
                Console.WriteLine("#####################");
                Console.WriteLine("#     1.sumar        #");
                Console.WriteLine("#    2. restar       #");
                Console.WriteLine("#    3. al cuadrado  #");
                Console.WriteLine("######################");

                Opcion = int.Parse(Console.ReadLine());

                switch (Opcion)
                {
                    case 1:
                        Sumarnumeros();
                        break;
                    case 2:
                        Restarnumeros();
                        break;
                    case 3:
                        AlCuadrado();s
                        break;
                    default:
                        Console.WriteLine("Ingrese una opcion valida");
                        break;

                }
                Console.WriteLine("Presiona cualquier tecla para continuar...");
                Console.ReadKey();
    
        } while (Opcion != 3);

            static void Sumarnumeros()
            {
                Saludar();
                Console.WriteLine("Ingresa el primer numero: ");
                int num1 = int.Parse(Console.ReadLine());
                Console.WriteLine("Ingresa el segundo numero: ");
                int num2 = int.Parse(Console.ReadLine());
                int resultado = num1 + num2;
                Console.WriteLine($"EL resulado es = {resultado}");
            }
            
            static void Restarnumeros()
            {
                Saludar();
                Console.WriteLine("Ingresa el primer numero:  ");
                int num1 = int.Parse(Console.ReadLine());
                Console.WriteLine("Ingrese el segundo numero: ");
                int num2 = int.Parse(Console.ReadLine());
                int resultado = num1 - num2;
                Console.WriteLine($"El resultado es {resultado}");


            }

            static void AlCuadrado()
            {
                Saludar();
                Console.WriteLine("Ingrese el numero que deses elevar al cuadrado: ");
                int num1 = int.Parse(Console.ReadLine());
                int restultado = num1 * num1;
                Console.WriteLine($"El cuadrado de {num1} es {restultado}");

            }

            static void Saludar()
            {
                Console.WriteLine("Hola ten un buen dia ");
            }
    }
}
