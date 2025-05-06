// MINI RETOS

// MINI RETO #1
using System.Runtime.CompilerServices;

string nombre;
int edad;

// MINI RETO #2
int numero;
string texto;
bool verdadero= true;

// MINI RETO #3
int area;
int baseR = 20;
int altura = 10;

area= baseR * altura;

Console.WriteLine("El área del rectángulo es: "+ area);

// MINI RETO #4
Console.WriteLine("Ingrese un número: ");
int num= int.Parse(Console.ReadLine());
int div= num % 2;
if (div == 0){
    Console.WriteLine("El número es par");
}else{
    Console.WriteLine("El número es impar");
}

// MINI RETO #5
string nombre2= "Juan";
void Datos(){
    Console.WriteLine("Bienvenid@ " + nombre2);
}
Datos();

//MINI RETO #6
Console.WriteLine("Ingrese su edad: ");
int edadU= int.Parse(Console.ReadLine());
int edadA= edadU + 10;
Console.WriteLine("En 10 años usted tendrá: "+ edadA);

// MINI RETO #7

//OBJETO
    Persona Persona1= new Persona();
    Persona1.NombreP= "Anika";
    Persona1.edadP= 22;
    Persona1.Bienvenida();
class Persona{
    public string NombreP;
    public int edadP;

   public void Bienvenida(){
     Console.WriteLine("¡¡BIENVENIDO/A!!! " + NombreP); 
     Console.WriteLine("Tu edad es: " + edadP + " años"); 
   }
}

// MINI RETO #8
class Carro{
    private int velocidad = 10;

    public void MostrarVelocidad(){
        Console.WriteLine("La velocidad del carro es: "+ velocidad +"km");
    }
}

// MINI RETO #9

class Animal{
    public void HacerSonido(){
        Console.WriteLine("El animal hace un sonido");
    }
}
class Perro:Animal{
    public void Ladrar(){
        Console.WriteLine("El perro hace ¡Guau Guau!");
    }
}

// MINI RETO #10
class Figura{
    public virtual void CalcularArea(){
        Console.WriteLine("El área de la figura es: 0");
    }
}

class Cuadrado:Figura{
    public override void CalcularArea(){
        Console.WriteLine("El área del cuadrado es: 4");
    }
}
class Circulo:Figura{
    public override void CalcularArea(){
        Console.WriteLine("El área del circulo es: 6");
    }
}

// MINI RETO #11

abstract class FiguraA{
    public double lado;
    public double radio;

    public FiguraA(double lado , double radio){
        this.lado = lado;
        this.radio= radio;
    }
    public abstract void CalcularAreas();

    public double AreaCuadrado()
    {
        return lado * lado;
    }
    public double AreaCirculo()
    {
        return Math.PI * radio * radio;
    }
}

class CuadradoA:FiguraA{
    public CuadradoA(double lado) : base(lado, 0) {}
    public override void CalcularAreas(){
        Console.WriteLine("El área del cuadrado es: " + AreaCuadrado());
    }
}
class CirculoA:FiguraA{
    public CirculoA(double radio) : base(0, radio) {}
    public override void CalcularAreas(){
        Console.WriteLine("El área del círculo es: " + AreaCirculo());
    }
}
class Ejecucion
{
    static void Main()
    {
        CuadradoA cuadrado = new CuadradoA(5);
        CirculoA circulo = new CirculoA(3);

        cuadrado.CalcularAreas();
        circulo.CalcularAreas();
    }
}
    



