using AutoMapper;
using SistemaAutenticacionAPI.Dtos.PermisosDtos;
using SistemaAutenticacionAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaAutenticacionAPI.Profiles
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
