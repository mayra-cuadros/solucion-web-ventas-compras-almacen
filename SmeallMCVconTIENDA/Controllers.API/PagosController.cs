using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using proyectoWEBSITESmeall.Services;
using proyectoWEBSITESmeall.Dtos;

namespace proyectoWEBSITESmeall.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly PagoService _svc;
        public PagosController(PagoService svc) { _svc = svc; }

        
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearPagoDto dto)
        {
            var creado = await _svc.CrearPagoAsync(dto);
            return Ok(creado);
        }

 
        [HttpPost("{idPago:int}/estado")]
        public async Task<IActionResult> Actualizar([FromRoute] int idPago, [FromBody] ActualizarEstadoPagoDto dto)
        {
            dto.IdPago = idPago;
            await _svc.ActualizarEstadoPagoAsync(dto);
            return NoContent();
        }
    }
}
