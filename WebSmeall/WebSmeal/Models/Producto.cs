using System;
using System.Collections.Generic;

namespace WebSmeal.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public string? Descripcion { get; set; }

    public int StockTotal { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetalleGuiaSalidum> DetalleGuiaSalida { get; set; } = new List<DetalleGuiaSalidum>();

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual ICollection<StockAlmacen> StockAlmacens { get; set; } = new List<StockAlmacen>();
}
