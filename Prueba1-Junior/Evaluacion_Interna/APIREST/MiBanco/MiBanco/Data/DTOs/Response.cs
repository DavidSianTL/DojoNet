namespace MiBanco.Data.DTOs
{
	public class Response
	{
		public bool Exito { get; set; } = false;
		public int Codigo { get; set; } = 500;
		public string? Mensaje { get; set; } = "Error desconocido.";

		public static Response Bueno(int codigo, string mensaje)
			{ return new Response { Exito = true, Codigo = codigo, Mensaje = mensaje }; }		

		public static Response Malo(int codigo, string mensaje)
			{ return new Response { Exito = false, Codigo = codigo, Mensaje = mensaje }; }

	}
}
