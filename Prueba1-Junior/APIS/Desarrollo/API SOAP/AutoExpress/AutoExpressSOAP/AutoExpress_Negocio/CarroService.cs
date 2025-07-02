using AutoExpress_Datos;
using AutoExpress_Entidades;
using AutoExpress_Entidades.DTOs;
using System.Collections.Generic;


namespace AutoExpress_Negocio
{
    public class CarroService
    {
        private readonly daoCarros _daoCarro;

        public CarroService(daoCarros daoCarro)
        {
            _daoCarro = daoCarro;
        }

        public List<Carro> Listar()
        {
            var carros = _daoCarro.GetCarros();
            if (carros == null) return new List<Carro>();
            else return carros;
        }

        public string Agregar(CarroRequestDTO carro)
        {
            if (carro.Anio < 2000) return "El carro es demaciado antiguo. ";
            if (carro.Precio < 0 ) return "El carro no puede venderse por ese precio. ";
            var respuesta = _daoCarro.AddCarro(carro);
            if (!respuesta) return "No se pudo insertar el carro, intentelo de nuevo más tarde. ";
            else return "El carro se ha agregado exitosamente. ";
        }

        public string Editar(Carro carro)
        {
            var carroValido = _daoCarro.GetCarroById(carro.Id);
            if (carroValido == null) return $"No existe un carro con el id: {carro.Id}, verifique su solicitud. ";

            if (carro.Anio < 2000) return "El carro es demaciado antiguo. ";
            if (carro.Precio < 0 ) return "El carro no puede venderse por ese precio. ";

            var modificado = _daoCarro.UpdateCarro(carro);
            if (!modificado) return "No se pudo editar el carro, intente de nuevo más tarde. ";
            else return "El carro se ha editado exitosamente. ";
        }

        public string Eliminar(int id)
        {
            var carroValido = _daoCarro.GetCarroById(id);
            if (carroValido == null) return $"No existe un carro con el id: {id}, verifique su solicitud. ";

            var eliminado = _daoCarro.DeleteCarro(id);
            if (!eliminado) return $"No se pudo eliminar el carro con id: {id}, intente de nuevo más tarde. ";
            else return "El carro se ha eliminado exitosamente. ";
        }

    }
}
