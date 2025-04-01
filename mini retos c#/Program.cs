class Program
// Repasamos los retos en una reunion con david sian, habian temas que no estaban claros.
{
    static void Main()
    {
        // RETO 1
       string nombre = "pablo";
     string apellido = "torres";
     int edad = 25; 
     Console.WriteLine("mi nombre es " + nombre + apellido + " y tengo "+ edad + " años" );

        // RETO 2 
        int numero = 90;
        string texto = "Hola, me gusta cocinar";
        bool valor = true;
        Console.WriteLine($"Número: {numero}");
        Console.WriteLine($"Texto: {texto}");
        Console.WriteLine($"Valor: {valor}");

        // RETO 3
        int baseRectangulo = 55;
        int alturaRectangulo = 12;
        int areaRectangulo = baseRectangulo * alturaRectangulo;
        Console.WriteLine($"El área del rectángulo es: {areaRectangulo}");

        // RETO 4
        int numeroParImpar = 89;
        Console.WriteLine($"El número {numeroParImpar} es {(numeroParImpar % 2 == 0 ? "par" : "impar")}");

        // RETO 5
        Saludar(nombre);
        Saludar(apellido);

        // RETO 6
        Console.Write("Ingresa tu edad Para calcular su edad en 10 años: ");
        if (int.TryParse(Console.ReadLine(), out int edadUsuario))
        {
            Console.WriteLine($"En 10 años tendrás {edadUsuario + 10} años");
        }
        else
        {
            Console.WriteLine("Por favor, ingresa una edad válida.");
        }

        // RETO 7
        Persona personaClase = new Persona { NombrePersona = "leonel", Edad = 25 };
        Console.WriteLine($"Nombre: {personaClase.NombrePersona}, Edad: {personaClase.Edad}");

        // RETO 8
        Coche cocheObjeto = new Coche();
        cocheObjeto.EstablecerVelocidad(100);
        Console.WriteLine($"La velocidad del coche es: {cocheObjeto.ObtenerVelocidad()} km/h");

        // RETO 9
        Perro miPerro = new Perro();
        miPerro.HacerSonido();
        miPerro.Ladrar();

        // RETO 10
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