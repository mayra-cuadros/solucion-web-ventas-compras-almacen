using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaCarritoPasarelaSmeall.Models;

namespace TiendaCarritoPasarelaSmeall.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {

        private static readonly List<Producto> productos = new List<Producto>
        {
            new Producto { Id = 1, Nombre = "Mochila Viajera Azul", Precio = 120.00m, ImagenUrl = "../img/mochilaviaje1.jpg", Categoria = "mochilaviaje" },
            new Producto { Id = 2, Nombre = "Bolso para Niños Dino", Precio = 89.50m, ImagenUrl= "../img/bolsonino1.jpg", Categoria = "bolsosniños" },
            new Producto { Id = 3, Nombre = "Bolso de Dama Rosa", Precio = 149.90m, ImagenUrl = "../img/bolsodama1.jpg", Categoria = "bolsosdama" },
            new Producto { Id = 4, Nombre = "Mochila Viajera Negra", Precio = 135.00m, ImagenUrl = "../img/mochilaviaje2.jpg", Categoria = "mochilaviaje" },
            new Producto { Id = 5, Nombre = "Bolso para Niños Superhéroes", Precio = 95.00m, ImagenUrl = "../img/bolsonino2.jpg", Categoria = "bolsosniños" },
            new Producto { Id = 6, Nombre = "Mochila de Viaje Gris", Precio = 110.00m, ImagenUrl = "../img/mochilaviaje3.jpg", Categoria = "mochilaviaje" },
            new Producto { Id = 7, Nombre = "Bolso de Dama Elegante", Precio = 160.00m, ImagenUrl = "../img/bolsodama2.jpg", Categoria = "bolsosdama" },
            new Producto { Id = 8, Nombre = "Bolso para Niños Espacial", Precio = 92.00m, ImagenUrl = "../img/bolsonino3.jpg", Categoria = "bolsosniños" }
        };


        [HttpGet]
        public ActionResult<IEnumerable<Producto>> GetProductos()
        {
            return Ok(productos);
        }


        [HttpGet("{id}")]
        public ActionResult<Producto> GetProducto(int id)
        {
            var producto = productos.Find(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }
    }

}