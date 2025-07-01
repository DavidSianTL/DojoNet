using AutoExpress.Datos;
using System;
using AutoExpress.Entidades;
using System.Collections.Generic;

namespace AutoExpress.Datos
{
    public class CarroService
    {
        private readonly CarroDAO carroDAO;

        public CarroService(CarroDAO carroDAO)
        {
            this.carroDAO = carroDAO;
        }

        public List<Carro> ObtenerCarros()
        {
            return carroDAO.ListarCarros();
        }
        public void AgregarCarro(Carro carro)
        {
            if (carro.Año <= 2000)
                throw new ArgumentException("El año debe ser mayor a 2000.");

            if (carro.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0.");

            carroDAO.AgregarCarro(carro);
        }

        public void EditarCarro(Carro carro)
        {
            if (carro.Id <= 0)
                throw new ArgumentException("Id inválido.");

            if (carro.Año <= 2000)
                throw new ArgumentException("El año debe ser mayor a 2000.");

            if (carro.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0.");

            carroDAO.EditarCarro(carro);
        }

        public void EliminarCarro(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");

            carroDAO.EliminarCarro(id);
        }


    }
}