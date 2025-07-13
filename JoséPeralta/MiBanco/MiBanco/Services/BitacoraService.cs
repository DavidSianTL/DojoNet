using MiBanco.Data;
using MiBanco.Models;

namespace MiBanco.Services
{
    // Definimos la interfaz IBitacoraService
    // Esta interfaz define los métodos que el servicio de bitacora debe implementar
    public interface IBitacoraService
    {
        // Método para registrar una acción en la bitácora
        void RegistrarAccion(string accion, string descripcion);

    }

    // Heredamos de la interfaz IBitacoraService
    public class BitacoraService : IBitacoraService
    {

        // Creamos una instancia del daoBitacoras para interactuar con la base de datos
        private readonly daoBitacora _daoBitacoras;

        // Constructor que recibe el daoBitacoras para inyección de dependencias
        public BitacoraService(daoBitacora daoBitacoras)
        {
            _daoBitacoras = daoBitacoras;
        }

        // Método que registra una acción en la bitácora
        public void RegistrarAccion(string accion, string descripcion)
        {
            // Pasamos los parámetros al método RegistrarAccion del daoBitacoras
            var bitacora = new BitacoraViewModel
            {
                FechaBitacora = DateTime.UtcNow,
                Accion = accion,
                Descripcion = descripcion
            };

            // Agregamos la bitácora usando el daoBitacoras
            _daoBitacoras.AgregarBitacora(bitacora);

        }


    }
}
