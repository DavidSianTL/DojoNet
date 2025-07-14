using MiBanco.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiBanco.Data
{
    public class daoClientes
    {

        // Lista de Clientes para simular la base de datos en memoria
        private List<ClientesViewModel> clientes = new List<ClientesViewModel>
        {
            new ClientesViewModel{
                Id = 1,
                Nombres = "Juan",
                Apellidos = "Perez",
                DPI = "1234567890101",
                Telefono = "12345678",
                CorreoPersonal = "juan.perez@gmail.com"
            },
            new ClientesViewModel
            {
                Id = 2,
                Nombres = "Maria",
                Apellidos = "Lopez",
                DPI = "1098765432101",
                Telefono = "87654321",
                CorreoPersonal = "maria.lopez@hotmail.com"
            },
            new ClientesViewModel
            {
                Id = 3,
                Nombres = "Carlos",
                Apellidos = "Gomez",
                DPI = "1122334455667",
                Telefono = "11223344",
                CorreoPersonal = "carlos.gomez@gmail.com"
            }
        };

        // Función (método) que obtiene una lista de los clientes
        public List<ClientesViewModel> ObtenerClientes(){
            return clientes;
        }

        // Función (método) que obtiene un cliente por su DPI
        public ClientesViewModel ObtenerClientePorDPI(string dpi)
        {
            // Buscamos el cliente en la lista por su DPI
            return clientes.FirstOrDefault(c => c.DPI == dpi);
        }

        // Función (método) que agrega un nuevo cliente
        public void AgregarCliente(ClientesViewModel cliente)
        {
            // Asignamos un ID automático al pago
            cliente.Id = clientes.Count > 0 ? clientes.Max(p => p.Id) + 1 : 1;

            // Agregamos el cliente a la lista
            clientes.Add(cliente);
        }


    }
}
