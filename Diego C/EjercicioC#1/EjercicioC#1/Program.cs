
class Persona 
{
    public string Nombre { get; set; }
    public int Edad { get; set; }

    public Persona(string nombre, int edad)
    {
        Nombre = nombre;
        Edad = edad;
    }

    public string VerEdad()
    {
        return (Edad >= 18) ? "es mayor de edad" : "es menor de edad";
    }
}


class Estudiante : Persona // Estudiante hereda de Persona
{
    public double Calificacion { get; set; }

    public Estudiante(string nombre, int edad, double calificacion) : base(nombre, edad)
    {
        Calificacion = calificacion;
    }

    public string VerAprobacion()
    {
        return (Calificacion >= 60) ? " Felicidades, Aprobado" : " Reprobado";
    }

    public void MostrarInfo()
    {
        Console.WriteLine("Nombre: " + Nombre);
        Console.WriteLine("Edad: " + Edad + " años " + VerEdad());
        Console.WriteLine("Calificación: " + Calificacion + VerAprobacion());
        Console.WriteLine("Gracias por usar nuestro programa " + Nombre);
    }
}

class Program
{
    static void Main()
    {
        
        Console.Write("Ingrese su nombre: ");
        string nombre = Console.ReadLine();

        Console.Write("Ingresa tu edad: ");
        int edad = Convert.ToInt32(Console.ReadLine());

        Console.Write("Ingresa tu calificación: ");
        double calificacion = Convert.ToDouble(Console.ReadLine());

        // Objeto
        Estudiante estudiante = new Estudiante(nombre, edad, calificacion);

        // Metodo
        estudiante.MostrarInfo();
    }
}