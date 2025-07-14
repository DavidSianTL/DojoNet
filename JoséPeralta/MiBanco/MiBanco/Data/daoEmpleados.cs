using MiBanco.Models;

namespace MiBanco.Data
{
    public class daoEmpleados
    {

        // Lista de Empleados para simular la base de datos en memoria
        private List<EmpleadosViewModel> empleados = new List<EmpleadosViewModel>
        {
            new EmpleadosViewModel{
                Id = 1,
                Nombres = "Ana",
                Apellidos = "Martinez",
                DPI = "1234567890123",
                Telefono = "12345678",
                CorreoPersonal = "ana.martinez@gmail.com",
                SucursalId = 1 // Asignar una sucursal por defecto
            },
            new EmpleadosViewModel
            {
                Id = 2,
                Nombres = "Luis",
                Apellidos = "Gonzalez",
                DPI = "9876543210987",
                Telefono = "87654321",
                CorreoPersonal = "luis.gonzales@hotmail.com",
                SucursalId = 1 // Asignar una sucursal por defecto
            }
        };

        // Función (método) que obtiene una lista de los empleados
        public List<EmpleadosViewModel> ObtenerEmpleados()
        {
            return empleados;
        }


    }
}
