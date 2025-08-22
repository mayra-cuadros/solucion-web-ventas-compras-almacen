using System;

namespace proyectoWEBSITESmeall.Dtos
{
    public class CrearOrdenDto
    {
        public int IdCarrito { get; set; }
        public string? Observaciones { get; set; }
    }

    public class OrdenDto
    {
        public int IdOrden { get; set; }
        public int IdCarrito { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}
