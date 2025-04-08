class Libro { 
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public int Anio { get; set; }

    public Libro(string titulo, string autor, int anio)
    {
        Titulo = titulo;
        Autor = autor;
        Anio = anio;
    }

    public void MostrarDatos()
    {
        Console.WriteLine($"Título: {Titulo}");
        Console.WriteLine($"Autor: {Autor}");
        Console.WriteLine($"Año: {Anio}");
    }

    class Program
    {
        static void Main()
        {
            Console.Write("Ingrese el título del libro 1: ");
            string titulo1 = Console.ReadLine();
            Console.Write("Ingrese el autor del libro 1: ");
            string autor1 = Console.ReadLine();
            Console.Write("Ingrese el año del libro 1: ");
            int anio1 = Convert.ToInt32(Console.ReadLine());
            Libro libro1 = new Libro(titulo1, autor1, anio1);
            Console.Write("Ingrese el título del libro 2: ");
            string titulo2 = Console.ReadLine();
            Console.Write("Ingrese el autor del libro 2: ");
            string autor2 = Console.ReadLine();
            Console.Write("Ingrese el año del libro 2: ");
            int anio2 = Convert.ToInt32(Console.ReadLine());
            Libro libro2 = new Libro(titulo2, autor2, anio2);
            Console.WriteLine();
            Console.WriteLine("Datos del libro 1:");
            libro1.MostrarDatos();
            Console.WriteLine();
            Console.WriteLine("Datos del libro 2:");
            libro2.MostrarDatos();
        }
    }

}