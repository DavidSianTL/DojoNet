namespace AppFuturista.Models
{
    public class ProductoApiDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public RatingDTO Rating { get; set; }
    }

    public class RatingDTO
    {
        public decimal Rate { get; set; }
        public int Count { get; set; }
    }
}
