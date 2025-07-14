using MiBanco.Models;
using System.Text.Json;


namespace MiBanco.Data.DAOs
{
	public interface IClienteDAO
	{
		List<Cliente> GetClientes();
		bool AddCliente(Cliente cliente);

	}


public class ClienteDAO : IClienteDAO
    {
		private string _ClientesPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Clientes.json");

		// Metodo que simula obtener datos de una base de datos
		public List<Cliente> GetClientes()
		{
			if (!File.Exists(_ClientesPath)) return new List<Cliente>();

			var content = File.ReadAllText(_ClientesPath);

			var clientes = JsonSerializer.Deserialize<List<Cliente>>(content);

			return clientes ?? new List<Cliente>();
		}

		// Metodo que simula agregar datos a una base de datos
		public bool AddCliente(Cliente cliente)
		{
			var clientes = GetClientes();
			clientes.Add(cliente);

			var content = JsonSerializer.Serialize(clientes, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(_ClientesPath, content);

			return true;
		}

	}

}