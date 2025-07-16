using System;
using System.Collections.Generic;

namespace proyectoWEBSITESmeall.Models;

public partial class DetalleCompra
{
    public int IdDetalleCompra { get; set; }

    public int IdCompra { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual Compra IdCompraNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    //public class PedidoRealizado
    //{
    //    public int Id { get; set; }
    //    public string CodigoProducto { get; set; }
    //    public DateTime FechaPedido { get; set; }
    //    public string MarcaProducto { get; set; }
    //    public string Categoria { get; set; }
    //    public string NombreProducto { get; set; }
    //    public int Cantidad { get; set; }
    //    public decimal PrecioUnitario { get; set; }
    //    public decimal PrecioTotal { get; set; }
    //}

}
