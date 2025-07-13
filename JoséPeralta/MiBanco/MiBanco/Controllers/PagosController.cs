using MiBanco.Data;
using MiBanco.Models;
using MiBanco.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MiBanco.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagosController : ControllerBase
    {

        private readonly daoPagos _daoPagos;
        private readonly daoCuentas _daoCuentas;
        private readonly IBitacoraService _bitacoraService;

        // Constructor que recibe el daoPagos, daoCuentas y IBitacoraService para inyección de dependencias
        public PagosController(daoPagos daoPagos, daoCuentas daoCuentas, IBitacoraService bitacoraService)
        {
            _daoPagos = daoPagos;
            _daoCuentas = daoCuentas;
            _bitacoraService = bitacoraService;
        }

        [HttpPost]
        public ActionResult Pago(PagosViewModel pago)
        {
            try
            {
                // Validamos el tipo de pago (lo convertimos a minúsculas para evitar problemas de mayúsculas/minúsculas)
                string metodo = pago.TipoPago.ToLower();

                // Verificamos las cuentas origen y destino
                // En caso de ser un tipo de pago que no requiera cuenta origen, se asigna null
                var cuentaOrigen = _daoCuentas.ObtenerCuentaPorNumero(pago.CuentaOrigen) ?? null;
                var cuentaDestino = _daoCuentas.ObtenerCuentaPorNumero(pago.CuentaDestino);

                // Validamos la opción entrante según el modo de pago
                switch (metodo)
                {
                    case "transferencia":

                        // Verificamos que las cuentas origen y destino no sean nulas
                        if (cuentaOrigen == null)
                        {
                            return NotFound("Cuenta origen no encontrada, por favor, verifica el número de cuenta.");
                        }
                        if (cuentaDestino == null)
                        {
                            return NotFound("Cuenta destino no encontrada, por favor, verifica el número de cuenta.");
                        }

                        // Verificamos que el saldo sea suficiente
                        if (cuentaOrigen.Saldo < pago.Monto)
                        {
                            // Retornamos un BadRequest (esto al ser un error de cliente) 
                            return BadRequest("Saldo insuficiente, por favor, verifica el saldo de tu cuenta.");
                        }

                        // Realizamos el pago
                        _daoPagos.AgregarPago(pago);

                        // Actualizamos el saldo de la cuenta origen
                        _daoCuentas.DebitarCuenta(pago.CuentaOrigen, pago.Monto);

                        // Actualizamos el saldo de la cuenta destino
                        _daoCuentas.AcreditarCuenta(pago.CuentaDestino, pago.Monto);

                        // Guardamos la acción en la bitácora
                        _bitacoraService.RegistrarAccion("Pago", $"Pago realizado: {JsonSerializer.Serialize(pago)}");

                        return Ok($"Pago realizado exitosamente.");

                    case "cheque":

                        // Verificamos que las cuentas origen y destino no sean nulas
                        if (cuentaOrigen == null)
                        {
                            return NotFound("La cuenta origen es obligatoria para un depósito mediante un cheque.");
                        }
                        if (cuentaDestino == null)
                        {
                            return NotFound("La cuenta destino es obligatoria para un depósito mediante un cheque.");
                        }

                        // Verificamos que el saldo sea suficiente
                        if (cuentaOrigen.Saldo < pago.Monto)
                        {
                            // Retornamos un BadRequest (esto al ser un error de cliente) 
                            return BadRequest("Saldo insuficiente, por favor, verifica el saldo de tu cuenta.");
                        }

                        // Realizamos el pago
                        pago.FechaPago = DateTime.UtcNow; // Asignamos la fecha del pago
                        _daoPagos.AgregarPago(pago);

                        // Actualizamos el saldo de la cuenta origen
                        _daoCuentas.DebitarCuenta(pago.CuentaOrigen, pago.Monto);

                        // Actualizamos el saldo de la cuenta destino
                        _daoCuentas.AcreditarCuenta(pago.CuentaDestino, pago.Monto);

                        // Guardamos la acción en la bitácora
                        _bitacoraService.RegistrarAccion("Pago", $"Pago realizado: {JsonSerializer.Serialize(pago)}");

                        return Ok($"Pago realizado exitosamente.");

                    case "efectivo":

                        // Realizamos el pago en efectivo
                        _daoPagos.AgregarPago(pago);

                        // Actualizamos el saldo de la cuenta destino
                        _daoCuentas.AcreditarCuenta(pago.CuentaDestino, pago.Monto);

                        // Guardamos la acción en la bitácora
                        _bitacoraService.RegistrarAccion("Pago", $"Pago en efectivo realizado: {JsonSerializer.Serialize(pago)}");
                        return Ok($"Pago en efectivo realizado exitosamente.");

                    default:

                        // Si el tipo de pago no es válido, retornamos un error
                        return BadRequest("Tipo de pago no válido, por favor, verifica el tipo de pago.");

                }

            }
            catch (Exception e)
            {
                // Manejo de excepciones
                return BadRequest($"Error al procesar el pago: {e.Message}");
            }

        }



    }
}
