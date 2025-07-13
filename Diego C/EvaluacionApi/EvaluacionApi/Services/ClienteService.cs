using EvaluacionApi.Models;

namespace EvaluacionApi.Services
{
    public class ClienteService
    {
        private readonly List<Cliente> _clientes = new();
        private int _idCounter = 1;
        private readonly LogService _log;

        public ClienteService(LogService log)
        {
            _log = log;
        }

        public Cliente? ObtenerPorDpi(string dpi)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Dpi == dpi);

            if (cliente != null)
                _log.Registrar($"Se consulto el cliente con DPI: {dpi}  Nombre: {cliente.Nombre} {cliente.Apellido}");
            else
                _log.Registrar($"Se intento consultar un cliente inexistente con DPI: {dpi}");

            return cliente;
        }

        public Cliente Crear(Cliente nuevo)
        {
            if (_clientes.Any(c => c.Dpi == nuevo.Dpi))
            {
                _log.Registrar($"Error: intento de crear cliente duplicado con DPI {nuevo.Dpi}");
                return null;
            }

            nuevo.Id = _idCounter++;
            _clientes.Add(nuevo);

            _log.Registrar($"Se creo un nuevo cliente: {nuevo.Nombre} {nuevo.Apellido}, DPI: {nuevo.Dpi}");
            return nuevo;
        }


    }
}
