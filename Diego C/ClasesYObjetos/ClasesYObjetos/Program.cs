using System;

class CuentaBanco // Clase CuentaBanco
{
    public string Titular { get; set; }
    public double Saldo { get; set; }

    public CuentaBanco(string nombre, double SaldoInicial) // Constructor   
    {
        Titular = nombre;
        Saldo = SaldoInicial;
    }

    public void Depositar(double cantidad) // Método depositar
    {
        Saldo += cantidad;
        Console.WriteLine($"Depósito hecho, su nuevo saldo es: {Saldo}");
    }

    public void Retirar(double cantidad) // Metodo retirar
    {
        if (cantidad > Saldo)
        {
            Console.WriteLine("Fondos insuficientes");
        }
        else
        {
            Saldo -= cantidad;
            Console.WriteLine($"Retiro hecho, su nuevo saldo es: {Saldo}");
        }
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Ingrese el nombre del titular: ");
        string titular = Console.ReadLine();

        Console.Write("Ingrese el saldo inicial: ");
        double SaldoInicial = Convert.ToDouble(Console.ReadLine()); //pide datos al usuario

        CuentaBanco cuenta = new CuentaBanco(titular, SaldoInicial); //crea un objeto de la clase CuentaBanco //cuenta es el objeto, creado con la clase CuentaBanco

        Console.Write("Ingrese la cantidad a depositar: ");
        double deposito = Convert.ToDouble(Console.ReadLine());
        cuenta.Depositar(deposito); //llama al método Depositar

        Console.Write("Ingrese la cantidad a retirar: ");
        double retiro = Convert.ToDouble(Console.ReadLine());
        cuenta.Retirar(retiro); //llama al método Retirar
    }
}