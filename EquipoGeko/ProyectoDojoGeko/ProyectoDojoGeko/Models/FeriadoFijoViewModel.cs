using System.ComponentModel.DataAnnotations;

namespace ProyectoDojoGeko.Models
{
    public class FeriadoFijoViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El día es obligatorio")]
        [Range(1, 31, ErrorMessage = "El día debe estar entre 1 y 31")]
        public int Dia { get; set; }

        [Required(ErrorMessage = "El mes es obligatorio")]
        [Range(1, 12, ErrorMessage = "El mes debe estar entre 1 y 12")]
        public int Mes { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El tipo de feriado es obligatorio")]
        [Display(Name = "Tipo de Feriado")]
        public int TipoFeriadoId { get; set; }

        [Required(ErrorMessage = "La proporción del día es obligatoria")]
        [Range(0.01, 1.00, ErrorMessage = "La proporción debe estar entre 0.01 y 1.00")]
        [Display(Name = "Proporción del Día")]
        public decimal ProporcionDia { get; set; }

        public int? Original_Dia { get; set; }
        public int? Original_Mes { get; set; }
        public int? Original_TipoFeriadoId { get; set; }

        [Display(Name = "Tipo de Feriado")]
        public string TipoFeriadoNombre { get; set; }

        public string Usr_creacion { get; set; }
        public string Usr_modifica { get; set; }

        public bool IsEditMode => Original_Dia.HasValue;
    }
}
