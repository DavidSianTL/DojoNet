using AutoExpress_Datos;
using AutoExpress_Entidades;
using AutoExpress_Entidades.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExpress_Negocio
{
    public class CarroService
    {
        private readonly daoCarros _daoCarro;

        public CarroService(daoCarros daoCarro)
        {
            _daoCarro = daoCarro;
        }

        public async Task<List<Carro>> Listar()
        {
            var carros = await _daoCarro.GetCarrosAsync();
            if (carros == null) return new List<Carro>();
            else return carros;
        }

        public async Task<string> Agregar(CarroRequestDTO carro)
        {
            if (carro.Anio < 2000) return "El carro es demaciado antiguo. ";
            if (carro.Precio < 0 ) return "El carro no puede venderse por ese precio. ";
            var respuesta = await _daoCarro.AddCarroAsync(carro);
            if (!respuesta) return "No se pudo insertar el carro, intentelo de nuevo más tarde. ";
            else return "El carro se ha agregado exitosamente. ";
        }

        public async Task<string> Editar(Carro carro)
        {
            var carroValido = await _daoCarro.GetCarroByIdAsync(carro.Id);
            if (carroValido == null) return $"No existe un carro con el id: {carro.Id}, verifique su solicitud. ";

            if (carro.Anio < 2000) return "El carro es demaciado antiguo. ";
            if (carro.Precio < 0 ) return "El carro no puede venderse por ese precio. ";

            var modificado = await _daoCarro.UpdateCarroAsync(carro);
            if (!modificado) return "No se pudo editar el carro, intente de nuevo más tarde. ";
            else return "El carro se ha editado exitosamente. ";
        }

        public async Task<string> Eliminar(int id)
        {
            var carroValido = await _daoCarro.GetCarroByIdAsync(id);
            if (carroValido == null) return $"No existe un carro con el id: {id}, verifique su solicitud. ";

            var eliminado = await _daoCarro.DeleteCarroAsync(id);
            if (!eliminado) return $"No se pudo eliminar el carro con id: {id}, intente de nuevo más tarde. ";
            else return "El carro se ha eliminado exitosamente. ";
        }

    }
}
