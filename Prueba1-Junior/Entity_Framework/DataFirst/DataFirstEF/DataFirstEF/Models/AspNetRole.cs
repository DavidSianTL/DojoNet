using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataFirstEF.Models;

[Index("UsuariosId", Name = "IX_AspNetRoles_UsuariosId")]
public partial class AspNetRole
{
    [Key]
    public string Id { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? UsuariosId { get; set; }

    [StringLength(256)]
    public string? Name { get; set; }

    [StringLength(256)]
    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaim>();

    [ForeignKey("UsuariosId")]
    [InverseProperty("AspNetRoles")]
    public virtual AspNetUser? Usuarios { get; set; }

    [ForeignKey("RolId")]
    [InverseProperty("Rols")]
    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();

    [ForeignKey("RoleId")]
    [InverseProperty("Roles")]
    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
