using System;
using System.Collections.Generic;

namespace proyectoWEBSITESmeall.Models;

public partial class DetalleGuiaSalidum
{
    public int IdDetalleGuia { get; set; }

    public int IdGuiaSalida { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual GuiaSalidum IdGuiaSalidaNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
