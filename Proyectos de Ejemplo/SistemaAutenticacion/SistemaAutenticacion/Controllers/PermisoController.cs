using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaAutenticacion.Data.Permisos;
using SistemaAutenticacion.Dtos.PermisosDtos;

namespace SistemaAutenticacion.Controllers
{
    public class PermisoController : Controller
    {
        private readonly IPermisosRepository _permisosRepository;
        private readonly IMapper _mapper;

        public PermisoController(IPermisosRepository permisosRepository, IMapper mapper)
        {
            _permisosRepository = permisosRepository;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermisoResponseDto>>> GetPermisos()
        {
            var permisos = await _permisosRepository.GetPermisos();

            var permisosDto = _mapper.Map<IEnumerable<PermisoResponseDto>>(permisos);

            return Ok(permisosDto);
        }



    }
}
