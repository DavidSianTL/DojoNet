using System.ComponentModel.DataAnnotations;

namespace APIRESTUniversidades.Models
{
    public class University
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string[] web_pages { get; set; }
        [Required]
        public string[] domains { get; set; }
    }
}
