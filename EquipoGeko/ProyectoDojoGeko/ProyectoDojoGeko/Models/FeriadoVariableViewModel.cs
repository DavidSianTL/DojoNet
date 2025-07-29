using System.ComponentModel.DataAnnotations;

namespace ProyectoDojoGeko.Models
{
    public class FeriadoVariableViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El tipo de feriado es obligatorio")]
        [Display(Name = "Tipo de Feriado")]
        public int TipoFeriadoId { get; set; }

        [Required(ErrorMessage = "La proporción del día es obligatoria")]
        [Range(0.01, 1.00, ErrorMessage = "La proporción debe estar entre 0.01 y 1.00")]
        [Display(Name = "Proporción del Día")]
        public decimal ProporcionDia { get; set; }

        // Propiedad para mostrar el nombre del tipo de feriado
        [Display(Name = "Tipo de Feriado")]
        public string TipoFeriadoNombre { get; set; }

        public string Usr_creacion { get; set; }
        public string Usr_modifica { get; set; }

        public bool IsEditMode => Id > 0;
    }
}
