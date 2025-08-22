using System;
using System.Collections.Generic;

namespace proyectoWEBSITESmeall.Dtos
{
    public class CrearCarritoDto
    {
        public int? IdUsuario { get; set; }
        public string? SessionId { get; set; }
    }

    public class AddOrUpdateItemDto
    {
        public int IdCarrito { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }

    public class CambiarCantidadDto
    {
        public int IdCarrito { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }

    public class EliminarItemDto
    {
        public int IdCarrito { get; set; }
        public int IdProducto { get; set; }
    }

    public class VaciarCarritoDto
    {
        public int IdCarrito { get; set; }
    }

    public class CarritoItemDto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }

    public class CarritoDto
    {
        public int IdCarrito { get; set; }
        public int? IdUsuario { get; set; }
        public string? SessionId { get; set; }
        public List<CarritoItemDto> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
