using System;

namespace ProyectoDojoGeko.Models
{
    public class ModuloSistemaViewModel
    {
        public int IdModuloSistema { get; set; }
        public int FK_IdSistema { get; set; }
        public int FK_IdModulo { get; set; }
        public int FK_IdEstado { get; set; }
    }
}
