using System;

class Figura
{
    public virtual void CalcularArea()
    {
        Console.WriteLine("Calculando are de la figura... ");
    }
}

class Cuadrado : Figura
{
    public double Lado { get; set; }

    public Cuadrado(double lado)
    {
        Lado = lado;
    }

    public override void CalcularArea()
    {
        double area = Lado * Lado;
        Console.WriteLine($"El área del cuadrado es: {area}");
    }
}

class Circulo : Figura
{
    public double Radio { get; set; }

    public Circulo(double radio)
    {
        Radio = radio;
    }

    public override void CalcularArea()
    {
        double area = Math.PI * Radio * Radio;
        Console.WriteLine($"El área del círculo es: {area}");
    }
}

class Program
{
    static void Main()
    {
        
        Figura figura = new Figura();
        figura.CalcularArea();  

        Cuadrado miCuadrado = new Cuadrado(4);
        Circulo miCirculo = new Circulo(3);

    
        miCuadrado.CalcularArea();  
        miCirculo.CalcularArea();   
    }
}
