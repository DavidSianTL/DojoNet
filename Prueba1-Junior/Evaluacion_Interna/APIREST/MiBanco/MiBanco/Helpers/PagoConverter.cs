using MiBanco.Data.DTOs.ClienteDTOs;
using MiBanco.Data.DTOs.PagoDTOs;
using MiBanco.Models;

namespace MiBanco.Helpers
{
    public class PagoConverter
    {
        public static Pago? ConvertPagoRequestDTOToPago(PagoRequestDTO clienteRequest)
        {
            if (clienteRequest == null) return null;

            var pago = new Pago
            {
                FK_IdCuentaOrigen = clienteRequest.FK_IdCuentaOrigen,
                FK_IdServicio = clienteRequest.FK_IdServicio,
                Monto = clienteRequest.Monto,
                Descripcion = clienteRequest.Descripcion,
                Fecha = DateTime.UtcNow,
                FK_IdEstado = 1
            };

            return pago;
        }
    }
}
