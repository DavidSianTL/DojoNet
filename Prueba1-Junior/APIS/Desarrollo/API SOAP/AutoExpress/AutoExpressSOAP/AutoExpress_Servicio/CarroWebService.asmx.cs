using AutoExpress_Datos;
using AutoExpress_Entidades;
using AutoExpress_Entidades.DTOs;
using AutoExpress_Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AutoExpress_Servicio
{
    [WebService(Namespace = "http://autoexpress.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(true)]
    public class CarroWebService
    {
        private readonly CarroService _carroService;

        public CarroWebService()
        {
            var dbService = new DbConnectionService();
            var dao = new daoCarros(dbService);
            _carroService = new CarroService(dao);
        }

        [WebMethod]
        public string PruebaConexion()
        {
            return "Estoy vivo 🚗";
        }

        [WebMethod(Description = "Metodo para listar los carros" )]
        public List<Carro> ListarCarros()
        {
            return _carroService.Listar();
        }

        [WebMethod(Description = "Metodo para agregar un carro")]
        public string AgregarCarro(CarroRequestDTO carro)
        {
            return _carroService.Agregar(carro);
        }

        [WebMethod]
        public string EditarCarro(Carro carro)
        {
            return _carroService.Editar(carro);
        }

        [WebMethod]
        public string EliminarCarro(int id)
        {
            return _carroService.Eliminar(id);
        }
    }

}
