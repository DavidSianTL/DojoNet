using AutoMapper;
using SistemaAutenticacion.Dtos.RolesDtos;
using SistemaAutenticacion.Models;

namespace SistemaAutenticacion.Profiles
{
    public class RolUsuarioProfile: Profile
    {
        public RolUsuarioProfile()
        {
            //Modelo Origen y modelo destino
            CreateMap<CustomRolUsuario, RolResponseDto>();
            CreateMap<RolResponseDto, CustomRolUsuario>();
        }
    }
}
