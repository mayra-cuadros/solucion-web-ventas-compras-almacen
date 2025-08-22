using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace proyectoWEBSITESmeall.Models
{
    public class CheckoutViewModel
    {
        public CarritoVM Carrito { get; set; } = new();

        [Required]
        public string Metodo { get; set; } = "YAPE"; // YAPE | PLIN | PAGOEFECTIVO | TARJETA

        public string? Observaciones { get; set; }

        // Campos opcionales por método (placeholder para UI)
        public string? WalletTelefono { get; set; }
    }

    public class AgregarItemVM
    {
        [Required] public int IdProducto { get; set; }
        [Required][Range(1, int.MaxValue)] public int Cantidad { get; set; } = 1;
    }
}
