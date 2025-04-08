using System.ComponentModel.DataAnnotations;

namespace FormularioJS.Models
{
    public class PedidoModel
    {

        [Required(ErrorMessage = "Ingrese al menos un producto.")]
        [RegularExpression("^(a-zA-Z).+$", ErrorMessage = "El nombre debe de contener solo letras")]
        public string Producto1 { get; set; }
        [RegularExpression("^(a-zA-Z).+$", ErrorMessage = "El nombre debe de contener solo letras")]
        public string Producto2 { get; set; }
        [RegularExpression("^(a-zA-Z).+$", ErrorMessage = "El nombre debe de contener solo letras")]
        public string Producto3 { get; set; }
        [RegularExpression("^(a-zA-Z).+$", ErrorMessage = "El nombre debe de contener solo letras")]
        public string Producto4 { get; set; }
        [RegularExpression("^(a-zA-Z).+$", ErrorMessage = "El nombre debe de contener solo letras")]
        public string Producto5 { get; set; }
        [RegularExpression("^(a-zA-Z).+$", ErrorMessage = "El nombre debe de contener solo letras")]
        public string Producto6 { get; set; }
        [RegularExpression("^(a-zA-Z).+$", ErrorMessage = "El nombre debe de contener solo letras")]
        public string Producto7 { get; set; }
        [RegularExpression("^(a-zA-Z).+$", ErrorMessage = "El nombre debe de contener solo letras")]
        public string Producto8 { get; set; }
        [RegularExpression("^(a-zA-Z).+$", ErrorMessage = "El nombre debe de contener solo letras")]
        public string Producto9 { get; set; }
        [RegularExpression("^(a-zA-Z).+$", ErrorMessage = "El nombre debe de contener solo letras")]
        public string Producto10 { get; set; }
        //campos personales

        [Required(ErrorMessage = "Campo obligatorio")]
        public string Envio { get; set; }


        [Required(ErrorMessage = "Campo obligatorio")]
        [MinLength(60, ErrorMessage = "La dirección debe de tener al menos 60 caracteres.")]
        public string Direccion { get; set; }



        [Required(ErrorMessage = "Ingrese su nombre.")]
        [RegularExpression("^(a-zA-Z).+$", ErrorMessage = "El nombre debe de contener solo letras")]
        public string Nombre { get; set; }



        [Required(ErrorMessage = "Ingrese su apellido.")]
        [RegularExpression("^(a-zA-Z).+$", ErrorMessage = "El Apellido debe de contener solo letras")]
        public string Apellido { get; set; }



        [Required(ErrorMessage = "Ingrese su telefono.")]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "El telefono debe de contener solo 8 digitos")]
        public string Telefono { get; set; }


        [Required(ErrorMessage = "Ingrese su email.")]
        [EmailAddress(ErrorMessage = "El email no es valido")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Ingrese su contraseña.")]
        [MinLength(6, ErrorMessage = "La contraseña debe de tener al menos 6 caracteres.")]

        public string Password { get; set; }
    }
}
