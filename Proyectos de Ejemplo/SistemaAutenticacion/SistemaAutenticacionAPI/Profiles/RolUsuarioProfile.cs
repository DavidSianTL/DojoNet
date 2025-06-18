using AutoMapper;
using SistemaAutenticacionAPI.Dtos.RolesDtos;
using SistemaAutenticacionAPI.Models;

namespace SistemaAutenticacionAPI.Profiles
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
