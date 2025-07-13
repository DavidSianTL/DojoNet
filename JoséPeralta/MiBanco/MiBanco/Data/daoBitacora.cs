using MiBanco.Models;

namespace MiBanco.Data
{
    public class daoBitacora
    {

        // Creamos una lista para almacenar las bitácoras
        private List<BitacoraViewModel> bitacoras = new List<BitacoraViewModel>();

        // Función (método) que obtiene una lista de las bitácoras
        public List<BitacoraViewModel> ObtenerBitacoras()
        {
            return bitacoras;
        }

        // Función (método) que agrega una nueva bitácora
        public void AgregarBitacora(BitacoraViewModel bitacora)
        {
            // Validamos que la bitácora no sea nula
            if (bitacora == null)
            {
                throw new ArgumentNullException(nameof(bitacora), "La bitácora no puede ser nula.");
            }

            // Asignamos un ID automático a la bitácora
            bitacora.Id = bitacoras.Count > 0 ? bitacoras.Max(b => b.Id) + 1 : 1;

            // Agregamos la bitácora a la lista
            bitacoras.Add(bitacora);
        }


    }
}
