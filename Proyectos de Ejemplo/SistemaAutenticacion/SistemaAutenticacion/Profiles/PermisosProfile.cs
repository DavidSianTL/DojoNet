using AutoMapper;
using SistemaAutenticacion.Dtos.PermisosDtos;
using SistemaAutenticacion.Models;

namespace SistemaAutenticacion.Profiles
{
    public class PermisosProfile: Profile
    {
        public PermisosProfile()
        {
            //Modelo Origen y modelo destino
            CreateMap<Permiso, PermisoResponseDto>();
            CreateMap<PermisoResponseDto, Permiso>();
        }
    }
}
