using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Models.Usuario
{
    /// <summary>
    /// ViewModel para la vista Index de Usuarios
    /// Contiene todos los datos necesarios para mostrar la lista de usuarios
    /// </summary>
    public class UsuarioIndexViewModel
    {
        /// <summary>
        /// Lista de usuarios a mostrar en la tabla
        /// </summary>
        public List<UsuarioViewModel> Usuarios { get; set; } = new List<UsuarioViewModel>();

        /// <summary>
        /// Lista de roles disponibles para filtrar
        /// </summary>
        public List<RolesViewModel> Roles { get; set; } = new List<RolesViewModel>();

        /// <summary>
        /// Total de usuarios en el sistema (sin filtros)
        /// </summary>
        public int TotalUsuarios { get; set; }

        /// <summary>
        /// Total de usuarios después de aplicar filtros
        /// </summary>
        public int TotalUsuariosFiltrados { get; set; }

        /// <summary>
        /// Página actual para la paginación
        /// </summary>
        public int PaginaActual { get; set; } = 1;

        /// <summary>
        /// Número de registros por página
        /// </summary>
        public int RegistrosPorPagina { get; set; } = 10;

        /// <summary>
        /// Total de páginas disponibles
        /// </summary>
        public int TotalPaginas => (int)Math.Ceiling((double)TotalUsuariosFiltrados / RegistrosPorPagina);

        /// <summary>
        /// Filtro por estado del usuario
        /// </summary>
        public bool? FiltroEstado { get; set; }

        /// <summary>
        /// Filtro por rol del usuario
        /// </summary>
        public int? FiltroRol { get; set; }

        /// <summary>
        /// Término de búsqueda
        /// </summary>
        public string? TerminoBusqueda { get; set; }

        /// <summary>
        /// Campo por el cual ordenar
        /// </summary>
        public string CampoOrden { get; set; } = "IdUsuario";

        /// <summary>
        /// Dirección del ordenamiento (asc/desc)
        /// </summary>
        public string DireccionOrden { get; set; } = "asc";

        /// <summary>
        /// Indica si hay usuarios anteriores (para paginación)
        /// </summary>
        public bool TieneAnterior => PaginaActual > 1;

        /// <summary>
        /// Indica si hay usuarios siguientes (para paginación)
        /// </summary>
        public bool TieneSiguiente => PaginaActual < TotalPaginas;

        /// <summary>
        /// Número de registro inicial en la página actual
        /// </summary>
        public int RegistroInicial => (PaginaActual - 1) * RegistrosPorPagina + 1;

        /// <summary>
        /// Número de registro final en la página actual
        /// </summary>
        public int RegistroFinal => Math.Min(PaginaActual * RegistrosPorPagina, TotalUsuariosFiltrados);

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public UsuarioIndexViewModel()
        {
            Usuarios = new List<UsuarioViewModel>();
            Roles = new List<RolesViewModel>();
        }

        /// <summary>
        /// Constructor con parámetros básicos
        /// </summary>
        /// <param name="usuarios">Lista de usuarios</param>
        /// <param name="roles">Lista de roles</param>
        public UsuarioIndexViewModel(List<UsuarioViewModel> usuarios, List<RolesViewModel> roles)
        {
            Usuarios = usuarios ?? new List<UsuarioViewModel>();
            Roles = roles ?? new List<RolesViewModel>();
            TotalUsuarios = Usuarios.Count;
            TotalUsuariosFiltrados = Usuarios.Count;
        }
    }
}
