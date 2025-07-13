using MiBanco.Models;

namespace MiBanco.Data
{
    public class daoCuentas
    {

        // Lista de Cuentas para simular la base de datos en memoria
        private List<CuentasViewModel> cuentas = new List<CuentasViewModel>
        {
            new CuentasViewModel{
                Id = 1,
                TipoCuenta = "Ahorros",
                NumeroCuenta = "1234567890",
                Saldo = 1000.00m,
                ClienteId = 1,
                SucursalId = 1,
                FechaApertura = DateTime.UtcNow
            },
            new CuentasViewModel
            {
                Id = 2,
                TipoCuenta = "Monetaria",
                NumeroCuenta = "0987654321",
                Saldo = 7000.00m,
                ClienteId = 2,
                SucursalId = 1,
                FechaApertura = DateTime.UtcNow
            },
            new CuentasViewModel
            {
                Id = 3,
                TipoCuenta = "Monetaria",
                NumeroCuenta = "1122334455",
                Saldo = 4700.00m,
                ClienteId = 3,
                SucursalId = 1,
                FechaApertura = DateTime.UtcNow
            }
        };

        // Función (método) que obtiene una lista de las cuentas
        public List<CuentasViewModel> ObtenerCuentas()
        {
            return cuentas;
        }

        // Función (método) que obtiene una cuenta por su número de cuenta
        public CuentasViewModel ObtenerCuentaPorNumero(string numeroCuenta)
        {
            // Buscamos la cuenta en la lista por su número de cuenta
            return cuentas.FirstOrDefault(c => c.NumeroCuenta == numeroCuenta);
        }

        // Función (método) que acredita el saldo de una cuenta
        public void AcreditarCuenta(string numeroCuenta, decimal nuevoSaldo)
        {
            // Buscamos la cuenta en la lista por su número de cuenta
            var cuenta = cuentas.FirstOrDefault(c => c.NumeroCuenta == numeroCuenta);
            if (cuenta != null)
            {
                // Actualizamos el saldo de la cuenta
                cuenta.Saldo += nuevoSaldo;
            }
            else
            {
                throw new Exception("Cuenta no encontrada.");
            }
        }

        // Función (método) que debita una cuenta
        public void DebitarCuenta(string numeroCuenta, decimal monto)
        {
            // Buscamos la cuenta en la lista por su número de cuenta
            var cuenta = cuentas.FirstOrDefault(c => c.NumeroCuenta == numeroCuenta);
            if (cuenta != null)
            {
                // Verificamos que el saldo sea suficiente
                if (cuenta.Saldo >= monto)
                {
                    // Actualizamos el saldo de la cuenta
                    cuenta.Saldo -= monto;
                }
                else
                {
                    throw new Exception("Saldo insuficiente.");
                }
            }
            else
            {
                throw new Exception("Cuenta no encontrada.");
            }
        }

    }
}
