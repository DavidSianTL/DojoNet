using AutoExpress.Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AutoExpress.Datos.DAO
{
    public class VehiculoDAO : Conexion.ConexionBD
    {
        public List<Vehiculo> Listar()
        {
            List<Vehiculo> lista = new List<Vehiculo>();
            try
            {
                Conectar();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Vehiculos", Conexion);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Vehiculo
                    {
                        IdVehiculo = Convert.ToInt32(dr["idVehiculo"]),
                        Marca = dr["marca"].ToString(),
                        Modelo = dr["modelo"].ToString(),
                        Anio = Convert.ToInt32(dr["anio"]),
                        Precio = Convert.ToDecimal(dr["precio"]),
                        Fk_IdTipoVehiculo = Convert.ToInt32(dr["fk_IdTipoVehiculo"]),
                        Fk_IdEstado = Convert.ToInt32(dr["fk_IdEstado"]),
                        Fk_IdPais = Convert.ToInt32(dr["fk_IdPais"])
                    });
                }
                dr.Close();
            }
            finally
            {
                Desconectar();
            }
            return lista;
        }

        public bool Agregar(Vehiculo v)
        {
            try
            {
                Conectar();
                SqlCommand cmd = new SqlCommand("INSERT INTO Vehiculos (marca, modelo, anio, precio, fk_IdTipoVehiculo, fk_IdEstado, fk_IdPais) VALUES (@marca, @modelo, @anio, @precio, @tipo, @estado, @pais)", Conexion);
                cmd.Parameters.AddWithValue("@marca", v.Marca);
                cmd.Parameters.AddWithValue("@modelo", v.Modelo);
                cmd.Parameters.AddWithValue("@anio", v.Anio);
                cmd.Parameters.AddWithValue("@precio", v.Precio);
                cmd.Parameters.AddWithValue("@tipo", v.Fk_IdTipoVehiculo);
                cmd.Parameters.AddWithValue("@estado", v.Fk_IdEstado);
                cmd.Parameters.AddWithValue("@pais", v.Fk_IdPais);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                Desconectar();
            }
        }

        public bool Editar(Vehiculo v)
        {
            try
            {
                Conectar();
                SqlCommand cmd = new SqlCommand("UPDATE Vehiculos SET marca=@marca, modelo=@modelo, anio=@anio, precio=@precio, fk_IdTipoVehiculo=@tipo, fk_IdEstado=@estado, fk_IdPais=@pais WHERE idVehiculo=@id", Conexion);
                cmd.Parameters.AddWithValue("@id", v.IdVehiculo);
                cmd.Parameters.AddWithValue("@marca", v.Marca);
                cmd.Parameters.AddWithValue("@modelo", v.Modelo);
                cmd.Parameters.AddWithValue("@anio", v.Anio);
                cmd.Parameters.AddWithValue("@precio", v.Precio);
                cmd.Parameters.AddWithValue("@tipo", v.Fk_IdTipoVehiculo);
                cmd.Parameters.AddWithValue("@estado", v.Fk_IdEstado);
                cmd.Parameters.AddWithValue("@pais", v.Fk_IdPais);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                Desconectar();
            }
        }

        public bool CambiarEstado(int idVehiculo, int nuevoEstado)
        {
            try
            {
                Conectar();
                SqlCommand cmd = new SqlCommand("UPDATE Vehiculos SET fk_IdEstado = @estado WHERE idVehiculo = @id", Conexion);
                cmd.Parameters.AddWithValue("@id", idVehiculo);
                cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally
            {
                Desconectar();
            }
        }
    }
}
