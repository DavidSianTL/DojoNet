using System;

class Program
{
    static void Main()
    {
        // Mini reto 1: Variables de nombre y edad
        string nombre = "David";
        int edad = 25;
        Console.WriteLine($"Mi nombre es {nombre} y tengo {edad} años");

        // Mini reto 2: Variables de diferentes tipos
        int numero = 10;
        string texto = "Hola";
        bool valor = true;
        Console.WriteLine($"Número: {numero}");
        Console.WriteLine($"Texto: {texto}");
        Console.WriteLine($"Valor: {valor}");

        // Mini reto 3: Cálculo de área de un rectángulo
        int baseRectangulo = 10;
        int alturaRectangulo = 5;
        int areaRectangulo = baseRectangulo * alturaRectangulo;
        Console.WriteLine($"El área del rectángulo es: {areaRectangulo}");

        // Mini reto 4: Verificar si un número es par o impar
        int numeroParImpar = 5;
        Console.WriteLine($"El número {numeroParImpar} es {(numeroParImpar % 2 == 0 ? "par" : "impar")}");

        // Mini reto 5: Método para saludar
        Saludar(nombre);

        // Mini reto 6: Pedir edad y calcularla en 10 años
        Console.Write("Ingresa tu edad: ");
        if (int.TryParse(Console.ReadLine(), out int edadUsuario))
        {
            Console.WriteLine($"En 10 años tendrás {edadUsuario + 10} años");
        }
        else
        {
            Console.WriteLine("Por favor, ingresa una edad válida.");
        }

        // Mini reto 7: Creación de una clase Persona
        Persona personaClase = new Persona { NombrePersona = "Carlos", Edad = 25 };
        Console.WriteLine($"Nombre: {personaClase.NombrePersona}, Edad: {personaClase.Edad}");

        // Mini reto 8: Clase Coche con velocidad privada
        Coche cocheObjeto = new Coche();
        cocheObjeto.EstablecerVelocidad(100);
        Console.WriteLine($"La velocidad del coche es: {cocheObjeto.ObtenerVelocidad()} km/h");

        // Mini reto 9: Herencia con la clase Perro
        Perro miPerro = new Perro();
        miPerro.HacerSonido();
        miPerro.Ladrar();

        // Mini reto 10: Clases Cuadrado y Círculo con cálculo de área
        Cuadrado cuadrado = new Cuadrado(5);
        Circulo circulo = new Circulo(3);
        Console.WriteLine($"Área del cuadrado: {cuadrado.CalcularArea()}");
        Console.WriteLine($"Área del círculo: {circulo.CalcularArea()}");
    }

    // Método Saludar
    static void Saludar(string nombre)
    {
        Console.WriteLine($"Hola {nombre}");
    }
}

// Clase Persona con propiedades autoimplementadas
class Persona
{
    public string NombrePersona { get; set; }
    public int Edad { get; set; }
}

// Clase Coche con velocidad privada
class Coche
{
    private int velocidad;

    public void EstablecerVelocidad(int velocidadCoche)
    {
        velocidad = velocidadCoche;
    }

    public int ObtenerVelocidad()
    {
        return velocidad;
    }
}

// Clase Animal con método HacerSonido()
class Animal
{
    public void HacerSonido()
    {
        Console.WriteLine("Hace un sonido...");
    }
}

// Clase Perro que hereda de Animal
class Perro : Animal
{
    public void Ladrar()
    {
        Console.WriteLine("¡Guau, guau!");
    }
}

// Clase abstracta FiguraBase con método abstracto CalcularArea()
abstract class FiguraBase
{
    public abstract double CalcularArea();
}

// Clase Cuadrado que hereda de FiguraBase
class Cuadrado : FiguraBase
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

// Clase Circulo que hereda de FiguraBase
class Circulo : FiguraBase
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
