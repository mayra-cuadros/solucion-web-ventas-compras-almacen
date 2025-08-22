using System;

namespace proyectoWEBSITESmeall.Models
{
    public class Resultado
    {
        public int IdOrden { get; set; }
        public int IdPago { get; set; }
        public string Metodo { get; set; } = string.Empty;
        public string? QrData { get; set; }
        public DateTime? QrExpiry { get; set; }
        public string? CipCode { get; set; }
        public DateTime? CipExpiry { get; set; }
        public string? ReturnUrl { get; set; }
        public string Mensaje { get; set; } = "Pago registrado. Sigue las instrucciones.";
    }
}