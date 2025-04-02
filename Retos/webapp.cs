using System; 
using System.InitializeComponent.DataAnnotations;

namespace Wbaappcss Models
{
    public class Empleado
    {
        public int Id { get; set;}  

        [Requiered]
        [StringLength(100)]
        public string Nombre { get; set;}
        [Requiered]
        public string Puesto { get}
        [Requiered]
        [Range(0,double.MaxValue, ErrorMessage = "debe de ser numero positivo")]

        [Requiered]
        [DataType (DataType.Date)]

        public DateTime FechaContratacion{ get; set;}
    }
}