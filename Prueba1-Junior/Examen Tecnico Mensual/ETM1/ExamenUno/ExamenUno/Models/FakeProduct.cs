using System.ComponentModel.DataAnnotations;

namespace ExamenUno.Models
{
    public class FakeProduct
    {
        [Required]
        public int id {  get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public decimal price { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string category { get; set; }
        [Required]
        public string image { get; set; }
    }
}
