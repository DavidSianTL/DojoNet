namespace EjemploConsumirApiRest.Models.DummyJSONModels
{

    public class ProductsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Thumbnail { get; set; }
    }

    public class ProductResponse
    {
        public List<ProductsViewModel> Products { get; set; }
    }


}
