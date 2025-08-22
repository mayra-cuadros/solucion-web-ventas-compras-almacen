using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace proyectoWEBSITESmeall.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    public DateOnly FechaCompra { get; set; }

    public int IdAlmacen { get; set; }

    public int IdUsuario { get; set; }

    public int IdProveedor { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    [ValidateNever]
    public virtual Almacen IdAlmacenNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Proveedore IdProveedorNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
