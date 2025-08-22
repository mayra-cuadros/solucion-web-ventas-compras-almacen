using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using proyectoWEBSITESmeall.Services;

namespace proyectoWEBSITESmeall.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TiendaController : ControllerBase
    {
        private readonly TiendaService _svc;
        public TiendaController(TiendaService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
            => Ok(await _svc.BuscarAsync(q, page, pageSize));

        [HttpGet("{idProducto:int}")]
        public async Task<IActionResult> GetById([FromRoute] int idProducto)
        {
            var prod = await _svc.ObtenerAsync(idProducto);
            if (prod is null) return NotFound();
            return Ok(prod);
        }
    }
}
