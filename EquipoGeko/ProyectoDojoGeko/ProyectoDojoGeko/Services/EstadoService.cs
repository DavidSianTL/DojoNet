using ProyectoDojoGeko.Data;

namespace ProyectoDojoGeko.Services
{
    // Interfaz para el servicio de estados
    public interface IEstadoService
    {
        // Obtenemos los estados activos
        Task<IEnumerable<dynamic>> ObtenerEstadosActivosAsync();

        // Obtener los estados activos de las solicitudes
        //Task<IEnumerable<dynamic>> ObtenerEstadosActivosSolicitudesAsync();
    }

    // Implementación del servicio de estados
    public class EstadoService : IEstadoService
    {
        // Instanciamos el daoEstadoWSAsync
        private readonly daoEstadoWSAsync _daoEstado;

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

        /*public async Task<IEnumerable<dynamic>> ObtenerEstadosActivosSolicitudesAsync()
        {
            var estados = await _daoEstado.ObtenerEstadosSolicitudesAsync();
            return estados
                .Where(e => e.Activo && e.IdEstado != 1) // Excluimos el estado "Pendiente"
                .Select(e => new
                {
                    e.IdEstado,
                    e.Estado,
                    e.Descripcion
                }).ToList();
        }*/

    }
}
