using System;

class Program
{
    public static void Minireto1()
    {
        string nombre = "Diego";
        int edad = 25;
        Console.WriteLine($"Nombre: {nombre}, Edad: {edad}");
    }

    public static void Minireto2()
    {
        string apellido = "Catalan";
        int edad = 25;
        bool Verdadero = false;
        Console.WriteLine($"Apellido: {apellido}, Edad: {edad}, Valor booleano: {Verdadero}");
    }

    public static void Minireto3()
    {
        int bas = 5;
        int altura = 3;
        int area = bas * altura;
        Console.WriteLine($"Área del rectángulo: {area}");
    }

    public static void Minireto4()
    {
        int num1;
        Console.WriteLine("Ingresa un número");
        num1 = Convert.ToInt32(Console.ReadLine());
        if (num1 % 2 == 0)
        {
            Console.WriteLine($"El número: {num1} es par");
        }
        else
        {
            Console.WriteLine($"El número: {num1} no es par");
        }
    }

    public static void Minireto5()
    {
        Console.WriteLine("Ingresa tu nombre:");
        string nomb = Console.ReadLine();
        Console.WriteLine($"Tu nombre es: {nomb}");
    }

    public static void Minireto6()
    {
        Console.WriteLine("Ingresa tu edad:");
        int edad = Convert.ToInt32(Console.ReadLine());
        int edadFutu = edad + 10;
        Console.WriteLine($"En 10 años tendrás: {edadFutu} años.");
    }

    class Persona
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }

        public Persona(string nombre, int edad)
        {
            Nombre = nombre;
            Edad = edad;
        }

        public void MostrarDatos()
        {
            Console.WriteLine($"Nombre: {Nombre}, Edad: {Edad}");
        }
    }

    public static void Minireto7()
    {
        Persona persona1 = new Persona("Diego", 25);
        persona1.MostrarDatos();
    }

    class Coche
    {
        private int velocidad;

        public void MarcarVelocidad(int nuevaVelocidad)
        {
            velocidad = nuevaVelocidad;
        }

        public void MostrarVelocidad()
        {
            Console.WriteLine($"Velocidad actual: {velocidad} km/h");
        }
    }

    public static void Minireto8()
    {
        Coche coche1 = new Coche();

        Console.WriteLine("Ingrese la velocidad del coche:");
        int velocidad = Convert.ToInt32(Console.ReadLine());

        coche1.MarcarVelocidad(velocidad);
        coche1.MostrarVelocidad();
    }

    class Animal
    {
        public virtual void HacerSonido()
        {
            Console.WriteLine("El animal hace un sonido.");
        }
    }

    class Perro : Animal
    {
        public override void HacerSonido()
        {
            Console.WriteLine("Ladrido de perro");
        }
    }

    public static void Minireto9()
    {
        Perro perro1 = new Perro();
        perro1.HacerSonido();

        Animal animal1 = new Animal();
        animal1.HacerSonido();
    }

    class Figura
    {
        public virtual double CalcularArea()
        {
            return 0;
        }
    }

    class Cuadrado : Figura
    {
        public double Lado { get; set; }

        public Cuadrado(double lado)
        {
            Lado = lado;
        }

        public override double CalcularArea()
        {
            return Lado * Lado;
        }
    }

    class Circulo : Figura
    {
        public double Radio { get; set; }

        public Circulo(double radio)
        {
            Radio = radio;
        }

        public override double CalcularArea()
        {
            return Math.PI * Radio * Radio;
        }
    }

    public static void Minireto10()
    {
        Cuadrado cuadrado = new Cuadrado(5);
        Console.WriteLine($"Área del cuadrado: {cuadrado.CalcularArea()}");

        Circulo circulo = new Circulo(3);
        Console.WriteLine($"Área del círculo: {circulo.CalcularArea()}");
    }

    abstract class Figura1
    {
        public abstract double CalcularArea();
    }

    class Cuadrado1 : Figura1
    {
        public double Lado { get; set; }

        public Cuadrado1(double lado)
        {
            Lado = lado;
        }

        public override double CalcularArea()
        {
            return Lado * Lado;
        }
    }

    class Circulo1 : Figura1
    {
        public double Radio { get; set; }

        public Circulo1(double radio)
        {
            Radio = radio;
        }

        public override double CalcularArea()
        {
            return Math.PI * Radio * Radio;
        }
    }

    public static void Minireto11()
    {
        Cuadrado1 cuadrado = new Cuadrado1(5);
        Console.WriteLine($"Área del cuadrado: {cuadrado.CalcularArea()}");

        Circulo1 circulo = new Circulo1(3);
        Console.WriteLine($"Área del círculo: {circulo.CalcularArea()}");
    }

    public static void Minireto12()
    {
        
        dynamic variableDynamic = 10; 
        Console.WriteLine($"Valor inicial (número): {variableDynamic}");

        // Se cambia sin problemas a texto
        variableDynamic = "Ahora soy un texto";
        Console.WriteLine($"Valor después de cambiar a texto: {variableDynamic}");

        // Se tiene que convertir
        object numeroDecimal = 3.14m; 
        double numeroDouble = Convert.ToDouble(numeroDecimal); // Aqui se convierte
        Console.WriteLine($"Valor convertido a double: {numeroDouble}");
    }

    static void Main(string[] args)
    {
        int opcion;

        do
        {
            Console.WriteLine("Selecciona el mini-reto a ejecutar:");
            Console.WriteLine("1. Minireto 1");
            Console.WriteLine("2. Minireto 2");
            Console.WriteLine("3. Minireto 3");
            Console.WriteLine("4. Minireto 4");
            Console.WriteLine("5. Minireto 5");
            Console.WriteLine("6. Minireto 6");
            Console.WriteLine("7. Minireto 7");
            Console.WriteLine("8. Minireto 8");
            Console.WriteLine("9. Minireto 9");
            Console.WriteLine("10. Minireto 10");
            Console.WriteLine("11. Minireto 11");
            Console.WriteLine("12. Minireto 12");
            Console.WriteLine("0. Salir");

            opcion = Convert.ToInt32(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    Minireto1();
                    break;
                case 2:
                    Minireto2();
                    break;
                case 3:
                    Minireto3();
                    break;
                case 4:
                    Minireto4();
                    break;
                case 5:
                    Minireto5();
                    break;
                case 6:
                    Minireto6();
                    break;
                case 7:
                    Minireto7();
                    break;
                case 8:
                    Minireto8();
                    break;
                case 9:
                    Minireto9();
                    break;
                case 10:
                    Minireto10();
                    break;
                case 11:
                    Minireto11();
                    break;
                case 12:
                    Minireto12();
                    break;
                case 0:
                    Console.WriteLine("Gracias por usar el programa");
                    break;
                default:
                    Console.WriteLine("Opcion no valida por favor ingrese un número entre 0 y 12.");
                    break;
            }

            Console.WriteLine();

        } while (opcion != 0);
    }
}
