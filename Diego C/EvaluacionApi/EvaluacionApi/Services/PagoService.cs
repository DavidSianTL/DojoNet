using EvaluacionApi.Models;

namespace EvaluacionApi.Services
{
    public class PagoService
    {
        private readonly List<Pago> _pagos = new();
        private int _idCounter = 1;
        private readonly ClienteService _clienteService;
        private readonly LogService _log;

        public PagoService(ClienteService clienteService, LogService log)
        {
            _clienteService = clienteService;
            _log = log;
        }
        public (bool Exitoso, string Mensaje, Pago? Resultado) Registrar(Pago nuevo)
        {
            var cliente = _clienteService.ObtenerPorDpi(nuevo.Dpi);
            if (cliente == null)
            {
                var msg = $"No se encontro un cliente con DPI {nuevo.Dpi}";
                _log.Registrar("Error: " + msg);
                return (false, msg, null);
            }

            if (cliente.Saldo <= 0)
            {
                var msg = $"El cliente con DPI {nuevo.Dpi} no tiene saldo pendiente.";
                _log.Registrar("Error: " + msg);
                return (false, msg, null);
            }

            if (nuevo.Monto > cliente.Saldo)
            {
                var msg = $"El pago Q{nuevo.Monto} excede el saldo Q{cliente.Saldo} del cliente con DPI {nuevo.Dpi}.";
                _log.Registrar("Error: " + msg);
                return (false, msg, null);
            }

            nuevo.Id = _idCounter++;
            _pagos.Add(nuevo);
            cliente.Saldo -= nuevo.Monto;

            _log.Registrar($"Pago registrado: Q{nuevo.Monto} para DPI {nuevo.Dpi}. Nueva deuda: Q{cliente.Saldo}");
            return (true, "Pago registrado con exito", nuevo);
        }


    }
}
