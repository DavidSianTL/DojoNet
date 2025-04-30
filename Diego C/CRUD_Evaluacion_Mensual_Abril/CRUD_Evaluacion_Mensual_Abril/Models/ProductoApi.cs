using System.ComponentModel.DataAnnotations;

public class ProductoApi
{
    public int id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [Display(Name = "Título")]
    public string title { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    [Display(Name = "Precio")]
    public double price { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [Display(Name = "Descripción")]
    public string description { get; set; }

    [Required(ErrorMessage = "La categoría es obligatoria")]
    [Display(Name = "Categoría")]
    public string category { get; set; }

    [Required(ErrorMessage = "La URL de imagen es obligatoria")]
    [Display(Name = "URL de la Imagen")]
    public string image { get; set; }
}
