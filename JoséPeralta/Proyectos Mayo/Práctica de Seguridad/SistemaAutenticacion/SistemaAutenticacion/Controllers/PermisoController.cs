using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaAutenticacion.Data.Permiso;
using SistemaAutenticacion.Dtos.PermisosDto;

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
        public async Task<ActionResult<IEnumerable<PermisosResponseDto>>> GetPermisos()
        {
            var permisos = await _permisosRepository.ObtenerPermisos();

            var permisosDto = _mapper.Map<IEnumerable<PermisosResponseDto>>(permisos);

            return Ok(permisosDto);
        }



    }
}
