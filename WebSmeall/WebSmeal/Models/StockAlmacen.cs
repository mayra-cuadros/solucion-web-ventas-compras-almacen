using System;
using System.Collections.Generic;

namespace WebSmeal.Models;

public partial class StockAlmacen
{
    public int IdStock { get; set; }

    public int IdAlmacen { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual Almacen IdAlmacenNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
