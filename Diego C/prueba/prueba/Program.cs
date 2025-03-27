/*
 Crea un programa en C# que haga lo siguiente:

Define una clase base llamada Vehiculo con:

Un atributo Marca.

Un método MostrarDetalles() que imprima la marca del vehículo.

Crea una clase derivada llamada Auto que herede de Vehiculo y agregue:

Un atributo CantidadPuertas.

Un método MostrarDetalles() que, además de la marca, muestre la cantidad de puertas.

En el Main():

Crea un objeto de la clase Auto, asigna valores y muestra los detalles.

*/

class Vehiculo
{
    public string Marca { get; set;}
    public string Modelo { get; set; }
    
    public Vehiculo(string marca, string modelo)
    {
        Marca = marca;
        Modelo = modelo;
    }
    //Chat gpt
   /* virtual public string MostrarDetalles()
    {
        return $"Marca: {Marca}, Modelo: {Modelo}";
    }
   */
}
class Auto : Vehiculo
{
    public int CantidadPuertas { get; set; }
    public Auto(string marca, string modelo, int cantidadPuertas) : base(marca, modelo)
    {
        CantidadPuertas = cantidadPuertas;
    }
    //Chat gpt
    /*override public string MostrarDetalles()
    {
        return $"Marca: {Marca}, Modelo: {Modelo}, Cantudad de puertas {CantidadPuertas} ";
    }
    */
    public string MostrarDetalles()
    {
        return $"Marca: {Marca}, Modelo: {Modelo}, Cantudad de puertas {CantidadPuertas} ";
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("ingrese la marca del auto");
        string marca = Console.ReadLine();
        Console.WriteLine("ingrese el modelo del auto");
        string modelo = Console.ReadLine();
        Console.WriteLine("ingrese la cantidad de puertas del auto");
        int cantidadPuertas = Convert.ToInt32(Console.ReadLine());
        Auto auto = new Auto(marca, modelo, cantidadPuertas);
        Console.WriteLine(auto.MostrarDetalles());
    }
}


