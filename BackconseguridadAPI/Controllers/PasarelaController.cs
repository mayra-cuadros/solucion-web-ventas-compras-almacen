using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaCarritoPasarelaSmeall.Models;

namespace TiendaCarritoPasarelaSmeall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasarelaController : ControllerBase
    {

        [HttpPost("confirmar")]
        public ActionResult<Pedido> Confirmar([FromBody] Pedido pedido)
        {
            if (pedido == null || pedido.Items == null || pedido.Items.Count == 0)
                return BadRequest("El pedido no contiene ítems.");

            if (string.IsNullOrWhiteSpace(pedido.MetodoPago))
                return BadRequest("Debe indicar un método de pago.");

            
            pedido.MontoTotal = pedido.Items.Sum(i => i.Subtotal);

            
            pedido.Numero = Random.Shared.Next(100000, 999999);
            pedido.Fecha = DateTime.Now;

            
            var automatico = pedido.MetodoPago.Equals("tarjeta", StringComparison.OrdinalIgnoreCase)
                             || pedido.MetodoPago.Equals("pagoefectivo", StringComparison.OrdinalIgnoreCase);

            pedido.Estado = automatico ? "Confirmado" : "Pendiente de verificación";

            

            return Ok(pedido);
        }
    }
}
