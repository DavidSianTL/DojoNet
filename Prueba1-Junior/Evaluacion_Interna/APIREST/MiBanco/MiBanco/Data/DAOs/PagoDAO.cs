using MiBanco.Data.DTOs;
using MiBanco.Data.DTOs.PagoDTOs;
using MiBanco.Models;


using System.Text.Json;

namespace MiBanco.Data.DAOs
{
    public interface IPagoDAO
    {
        bool AddPagos(Pago pago);
        List<Pago> GetPagos();
    }

    public class PagoDAO : IPagoDAO
    {
        private readonly string _pagoPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Pagos.json" );


        // Método que simula obtener datos de una base de datos
        public List<Pago> GetPagos()
        {
            if (_pagoPath == null) return new List<Pago>();

            var content = File.ReadAllText( _pagoPath );

            var pagos = JsonSerializer.Deserialize<List<Pago>>(content);

            return pagos?? new List<Pago>();
        }

        // Metodo que simula agreagar datos a una base de datos 
        public bool AddPagos(Pago pago)
        {
            var pagos = GetPagos();

            if (pagos == null) return false ;

            pagos.Add(pago);

            var json = JsonSerializer.Serialize(pagos, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_pagoPath, json);

            return true;

        }

    }
}
