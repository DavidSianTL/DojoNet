using Newtonsoft.Json;
using Productos.Models;
public class ProductoService
{
    private readonly string _rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "productos.json");

    public List<ProductoModel> ObtenerProductos()
    {
        if (File.Exists(_rutaArchivo))
        {
            var json = System.IO.File.ReadAllText(_rutaArchivo);
            var productos = JsonConvert.DeserializeObject<List<ProductoModel>>(json);
            return productos ?? new List<ProductoModel>();
        }
        return new List<ProductoModel>();
    }

    public void GuardarProducto(ProductoModel producto)
    {
        var productos = ObtenerProductos();
        productos.Add(producto);

        var jsonData = System.Text.Json.JsonSerializer.Serialize(productos);
        File.WriteAllText(_rutaArchivo, jsonData);
    }

    public void EliminarProducto(int index)
    {
        var productos = ObtenerProductos();
        if (index >= 0 && index < productos.Count)
        {
            productos.RemoveAt(index);
            var jsonData = System.Text.Json.JsonSerializer.Serialize(productos);
            File.WriteAllText(_rutaArchivo, jsonData);
        }
    }

    public void ActualizarProducto(int index, ProductoModel producto)
    {
        var productos = ObtenerProductos();
        if (index >= 0 && index < productos.Count)
        {
            productos[index] = producto;
            var jsonData = System.Text.Json.JsonSerializer.Serialize(productos);
            File.WriteAllText(_rutaArchivo, jsonData);
        }
    }
}
