namespace Final_Evaluacion_Mensual_Abril.Models
{
    public class ProductoApi
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public Rating Rating { get; set; }
    }

    public class UsuarioApi  
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public NombreApi Name { get; set; }
        public string Phone { get; set; }
        public DireccionApi Address { get; set; }
    }

    public class NombreApi
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }

    public class DireccionApi
    {
        public string City { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string Zipcode { get; set; }
        public GeolocalizacionApi Geolocation { get; set; }
    }

    public class GeolocalizacionApi
    {
        public string Lat { get; set; }
        public string Long { get; set; }
    }

    public class Rating
    {
        public decimal Rate { get; set; }
        public int Count { get; set; }
    }

    public class CarritoApi  
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public List<ProductoCarritoApi> Products { get; set; }
    }

    public class ProductoCarritoApi
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}