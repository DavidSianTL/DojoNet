using Microsoft.AspNetCore.Mvc;
using NuevoEjemploConsumoApiSoap.Models.NumberModel;
using NuevoEjemploConsumoApiSoap.Services.NumberService;

namespace NuevoEjemploConsumoApiSoap.Controllers.NumberController
{
    public class NumberController : Controller
    {
        
        private readonly IConsumoDeMetodos _consumoDeMetodos;

        public NumberController(IConsumoDeMetodos consumoDeMetodos)
        {
            _consumoDeMetodos = consumoDeMetodos;
        }

        public async Task<IActionResult> ConvertirNumeroALetra(int number)
        {

            // Llamada al servicio SOAP
            var resultado = await _consumoDeMetodos.ConsumoDeMetodosAsync(number);

            // Verificamos si el resultado es nulo o vacío
            if (string.IsNullOrEmpty(resultado))
            {
                // Si el resultado es nulo o vacío, redirigimos a la vista de error
                return RedirectToAction("Error", "Home");
            }

            try
            {
                resultado = await _consumoDeMetodos.ConsumoDeMetodosAsync(number);

                return View("ConvertirNumeroALetra", new NumberViewModel
                {
                    Number = number,
                    Result = resultado
                });

            }
            catch(Exception e)
            {
                // Manejo de excepciones
                return RedirectToAction("Error", "Home");   
            }



        }


    }
}
