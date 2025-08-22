using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using proyectoWEBSITESmeall.Services;
using proyectoWEBSITESmeall.Dtos;

namespace proyectoWEBSITESmeall.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhooksController : ControllerBase
    {
        private readonly PagoService _svc;
        public WebhooksController(PagoService svc) { _svc = svc; }

        
        [HttpPost("{origen}")]
        public async Task<IActionResult> Recibir([FromRoute] string origen, [FromQuery] int? idPago)
        {
            
            var secreto = Request.Headers["X-Webhook-Secret"].ToString();
            if (!_svc.ValidarSecretoWebhook(origen, secreto))
                return Unauthorized(new { error = "Invalid webhook secret" });

            
            string payload;
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                payload = await reader.ReadToEndAsync();
            }

            
            var eventType = Request.Headers["X-Webhook-Event"].ToString();
            var providerStatus = Request.Headers["X-Status"].ToString();
            var externalId = Request.Headers["X-External-Id"].ToString();
            var authCode = Request.Headers["X-Auth-Code"].ToString();
            var last4 = Request.Headers["X-Card-Last4"].ToString();

            
            var idWebhook = await _svc.RegistrarWebhookAsync(new WebhookDto
            {
                Origen = origen.ToUpperInvariant(),
                EventType = string.IsNullOrWhiteSpace(eventType) ? "unknown" : eventType,
                Payload = payload,
                IdPago = idPago
            });

            
            if (idPago.HasValue)
            {
                var nuevoEstado = PagoService.InferirEstadoPorEvento(eventType, providerStatus);

                
                if (nuevoEstado != PagoEstado.PENDIENTE)
                {
                    await _svc.ActualizarEstadoPagoAsync(new ActualizarEstadoPagoDto
                    {
                        IdPago = idPago.Value,
                        NuevoEstado = nuevoEstado,
                        ExternalId = string.IsNullOrWhiteSpace(externalId) ? null : externalId,
                        AuthorizationCode = string.IsNullOrWhiteSpace(authCode) ? null : authCode,
                        CardLast4 = string.IsNullOrWhiteSpace(last4) ? null : last4,
                        Observaciones = $"Webhook {origen} #{idWebhook}"
                    });
                }
            }

            return Ok(new { ok = true, origen, idWebhook });
        }
    }
}
