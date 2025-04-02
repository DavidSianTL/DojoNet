using System;
abstract class Figura  
{
    public abstract double CalcularArea(); 
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

class Program
{
    static void Main()
    {
        Figura miCuadrado = new Cuadrado(5); 
        Figura miCirculo = new Circulo(3);    

        Console.WriteLine($"Area del cuadrado es: {miCuadrado.CalcularArea()}");
        Console.WriteLine($"Are del cirulo es: {miCirculo.CalcularArea()}");
    }
}
