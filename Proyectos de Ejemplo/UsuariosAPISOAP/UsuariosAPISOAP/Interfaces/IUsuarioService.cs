using System.Collections.Generic;
using System.ServiceModel;
using UsuariosAPISOAP.Models;


namespace UsuariosAPISOAP.Interfaces
{
    [ServiceContract]
    public interface IUsuarioService
    {
        [OperationContract]
        List<Usuario> ObtenerUsuarios();

        [OperationContract]
        Usuario ObtenerUsuarioPorId(int id);

        [OperationContract]
        bool CrearUsuario(Usuario usuario);

        [OperationContract]
        bool ActualizarUsuario(Usuario usuario);

        [OperationContract]
        bool EliminarUsuario(int id);
    }
}
