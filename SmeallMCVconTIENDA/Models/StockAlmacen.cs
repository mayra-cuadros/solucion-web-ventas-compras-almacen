using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace proyectoWEBSITESmeall.Models;

public partial class StockAlmacen
{
    public int IdStock { get; set; }

    public int IdAlmacen { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    [ValidateNever]
    public virtual Almacen IdAlmacenNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
