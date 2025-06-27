using System.Collections.Generic;
using System.ServiceModel;
using UsuariosAPISOAP.Models;


namespace UsuariosAPISOAP.Interfaces.v3
{
    [ServiceContract]
    public interface IUsuarioServiceEF
    {
        [OperationContract]
        List<UsuarioDTO> ObtenerUsuarios();

        [OperationContract]
        UsuarioDTO ObtenerUsuarioPorId(int id);

        [OperationContract]
        bool CrearUsuario(UsuarioEF usuario);

        [OperationContract]
        bool ActualizarUsuario(UsuarioEF usuario);

        [OperationContract]
        bool EliminarUsuario(int id);
    }
}
