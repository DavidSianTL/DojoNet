using MiBanco.Models;

namespace MiBanco.Data
{
    public class daoSucursales
    {

        // Lista de Sucursales para simular la base de datos en memoria
        private List<SucursalViewModel> sucursales = new List<SucursalViewModel>
        {
            new SucursalViewModel{
                Id = 1,
                Nombre = "Sucursal Central",
                Direccion = "Avenida Principal 123",
                Telefono = "12345678",
                Correo = "sucursalgeneral@gmail.com"
            },
            new SucursalViewModel
            {
                Id = 2,
                Nombre = "Sucursal Norte",
                Direccion = "Avenida Norte 456",
                Telefono = "87654321",
                Correo = "sucursalnorte@gmail.com"
            }
        };

        // Función (método) que obtiene una lista de las sucursales
        public List<SucursalViewModel> ObtenerSucursales()
        {
            return sucursales;
        }

    }
}
