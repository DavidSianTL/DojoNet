using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Appmvc.Models;

namespace Appmvc.Controllers
{
    [Route("Persona")]
    public class PersonaController : Controller
    {
        private readonly ILogger<PersonaController> _logger;

        public PersonaController(ILogger<PersonaController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<Persona> lista = new List<Persona>();

            Persona per1 = new Persona { codigo = 1, nombre = "Erick", apepat = "Perez", apemat = "Gonzales" };
            Persona per2 = new Persona { codigo = 2, nombre = "Juni", apepat = "bodoque", apemat = "Gonzales" };
            Persona per3 = new Persona { codigo = 3, nombre = "tulio", apepat = "Perez", apemat = "tribillo" };

            lista.Add(per1);
            lista.Add(per2);
            lista.Add(per3);

            return View(lista);
        }

        [HttpGet("Error")]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}
