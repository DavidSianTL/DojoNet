using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaAutenticacion.Dtos.RolesDtos;
using SistemaAutenticacion.Models;
using SistemaAutenticacion.Token;

namespace SistemaAutenticacion.Data.Roles
{
    public interface IRolUsuarioRepository
    {
        Task<RolResponseDto> CreateRol(RolRegistroRequestDto rolRegistroRequestDto);
        Task<IdentityResult> DeleteRol(string id);
        Task<RolResponseDto> EditarRol(string id, RolRegistroRequestDto rolRegistroRequestDto);
        Task<CustomRolUsuario> GetRolesById(string id);
        Task<List<CustomRolUsuario>> ObtenerRoles();
        Task<bool> SaveChanges();
    }

    public class RolUsuarioRepository: IRolUsuarioRepository
    {
        private readonly UserManager<Usuarios> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly IUsuarioSesion _usuarioSesion;
        private readonly RoleManager<CustomRolUsuario> _roleManager;

        public RolUsuarioRepository(UserManager<Usuarios> userManager, AppDbContext appDbContext, IUsuarioSesion usuarioSesion, RoleManager<CustomRolUsuario> roleManager)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _usuarioSesion = usuarioSesion;
            _roleManager = roleManager;
        }

        private RolResponseDto TransformerRolUserToRolUserDto(CustomRolUsuario customRolUsuario)
        {
            return new RolResponseDto
            {
                Nombre = customRolUsuario.Name,
                Descripcion = customRolUsuario.Descripcion
            };
        }

        public async Task<List<CustomRolUsuario>> ObtenerRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            if (!roles.Any())
            {
                throw new Exception("No existen roles creados");
            }

            return roles;
        }

        public async Task<CustomRolUsuario> GetRolesById(string id)
        {
            var rol = await _roleManager.FindByIdAsync(id);

            if (rol is null)
            {
                throw new Exception($"No se encintro ningun rol");
            }

            return rol;
        }

        public async Task<RolResponseDto> CreateRol(RolRegistroRequestDto rolRegistroRequestDto)
        {
            var rolExiste = await _roleManager.Roles.Where(x => x.Name == rolRegistroRequestDto.Nombre!).AnyAsync();

            if (rolExiste)
            {
                throw new Exception($"Ya se encuentra registrado un rol con el mismo nombre");
            }

            var RolUsuarioi = new CustomRolUsuario
            {
                Name = rolRegistroRequestDto.Nombre,
                Descripcion = rolRegistroRequestDto.Descripcion,
                FechaCreacion = rolRegistroRequestDto.FechaCreacion = DateTime.UtcNow
            };

            var resultado = await _roleManager.CreateAsync(RolUsuarioi);

            if (resultado.Succeeded)
            {
                return TransformerRolUserToRolUserDto(RolUsuarioi!);
            }

            throw new Exception("Ocurrio un error durante la operacion, intentalo nuevamente");
        }


        public async Task<RolResponseDto> EditarRol(string id, RolRegistroRequestDto rolRegistroRequestDto)
        {
            var rolExiste = await _roleManager.FindByIdAsync(id);

            if (rolExiste is null)
            {
                throw new Exception("No se encotro el rol");
            }

            var nombreExiste = await _appDbContext.Roles.AnyAsync(x => x.Name == rolRegistroRequestDto.Nombre && x.Id != id);

            if (nombreExiste)
            {
                throw new Exception($"Ya existe un rol con el nombre");
            }

            rolExiste.Name = rolRegistroRequestDto.Nombre;
            rolExiste.Descripcion = rolRegistroRequestDto.Descripcion;

            var resultado = await _roleManager.UpdateAsync(rolExiste);

            if (resultado.Succeeded)
            {
                return TransformerRolUserToRolUserDto(rolExiste);
            }

            throw new Exception("Ocurrio un error durante la operacion, intentalo nuevamente");
        }

        public async Task<IdentityResult> DeleteRol(string id)
        {
            var rol = await _roleManager.FindByIdAsync(id);

            if (rol is null)
            {
                throw new Exception("No se encotro el rol");
            }

            return await _roleManager.DeleteAsync(rol);
        }

        public async Task<bool> SaveChanges()
        {
            return (await _appDbContext.SaveChangesAsync() > 0);
        }

    }
}
