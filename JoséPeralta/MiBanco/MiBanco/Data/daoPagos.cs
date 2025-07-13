using MiBanco.Models;

namespace MiBanco.Data
{
    public class daoPagos
    {

        // Creamos una lista para almacenar los pagos
        private List<PagosViewModel> pagos = new List<PagosViewModel>();

        // Función (método) que obtiene una lista de los pagos
        public List<PagosViewModel> ObtenerPagos()
        {
            return pagos;
        }

        // Función (método) que agrega un nuevo pago
        public void AgregarPago(PagosViewModel pago)
        {
            // Asignamos un ID automático al pago
            pago.Id = pagos.Count > 0 ? pagos.Max(p => p.Id) + 1 : 1;

            // Agregamos el pago a la lista
            pagos.Add(pago);
        }


    }
}
