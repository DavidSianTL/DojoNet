using System.Threading.Tasks;
using UsuariosApi.Models;


namespace UsuariosApi.DAO
{
    public interface IAuditoriaDAO
    {
        Task InsertarAuditoriaAsync(Auditoria auditoria);
    }
}
