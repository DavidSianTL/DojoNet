using AutoMapper;
using SistemaAutenticacion.Dtos.RolesDto;
using SistemaAutenticacion.Models;

namespace SistemaAutenticacion.Profiles
{
    public class RolUsuarioProfile: Profile
    {
        public RolUsuarioProfile()
        {
            //Modelo Origen y modelo destino
            CreateMap<CustomRolUsuarioViewModel, RolResponseDto>();
            CreateMap<RolResponseDto, CustomRolUsuarioViewModel>();
        }
    }
}
