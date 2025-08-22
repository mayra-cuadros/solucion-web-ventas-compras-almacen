using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using proyectoWEBSITESmeall.Services;
using proyectoWEBSITESmeall.Dtos;

namespace proyectoWEBSITESmeall.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritoController : ControllerBase
    {
        private readonly CarritoService _svc;
        public CarritoController(CarritoService svc) { _svc = svc; }

        [HttpPost("crear-o-recuperar")]
        public async Task<IActionResult> CrearORecuperar([FromBody] CrearCarritoDto dto)
        {
            var id = await _svc.CrearORecuperarAsync(dto.IdUsuario, dto.SessionId);
            return Ok(new { IdCarrito = id });
        }

        [HttpGet("{idCarrito:int}")]
        public async Task<IActionResult> Obtener([FromRoute] int idCarrito)
        {
            var carrito = await _svc.ObtenerAsync(idCarrito);
            if (carrito is null) return NotFound();
            return Ok(carrito);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AgregarOActualizar([FromBody] AddOrUpdateItemDto dto)
        {
            await _svc.AgregarOActualizarItemAsync(dto.IdCarrito, dto.IdProducto, dto.Cantidad);
            return NoContent();
        }

        [HttpPut("items")]
        public async Task<IActionResult> CambiarCantidad([FromBody] CambiarCantidadDto dto)
        {
            await _svc.CambiarCantidadAsync(dto.IdCarrito, dto.IdProducto, dto.Cantidad);
            return NoContent();
        }

        [HttpDelete("items")]
        public async Task<IActionResult> Eliminar([FromBody] EliminarItemDto dto)
        {
            await _svc.EliminarItemAsync(dto.IdCarrito, dto.IdProducto);
            return NoContent();
        }

        [HttpDelete("{idCarrito:int}/vaciar")]
        public async Task<IActionResult> Vaciar([FromRoute] int idCarrito)
        {
            await _svc.VaciarCarritoAsync(idCarrito);
            return NoContent();
        }
    }
}
