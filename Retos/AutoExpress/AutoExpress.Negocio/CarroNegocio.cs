using System;
using System.Collections.Generic;
using AutoExpress.Datos;
using AutoExpress.Entidades;

namespace AutoExpress.Negocio
{
    public class CarroNegocio
    {
        private CarroDAO carroDAO;

        public CarroNegocio()
        {
            carroDAO = new CarroDAO();
        }

        public RespuestaServicio ListarCarros()
        {
            try
            {
                List<Carro> carros = carroDAO.ListarTodos();
                return new RespuestaServicio(true, "Carros obtenidos exitosamente", carros);
            }
            catch (Exception ex)
            {
                return new RespuestaServicio(false, "Error al obtener los carros: " + ex.Message);
            }
        }

        public RespuestaServicio ObtenerCarro(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new RespuestaServicio(false, "El ID debe ser mayor a 0");
                }

                Carro carro = carroDAO.ObtenerPorId(id);
                if (carro == null)
                {
                    return new RespuestaServicio(false, "No se encontró el carro con el ID especificado");
                }

                return new RespuestaServicio(true, "Carro obtenido exitosamente", carro);
            }
            catch (Exception ex)
            {
                return new RespuestaServicio(false, "Error al obtener el carro: " + ex.Message);
            }
        }

        public RespuestaServicio AgregarCarro(Carro carro)
        {
            try
            {
                // Validaciones de negocio
                string mensajeValidacion = ValidarCarro(carro);
                if (!string.IsNullOrEmpty(mensajeValidacion))
                {
                    return new RespuestaServicio(false, mensajeValidacion);
                }

                int nuevoId = carroDAO.Agregar(carro);
                carro.Id = nuevoId;

                return new RespuestaServicio(true, "Carro agregado exitosamente", carro);
            }
            catch (Exception ex)
            {
                return new RespuestaServicio(false, "Error al agregar el carro: " + ex.Message);
            }
        }

        public RespuestaServicio EditarCarro(Carro carro)
        {
            try
            {
                if (carro.Id <= 0)
                {
                    return new RespuestaServicio(false, "El ID debe ser mayor a 0");
                }

                // Verificar si el carro existe
                if (!carroDAO.Existe(carro.Id))
                {
                    return new RespuestaServicio(false, "No se encontró el carro con el ID especificado");
                }

                // Validaciones de negocio
                string mensajeValidacion = ValidarCarro(carro);
                if (!string.IsNullOrEmpty(mensajeValidacion))
                {
                    return new RespuestaServicio(false, mensajeValidacion);
                }

                bool actualizado = carroDAO.Actualizar(carro);
                if (actualizado)
                {
                    return new RespuestaServicio(true, "Carro actualizado exitosamente", carro);
                }
                else
                {
                    return new RespuestaServicio(false, "No se pudo actualizar el carro");
                }
            }
            catch (Exception ex)
            {
                return new RespuestaServicio(false, "Error al editar el carro: " + ex.Message);
            }
        }

        public RespuestaServicio EliminarCarro(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return new RespuestaServicio(false, "El ID debe ser mayor a 0");
                }

                // Verificar si el carro existe
                if (!carroDAO.Existe(id))
                {
                    return new RespuestaServicio(false, "No se encontró el carro con el ID especificado");
                }

                bool eliminado = carroDAO.Eliminar(id);
                if (eliminado)
                {
                    return new RespuestaServicio(true, "Carro eliminado exitosamente");
                }
                else
                {
                    return new RespuestaServicio(false, "No se pudo eliminar el carro");
                }
            }
            catch (Exception ex)
            {
                return new RespuestaServicio(false, "Error al eliminar el carro: " + ex.Message);
            }
        }

        private string ValidarCarro(Carro carro)
        {
            if (carro == null)
            {
                return "El objeto carro no puede ser nulo";
            }

            if (string.IsNullOrWhiteSpace(carro.Marca))
            {
                return "La marca es requerida";
            }

            if (string.IsNullOrWhiteSpace(carro.Modelo))
            {
                return "El modelo es requerido";
            }

            if (carro.Año <= 2000)
            {
                return "El año debe ser mayor a 2000";
            }

            if (carro.Año > DateTime.Now.Year + 1)
            {
                return "El año no puede ser mayor al año siguiente";
            }

            if (carro.Precio <= 0)
            {
                return "El precio debe ser mayor a 0";
            }

            if (carro.Marca.Length > 100)
            {
                return "La marca no puede exceder 100 caracteres";
            }

            if (carro.Modelo.Length > 100)
            {
                return "El modelo no puede exceder 100 caracteres";
            }

            return string.Empty; // Sin errores
        }
    }
}
