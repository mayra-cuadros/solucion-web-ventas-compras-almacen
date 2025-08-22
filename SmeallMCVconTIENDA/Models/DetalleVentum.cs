using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace proyectoWEBSITESmeall.Models;

public partial class DetalleVentum
{
    public int IdDetalleVenta { get; set; }

    public int IdVenta { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    [ValidateNever]
    public virtual Producto IdProductoNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Ventum IdVentaNavigation { get; set; } = null!;
}
