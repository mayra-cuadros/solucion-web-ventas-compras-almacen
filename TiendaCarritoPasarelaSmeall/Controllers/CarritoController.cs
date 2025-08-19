using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaCarritoPasarelaSmeall.Models;

namespace TiendaCarritoPasarelaSmeall.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {

        private static readonly List<CarritoItem> _carrito = new();

        
        [HttpGet]
        public ActionResult<IEnumerable<CarritoItem>> Get() => Ok(_carrito);

        
        [HttpPost]
        public ActionResult<IEnumerable<CarritoItem>> Agregar([FromBody] CarritoItem item)
        {
            if (item == null || item.ProductoId <= 0 || item.Cantidad <= 0)
                return BadRequest("Item inválido.");

            var existente = _carrito.FirstOrDefault(i => i.ProductoId == item.ProductoId);
            if (existente != null)
            {
                existente.Cantidad += item.Cantidad;
            }
            else
            {
                _carrito.Add(item);
            }
            return Ok(_carrito);
        }

        
        [HttpPut("{productoId:int}")]
        public ActionResult<CarritoItem> ActualizarCantidad(int productoId, [FromBody] int cantidad)
        {
            if (cantidad <= 0) return BadRequest("Cantidad debe ser mayor a 0.");

            var item = _carrito.FirstOrDefault(i => i.ProductoId == productoId);
            if (item == null) return NotFound();

            item.Cantidad = cantidad;
            return Ok(item);
        }

        
        [HttpDelete("{productoId:int}")]
        public ActionResult<IEnumerable<CarritoItem>> Eliminar(int productoId)
        {
            var item = _carrito.FirstOrDefault(i => i.ProductoId == productoId);
            if (item == null) return NotFound();

            _carrito.Remove(item);
            return Ok(_carrito);
        }

        
        [HttpDelete]
        public IActionResult Vaciar()
        {
            _carrito.Clear();
            return Ok();
        }
    }

}

