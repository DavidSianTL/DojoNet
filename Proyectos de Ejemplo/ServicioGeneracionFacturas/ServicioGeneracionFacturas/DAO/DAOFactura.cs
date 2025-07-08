using ServicioGeneracionFacturas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ServicioGeneracionFacturas.DAO
{
    public class DAOFactura
    {

        private readonly string _cadena;

        public DAOFactura(string cadenaConexion) => _cadena = cadenaConexion;

        public List<Factura> ObtenerFacturasPendientes()
        {
            var lista = new List<Factura>();
            using var conn = new SqlConnection(_cadena);
            conn.Open();

            using var cmd = new SqlCommand(
                "SELECT Id, Cliente, Fecha FROM FacturasPendientes WHERE Estado = 'Pendiente'", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                lista.Add(new Factura
                {
                    Id = reader.GetInt32(0),
                    Cliente = reader.GetString(1),
                    Fecha = reader.GetDateTime(2)
                });

            foreach (var f in lista)
            {
                f.Detalles = ObtenerDetalles(conn, f.Id);
            }
            return lista;

        }


        private List<DetalleFactura> ObtenerDetalles(SqlConnection conn, int idFactura)
        {
            using var cmd = new SqlCommand(
                "SELECT Producto, Cantidad, PrecioUnitario FROM DetalleFactura WHERE IdFactura = @id", conn);
            cmd.Parameters.AddWithValue("@id", idFactura);

            var list = new List<DetalleFactura>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(new DetalleFactura
                {
                    Producto = reader.GetString(0),
                    Cantidad = reader.GetInt32(1),
                    PrecioUnitario = reader.GetDecimal(2)
                });
            return list;
        }

        public void MarcarComoFacturada(int id, string rutaPdf)
        {
            using var conn = new SqlConnection(_cadena);
            conn.Open();

            using var cmd = new SqlCommand(
                "UPDATE FacturasPendientes SET Estado = 'Facturada', RutaPDF = @ruta WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@ruta", rutaPdf);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
       
    }
    
}
