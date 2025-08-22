using System;

namespace proyectoWEBSITESmeall.Dtos
{
    public static class PagoMetodo
    {
        public const string TARJETA = "TARJETA";
        public const string YAPE = "YAPE";
        public const string PLIN = "PLIN";
        public const string PAGOEFECTIVO = "PAGOEFECTIVO";
    }

    public static class PagoEstado
    {
        public const string APROBADO = "APROBADO";
        public const string RECHAZADO = "RECHAZADO";
        public const string PENDIENTE = "PENDIENTE";
    }

    public class CrearPagoDto
    {
        public int IdOrden { get; set; }
        public string Metodo { get; set; } = PagoMetodo.TARJETA;
        public decimal Monto { get; set; }
        public string Moneda { get; set; } = "PEN";

        // TARJETA
        public string? TarjetaMarca { get; set; }
        public string? AuthorizationCode { get; set; }
        public string? CardLast4 { get; set; }

        // Wallets (Yape/Plin)
        public string? WalletTelefono { get; set; }
        public string? WalletRef { get; set; }
        public string? QrData { get; set; }
        public DateTime? QrExpiry { get; set; }

        // PagoEfectivo
        public string? CipCode { get; set; }
        public DateTime? CipExpiry { get; set; }

        // Común
        public string? ExternalId { get; set; }
        public string? ReturnUrl { get; set; }
        public string? Observaciones { get; set; }
    }

    public class PagoCreadoDto
    {
        public int IdPago { get; set; }
        public string? Metodo { get; set; }
        public string? QrData { get; set; }
        public DateTime? QrExpiry { get; set; }
        public string? CipCode { get; set; }
        public DateTime? CipExpiry { get; set; }
        public string? ReturnUrl { get; set; }
    }

    public class ActualizarEstadoPagoDto
    {
        public int IdPago { get; set; }
        public string NuevoEstado { get; set; } = PagoEstado.PENDIENTE;
        public string? AuthorizationCode { get; set; }
        public string? ExternalId { get; set; }
        public string? CardLast4 { get; set; }
        public string? Observaciones { get; set; }
    }

    public class WebhookDto
    {
        public string Origen { get; set; } = string.Empty; // TARJETA/YAPE/PLIN/PAGOEFECTIVO
        public string EventType { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty; // JSON raw
        public int? IdPago { get; set; }
    }
}
