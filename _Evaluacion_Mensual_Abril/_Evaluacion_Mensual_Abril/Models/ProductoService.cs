using Newtonsoft.Json;

namespace Evaluacion_Mensual_Abril.Models
{
    public class ProductoService
    {
        private readonly string _rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "productos.json");

        public List<ProductoViewModel> ObtenerProductos()
        {
            if (!File.Exists(_rutaArchivo)) return new List<ProductoViewModel>();

            var json = File.ReadAllText(_rutaArchivo);
            return JsonConvert.DeserializeObject<List<ProductoViewModel>>(json) ?? new List<ProductoViewModel>();
        }

        public void GuardarProductos(List<ProductoViewModel> productos)
        {
            var json = JsonConvert.SerializeObject(productos, Formatting.Indented);
            File.WriteAllText(_rutaArchivo, json);
        }
    }
}

