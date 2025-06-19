using AutoMapper;
using SistemaAutenticacion.Dtos.PermisosDto;
using SistemaAutenticacion.Models;

namespace SistemaAutenticacion.Profiles
{
    public class PermisosProfile: Profile
    {
        public PermisosProfile()
        {
            //Modelo Origen y modelo destino
            CreateMap<PermisosViewModel, PermisosResponseDto>();
            CreateMap<PermisosResponseDto, PermisosViewModel>();
        }
    }
}
