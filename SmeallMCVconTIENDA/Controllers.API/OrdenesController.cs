using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using proyectoWEBSITESmeall.Services;
using proyectoWEBSITESmeall.Dtos;

namespace proyectoWEBSITESmeall.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenesController : ControllerBase
    {
        private readonly OrdenService _svc;
        public OrdenesController(OrdenService svc) { _svc = svc; }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearOrdenDto dto)
        {
            var id = await _svc.CrearDesdeCarritoAsync(dto.IdCarrito, dto.Observaciones);
            return Ok(new { IdOrden = id });
        }

        [HttpGet("{idOrden:int}")]
        public async Task<IActionResult> Obtener([FromRoute] int idOrden)
        {
            var ord = await _svc.ObtenerAsync(idOrden);
            if (ord is null) return NotFound();
            return Ok(ord);
        }
    }
}
