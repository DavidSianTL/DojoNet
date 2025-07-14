using MiBanco.Data.DAOs;
using MiBanco.Data.DTOs;
using MiBanco.Data.DTOs.ClienteDTOs;
using MiBanco.Helpers;
using MiBanco.Models;
using Microsoft.AspNetCore.Mvc;

namespace MiBanco.Services
{
	public interface IClienteService
	{
		Response<ClienteResponseDTO> GetCliente(string DPI);
		Response<bool> AddCliente(ClienteRequestDTO clienteRequest);
		int GenerateId(List<Cliente> clientes);
	}

	public class ClienteService :IClienteService
	{
		private IClienteDAO _clientesDAO;
		private readonly ILogService _logService;
		public ClienteService(IClienteDAO clienteDAO, ILogService logService)
		{
			_logService = logService;
			_clientesDAO = clienteDAO;
		}


		
		public Response<ClienteResponseDTO> GetCliente([FromBody] string DPI)
		{
			if (string.IsNullOrWhiteSpace(DPI)) 
			{ 
				_logService.LogError(400, "El DPI no puede ser nulo o vacío");
				return Response<ClienteResponseDTO>.Malo(400, "El DPI no puede ser nulo o vacío"); // Validar que no sea Vacio o un espacio en blanco
			}

			if (!DPI.All(char.IsDigit)) 
			{ 
				_logService.LogWarning(400, $"Formato invalido, el DPI sólo puede contener números {DPI}");
				return Response<ClienteResponseDTO>.Malo(400, "Formato invalido, el DPI sólo puede contener números"); // Validar qeu no contenga letras o caracteres especiales
			}

			if (DPI.Length != 13)
			{ 
				_logService.LogError(400, "Formato invalido, el DPI sólo puede tener 13 digitos");
                return Response<ClienteResponseDTO>.Malo(400, "Formato invalido, el DPI sólo puede tener 13 digitos"); // Validar qeu la longitud no sea superior o inferior a 13 digitos xd
			}

			var clientes = _clientesDAO.GetClientes();

			var cliente = clientes.FirstOrDefault(cli => cli.DPI == DPI);
			if (cliente == null)
			{
				_logService.LogError(404, $"No se a encontrado un cliente con el DPI: {DPI}");
                return Response<ClienteResponseDTO>.Malo(404, "No se a encontrado un cliente con ese DPI");
			}


			var clienteResponse = ClienteConverter.ConvertClienteToClienteResponseDTO(cliente);
			if (clienteResponse == null) 
			{
				_logService.LogWarning(500, "Hubo un error al convertir el CLiente a clienteResponseDTO");
                return Response<ClienteResponseDTO>.Malo(500, "Hubo un error al cargar el cliente, intentelo más tarde");
			}

			_logService.LogInfo(200, "Cliente encontrado exitosamente");
            return Response<ClienteResponseDTO>.Bueno(200, "Cliente encontrado exitosamente", clienteResponse);
		}


		public Response<bool> AddCliente([FromBody] ClienteRequestDTO clienteRequest)
		{
			if (clienteRequest == null)
			{
				_logService.LogError(400, "El cliente no puede ser nulo. ");
                return Response<bool>.Malo(400, "El cliente no puede ser nulo. ");
			}

			var clientes = _clientesDAO.GetClientes();

			var DPIExiste = clientes.Any(cli => cli.DPI == clienteRequest.DPI);

			if (DPIExiste)
			{
				_logService.LogError(400, "El cliente ingresado ya se encuentra registrado.");
                return Response<bool>.Malo(400, "El cliente ingresado ya se encuentra registrado.");
			}

			Cliente? cliente = ClienteConverter.ConvertClienteRequestDTOToCliente(clienteRequest);

			if (cliente == null)
			{
				_logService.LogWarning(500, "No se pudo convertir el ClienteRequestDTO, a Cliente.");
                return Response<bool>.Malo(500, "Ups, no se pudo registrar el cliente, intentalo más tarde.");
			}

			cliente.Id = GenerateId(clientes); // Generamos un Id nuevo para el cliente

			if (!_clientesDAO.AddCliente(cliente))
			{
				_logService.LogWarning(500, "No se pudo agregar el cliente a la base de datos");
                return Response<bool>.Malo(500, "Ups, no se pudo registrar el cliente, intentalo más tardecito.");
			}

			_logService.LogInfo(200, $"Cliente agregado exitosamente con ID: {cliente.Id} ");
            return Response<bool>.Bueno(200, "Agregado exitosamente", true);
		}


		public int GenerateId( List<Cliente> clientes)
		{
			if (clientes == null || !clientes.Any()) return 1; // Si la lista está vacia se devuelve 1 (para comenzar la secuencia)
		   
			int IdMayor = clientes.Max(obj => obj.Id);

			var id = IdMayor + 1;

			return id;
		}
	}
}
