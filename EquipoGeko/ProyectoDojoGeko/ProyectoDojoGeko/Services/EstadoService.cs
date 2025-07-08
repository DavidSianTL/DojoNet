using ProyectoDojoGeko.Data;

namespace ProyectoDojoGeko.Services
{
    // Interfaz para el servicio de estados
    public interface IEstadoService
    {
        // Obtenemos los estados activos
        Task<IEnumerable<dynamic>> ObtenerEstadosActivosAsync();
    }

    // Implementación del servicio de estados
    public class EstadoService : IEstadoService
    {
        // Instanciamos el daoEstadoWSAsync
        private readonly daoEstadoWSAsync _daoEstado;

        // Instanciamos la conexión a la base de datos de momento
        string connectionString = "Server=NEWPEGHOSTE\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";

        // Constructor con la conexión a la base de datos
        public EstadoService(daoEstadoWSAsync daoEstado)
        {
            _daoEstado = daoEstado;
        }

        // Obtenemos los estados activos
        // Usamos el IEnumerable<dynamic> para devolver los estados
        // El IEnumerable es un tipo de colección que permite iterar sobre una lista de elementos
        // El dynamic es un tipo de datos que permite devolver cualquier tipo de datos
        public async Task<IEnumerable<dynamic>> ObtenerEstadosActivosAsync()
        {
            var estados = await _daoEstado.ObtenerEstadosAsync();
            return estados
                .Where(e => e.Activo)
                .Select(e => new
                {
                    e.IdEstado,
                    e.Estado,
                    e.Descripcion
                }).ToList();
        }
    }
}
