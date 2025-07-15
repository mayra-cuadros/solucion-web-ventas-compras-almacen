using System;
using System.Collections.Generic;

namespace WebSmeal.Models;

public partial class Almacen
{
    public int IdAlmacen { get; set; }

    public string Nombre { get; set; } = null!;

    public string Ubicacion { get; set; } = null!;

    public int Capacidad { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<GuiaSalidum> GuiaSalida { get; set; } = new List<GuiaSalidum>();

    public virtual ICollection<StockAlmacen> StockAlmacens { get; set; } = new List<StockAlmacen>();

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
