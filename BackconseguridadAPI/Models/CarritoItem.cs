﻿namespace TiendaCarritoPasarelaSmeall.Models
{
    public class CarritoItem
    {

        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}
