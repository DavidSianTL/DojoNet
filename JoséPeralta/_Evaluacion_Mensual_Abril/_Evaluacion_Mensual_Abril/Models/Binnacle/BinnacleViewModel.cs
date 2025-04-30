namespace _Evaluacion_Mensual_Abril.Models.Binnacle
{
    public class BinnacleViewModel
    {
        public string FechaHora { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string Descripcion { get; set; }

        // Para agregar nueva entrada manualmente
        public string NuevaAccion { get; set; }
        public string NuevaDescripcion { get; set; }

    }

    public class BinnaclePageViewModel
    {
        public List<BinnacleViewModel> Logs { get; set; }
        public BinnacleViewModel NuevaEntrada { get; set; }

    }

}
