using System.ComponentModel.DataAnnotations;

namespace RazorCrudApp.Models
{
    public class User
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
        
        public string Name { get; set; }
    }
}
