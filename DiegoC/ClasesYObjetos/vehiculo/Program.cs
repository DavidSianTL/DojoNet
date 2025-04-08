class Vehiculo
{
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Anio { get; set; }

    public Vehiculo(string marca, string modelo, int anio)
    {
        Marca = marca;
        Modelo = modelo;
        Anio = anio;
    }

    public void MostrarDatos()
    {
        Console.WriteLine($"Marca: {Marca}");
        Console.WriteLine($"Modelo: {Modelo}");
        Console.WriteLine($"Año: {Anio}");
    }

    class Program
    {
        static void Main()
        {
            Console.Write("Ingrese la marca del vehículo 1: ");
            string marca1 = Console.ReadLine();
            Console.Write("Ingrese el modelo del vehículo 1: ");
            string modelo1 = Console.ReadLine();
            Console.Write("Ingrese el año del vehículo 1: ");
            int anio1 = Convert.ToInt32(Console.ReadLine());
            Vehiculo vehiculo1 = new Vehiculo(marca1, modelo1, anio1);
            Console.Write("Ingrese la marca del vehículo 2: ");
            string marca2 = Console.ReadLine();
            Console.Write("Ingrese el modelo del vehículo 2: ");
            string modelo2 = Console.ReadLine();
            Console.Write("Ingrese el año del vehículo 2: ");
            int anio2 = Convert.ToInt32(Console.ReadLine());
            Vehiculo vehiculo2 = new Vehiculo(marca2, modelo2, anio2);
            Console.WriteLine();
            Console.WriteLine("Datos del vehículo 1:");
            vehiculo1.MostrarDatos();
            Console.WriteLine();
            Console.WriteLine("Datos del vehículo 2:");
            vehiculo2.MostrarDatos();
        }
    }

}