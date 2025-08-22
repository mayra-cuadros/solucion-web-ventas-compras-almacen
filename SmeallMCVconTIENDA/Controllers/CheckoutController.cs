using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using proyectoWEBSITESmeall.Services;
using proyectoWEBSITESmeall.Dtos;
using proyectoWEBSITESmeall.ViewModels;
using proyectoWEBSITESmeall.Models;

namespace proyectoWEBSITESmeall.Controllers
{
    public class CheckoutController : Controller
    {
        private const string SidCookie = "smeall.sid";

        private readonly CarritoService _carrito;
        private readonly OrdenService _orden;
        private readonly PagoService _pago;

        public CheckoutController(CarritoService carrito, OrdenService orden, PagoService pago)
        {
            _carrito = carrito;
            _orden = orden;
            _pago = pago;
        }

        private string GetOrCreateSid()
        {
            if (!Request.Cookies.TryGetValue(SidCookie, out var sid) || string.IsNullOrWhiteSpace(sid))
            {
                sid = Guid.NewGuid().ToString("N");
                Response.Cookies.Append(SidCookie, sid, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Secure = Request.IsHttps,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });
            }
            return sid;
        }

        private async Task<int> EnsureCarritoAsync()
        {
            var sid = GetOrCreateSid();
            return await _carrito.CrearORecuperarAsync(null, sid);
        }

        private static CarritoVM MapToVM(proyectoWEBSITESmeall.Dtos.CarritoDto dto)
            => new CarritoVM
            {
                IdCarrito = dto.IdCarrito,
                Items = dto.Items.Select(x => new CarritoItemVM
                {
                    IdProducto = x.IdProducto,
                    NombreProducto = x.NombreProducto,
                    Cantidad = x.Cantidad,
                    PrecioUnitario = x.PrecioUnitario
                }).ToList(),
                Total = dto.Total
            };

        // GET: /Checkout
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var idCarrito = await EnsureCarritoAsync();
            var carrito = await _carrito.ObtenerAsync(idCarrito) ?? new proyectoWEBSITESmeall.Dtos.CarritoDto { IdCarrito = idCarrito };
            var vm = new CheckoutViewModel { Carrito = MapToVM(carrito) };
            return View(vm);
        }

        // POST: /Checkout/Agregar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(AgregarItemVM model)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            var idCarrito = await EnsureCarritoAsync();
            await _carrito.AgregarOActualizarItemAsync(idCarrito, model.IdProducto, model.Cantidad);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Checkout/Pagar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pagar(CheckoutViewModel model)
        {
            var idCarrito = await EnsureCarritoAsync();
            var ordId = await _orden.CrearDesdeCarritoAsync(idCarrito, model.Observaciones);

            // Construye el DTO para crear el pago
            var crearPago = new CrearPagoDto
            {
                IdOrden = ordId,
                Metodo = model.Metodo,        // "YAPE","PLIN","PAGOEFECTIVO","TARJETA"
                Monto = (await _carrito.ObtenerAsync(idCarrito))?.Total ?? 0m,
                Moneda = "PEN",
                WalletTelefono = model.WalletTelefono
                // Si tu SP genera Qr/CIP, basta con mandar ReturnUrl (se toma de appsettings si es null)
            };

            var creado = await _pago.CrearPagoAsync(crearPago);

            var vm = new Resultado
            {
                IdOrden = ordId,
                IdPago = creado.IdPago,
                Metodo = creado.Metodo ?? model.Metodo,
                QrData = creado.QrData,
                QrExpiry = creado.QrExpiry,
                CipCode = creado.CipCode,
                CipExpiry = creado.CipExpiry,
                ReturnUrl = creado.ReturnUrl,
                Mensaje = creado.Metodo switch
                {
                    "YAPE" or "PLIN" => "Escanea el QR o sigue las instrucciones en tu app.",
                    "PAGOEFECTIVO" => "Usa el código CIP antes de la fecha de vencimiento.",
                    "TARJETA" => "Procesando pago con tarjeta.",
                    _ => "Pago registrado."
                }
            };

            return View("Pago", vm);
        }

        // GET: /Checkout/Return  (returnUrl del proveedor)
        [HttpGet]
        public IActionResult Return(int? idOrden, int? idPago, string? status = null)
        {
            ViewBag.IdOrden = idOrden;
            ViewBag.IdPago = idPago;
            ViewBag.Status = status;
            return View();
        }
    }
}
