

Variables para almacenar nombre y edad:
    string nombre = "TuNombre";
int edad = 25;




Declarar tres variables (entero, texto, booleano):
int numero = 10;
string texto = "Hola, mundo";
bool esVerdadero = true;






Calcular el área de un rectángulo con base y altura:
double baseRectangulo = 5.0;
double altura = 3.0;
double area = baseRectangulo * altura;



If para verificar si un número es par o impar:
int numero = 7;
if (numero % 2 == 0)
{
    Console.WriteLine("Es par.");
}
else
{
    Console.WriteLine("Es impar.");
}





Método que recibe un nombre e imprime en pantalla:
void MostrarNombre(string nombre)
{
    Console.WriteLine("Tu nombre es: " + nombre);
}




Pedir edad y mostrar cuántos años tendrá en 10 años:
Console.Write("Ingrese su edad: ");
int edad = int.Parse(Console.ReadLine());
Console.WriteLine("En 10 años tendrás: " + (edad + 10));




Clase Persona con propiedades nombre y edad:
class Persona
{
    public string Nombre;
    public int Edad;
}
Persona persona1 = new Persona { Nombre = "Carlos", Edad = 30 };
Console.WriteLine($"Nombre: {persona1.Nombre}, Edad: {persona1.Edad}");





Clase Coche con velocidad privada y método público:
class Coche
{
    private int velocidad;

    public void SetVelocidad(int nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }
}




Clase Animal con HacerSonido() y Perro que hereda:
class Animal
{
    public void HacerSonido()
    {
        Console.WriteLine("Hace un sonido...");
    }
}

class Perro : Animal
{
    public void Ladrar()
    {
        Console.WriteLine("¡Guau Guau!");
    }
}






Clase Figura con CalcularArea(), Cuadrado y Círculo:
class Figura
{
    public virtual double CalcularArea()
    {
        return 0;
    }
}

class Cuadrado : Figura
{
    public double Lado;
    public override double CalcularArea() => Lado * Lado;
}

class Circulo : Figura
{
    public double Radio;
    public override double CalcularArea() => Math.PI * Radio * Radio;
}





Clase Figura abstracta con CalcularArea(), Cuadrado y Círculo:
abstract class Figura
{
    public abstract double CalcularArea();
}

class Cuadrado : Figura
{
    public double Lado;
    public override double CalcularArea() => Lado * Lado;
}

class Circulo : Figura
{
    public double Radio;
    public override double CalcularArea() => Math.PI * Radio * Radio;
}





Variable dynamic y object:


dynamic variable = 10;
variable = "Ahora soy un texto";

object obj = 5.5;
double numero = (double)obj;
Console.WriteLine(numero);