using System;

class Animal
{
    public void HacerSonido()
    {
        Console.WriteLine("Hace un sonido....");
    }
}

class Perro : Animal
{
    public void Ladrar()
    {
        Console.WriteLine("guaff guafff prrrr guaff guaffff");
    }
}

class Program
{
    static void Main()
    {
        Perro miPerro = new Perro();

        miPerro.HacerSonido();
        miPerro.Ladrar();
    }
}
