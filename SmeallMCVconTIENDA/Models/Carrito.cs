using System.Collections.Generic;

namespace proyectoWEBSITESmeall.Models
{
    public class CarritoItemVM
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }

    public class CarritoVM
    {
        public int IdCarrito { get; set; }
        public List<CarritoItemVM> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
