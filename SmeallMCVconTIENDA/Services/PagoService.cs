using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using proyectoWEBSITESmeall.Dtos;
using proyectoWEBSITESmeall.Repositories;

namespace proyectoWEBSITESmeall.Services
{
    public class PagoService
    {
        private readonly PagoRepository _repo;
        private readonly IConfiguration _cfg;

        public PagoService(PagoRepository repo, IConfiguration cfg)
        {
            _repo = repo;
            _cfg = cfg;
        }

        public async Task<PagoCreadoDto> CrearPagoAsync(CrearPagoDto dto)
        {
            // ReturnUrl por defecto si no viene
            dto.ReturnUrl ??= _cfg["Payment:ReturnUrlBase"];

            var id = await _repo.CrearPagoAsync(dto);
            return new PagoCreadoDto
            {
                IdPago = id,
                Metodo = dto.Metodo,
                QrData = dto.QrData,
                QrExpiry = dto.QrExpiry,
                CipCode = dto.CipCode,
                CipExpiry = dto.CipExpiry,
                ReturnUrl = dto.ReturnUrl
            };
        }

        public Task ActualizarEstadoPagoAsync(ActualizarEstadoPagoDto dto)
            => _repo.ActualizarEstadoPagoAsync(dto);

        public async Task<int> RegistrarWebhookAsync(WebhookDto dto)
            => await _repo.RegistrarWebhookAsync(dto);

        public bool ValidarSecretoWebhook(string origen, string? recibido)
        {
            if (string.IsNullOrWhiteSpace(origen)) return false;
            var key = origen.ToUpperInvariant() switch
            {
                "YAPE" => "Payment:WebhookSecret_Yape",
                "PLIN" => "Payment:WebhookSecret_Plin",
                "PAGOEFECTIVO" => "Payment:WebhookSecret_PagoEfectivo",
                _ => null
            };
            if (key is null) return false;
            var esperado = _cfg[key];
            return !string.IsNullOrEmpty(esperado) && recibido == esperado;
        }

        public static string InferirEstadoPorEvento(string eventType, string? statusHeader)
        {
            var e = (eventType ?? "").ToLowerInvariant();
            var s = (statusHeader ?? "").ToUpperInvariant();

            if (e.Contains("approve") || e.Contains("authorized") || s == "APPROVED" || s == "AUTHORIZED")
                return PagoEstado.APROBADO;

            if (e.Contains("reject") || e.Contains("failed") || e.Contains("canceled") || s == "REJECTED" || s == "FAILED")
                return PagoEstado.RECHAZADO;

            return PagoEstado.PENDIENTE;
        }
    }
}
