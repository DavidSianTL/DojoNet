namespace MiBanco.Data.DTOs
{
	public class Response<T>
	{
		public bool Exito { get; set; } = false;
		public int Codigo { get; set; } = 500;
		public string? Mensaje { get; set; } = "Error desconocido.";
		public T? Data { get; set; } 

        public static Response<T> Bueno(int codigo, string mensaje, T data)
			{ return new Response<T> { Exito = true, Codigo = codigo, Mensaje = mensaje, Data = data }; }		

		public static Response<T> Malo(int codigo, string mensaje)
			{ return new Response<T> { Exito = false, Codigo = codigo, Mensaje = mensaje }; }

	}
}
