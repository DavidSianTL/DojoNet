using MiBanco.Data.DTOs.ClienteDTOs;
using MiBanco.Models;

namespace MiBanco.Helpers
{
    public class ClienteConverter
    {
        public static ClienteResponseDTO? ConvertClienteToClienteResponseDTO(Cliente cliente)
        {
            if (cliente == null) return null;

            var clienteResponse = new ClienteResponseDTO
            {
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                NIT = cliente.NIT,
                Direccion = cliente.Direccion,
                Estado = cliente.Estado
            };

            return clienteResponse;
        }

        public static Cliente? ConvertClienteRequestDTOToCliente(ClienteRequestDTO clienteRequest)
        {
            if (clienteRequest == null) return null;

            var cliente = new Cliente
            {
                DPI = clienteRequest.DPI,
                Nombre = clienteRequest.Nombre,
                Apellido = clienteRequest.Apellido,
                NIT = clienteRequest.NIT,
                Direccion = clienteRequest.Direccion,
                Estado = 1 //  0 = Inactivo, 1 = Activo, etc... (supongamos) aunque idealmente debría hacerse en la base de datos
            };

            return cliente;
        }
    }
}
