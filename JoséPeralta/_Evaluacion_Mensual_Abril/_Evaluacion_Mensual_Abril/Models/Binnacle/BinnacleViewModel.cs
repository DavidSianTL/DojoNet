using System.ComponentModel.DataAnnotations;

namespace _Evaluacion_Mensual_Abril.Models.Binnacle
{
    public class BinnacleViewModel
    {
        public string FechaHora { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string Descripcion { get; set; }

        // Para agregar nueva entrada manualmente
        [Required(ErrorMessage = "La acción es requerida")]
        [StringLength(50, ErrorMessage = "La acción no puede exceder los 50 caracteres")]
        public string NuevaAccion { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres")]
        public string NuevaDescripcion { get; set; }

    }

    public class BinnaclePageViewModel
    {
        public List<BinnacleViewModel> Logs { get; set; }
        public BinnacleViewModel NuevaEntrada { get; set; }

    }

}
