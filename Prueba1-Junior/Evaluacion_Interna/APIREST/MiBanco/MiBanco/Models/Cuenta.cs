namespace MiBanco.Models
{
    public class Cuenta
    {
        int Id { get; set; }
        int FK_IdCliente { get; set; }
        string NumeroCuenta { get; set; } = string.Empty;
        decimal Saldo { get; set; }
        int FK_IdEstado { get; set; }

    }
}
