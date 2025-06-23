using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models.DepartamentosEmpresa
{
    [Table("DepartamentosEmpresas")]
    public class DepartamentoEmpresaViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDepartamentoEmpresa { get; set; }

        [Required]
        public int FK_IdDepartamento { get; set; }

        [Required]
        public int FK_IdEmpresa { get; set; }

        [NotMapped]
        public string? NombreDepartamento { get; set; }
        
        [NotMapped]
        public string? NombreEmpresa { get; set; }

        // Relaciones de navegación (opcional)
        public DepartamentoViewModel? Departamento { get; set; }
        public EmpresaViewModel? Empresa { get; set; }
    }
}
