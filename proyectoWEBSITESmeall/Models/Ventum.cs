using System;
using System.Collections.Generic;

namespace proyectoWEBSITESmeall.Models;

public partial class Ventum
{
    public int IdVenta { get; set; }

    public DateOnly FechaVenta { get; set; }

    public int IdAlmacen { get; set; }

    public int IdUsuario { get; set; }

    public int IdCliente { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual Almacen IdAlmacenNavigation { get; set; } = null!;

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
