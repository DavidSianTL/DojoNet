using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataFirstEF.Models;

public partial class Permiso
{
    [Key]
    public int Id { get; set; }

    public string? NombrePermiso { get; set; }

    public string? Descripcion { get; set; }

    [ForeignKey("PermisoId")]
    [InverseProperty("Permisos")]
    public virtual ICollection<AspNetRole> Rols { get; set; } = new List<AspNetRole>();
}
