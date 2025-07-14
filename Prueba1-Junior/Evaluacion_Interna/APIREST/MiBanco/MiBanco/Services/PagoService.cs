using MiBanco.Data.DAOs;
using MiBanco.Data.DTOs;
using MiBanco.Data.DTOs.PagoDTOs;
using MiBanco.Helpers;
using MiBanco.Models;

namespace MiBanco.Services
{
    public interface IPagoService
    {
        Response<bool> AddPago(PagoRequestDTO pagoRequest);
    }

    public class PagoService : IPagoService
    {
        private readonly ILogService _logService;
        private readonly IPagoDAO _pagoDao;

        public PagoService(ILogService logService, IPagoDAO pagoDao)
        {
            _logService = logService;
            _pagoDao = pagoDao;
        }


        public Response<bool> AddPago(PagoRequestDTO pagoRequest)
        {
            if (pagoRequest == null)
            {
                _logService.LogError(400, "El pago no puede ser nulo.");
                return Response<bool>.Malo(400, "El pago no puede ser nulo");
            }

            var pago = PagoConverter.ConvertPagoRequestDTOToPago(pagoRequest);

            var pagos = _pagoDao.GetPagos();

            if (pago == null)
            {
                _logService.LogError(500, "No se pudo convertir el pagoRequestDTO a un objeto Pago.");
                return Response<bool>.Malo(500, "No pudo agregar el pago, intentalo más tarde.");
            }

            pago.Id = GenerateId(pagos);

            var pagoAgregado = _pagoDao.AddPagos(pago);

            if (!pagoAgregado)
            {
                _logService.LogError(500, "No se pudo agregar el pago a la base de datos.");
                return Response<bool>.Malo(500, "No se pudo agregar el pago, intentalo más tarde.");
            }

            _logService.LogInfo(200, $"Pago agregado exitosamente con ID: {pago.Id}");
            return Response<bool>.Bueno(200, "Pago agregado exitosamente.", true);

        }


        public int GenerateId(List<Pago> pago)
        {
            if (pago == null || !pago.Any()) return 1; // Si la lista está vacia se devuelve 1 (para comenzar la secuencia)

            int IdMayor = pago.Max(obj => obj.Id);

            var id = IdMayor + 1;

            return id;
        }

    }
}
