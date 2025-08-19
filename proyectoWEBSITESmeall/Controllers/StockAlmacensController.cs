
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using proyectoWEBSITESmeall.Models;

namespace proyectoWEBSITESmeall.Controllers
{
    public class StockAlmacensController : Controller
    {
        private readonly BbddSmeallContext _context;

        public StockAlmacensController(BbddSmeallContext context)
        {
            _context = context;
        }

        // GET: StockAlmacens
        public async Task<IActionResult> Index()
        {
            var bbddSmeallContext = _context.StockAlmacens
                .Include(s => s.IdAlmacenNavigation)
                .Include(s => s.IdProductoNavigation);
            return View(await bbddSmeallContext.ToListAsync());
        }

        // GET: StockAlmacens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var stockAlmacen = await _context.StockAlmacens
                .Include(s => s.IdAlmacenNavigation)
                .Include(s => s.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdStock == id);

            if (stockAlmacen == null) return NotFound();

            return View(stockAlmacen);
        }

        // GET: StockAlmacens/Create
        public IActionResult Create()
        {
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "Nombre");
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre");
            return View();
        }

        // POST: StockAlmacens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStock,IdAlmacen,IdProducto,Cantidad")] StockAlmacen stockAlmacen)
        {
            if (ModelState.IsValid)
            {
                stockAlmacen.FechaRegistro = DateTime.Now;
                stockAlmacen.FechaActualizacion = DateTime.Now;

                _context.Add(stockAlmacen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "Nombre", stockAlmacen.IdAlmacen);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", stockAlmacen.IdProducto);
            return View(stockAlmacen);
        }



        // GET: StockAlmacens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var stockAlmacen = await _context.StockAlmacens.FindAsync(id);
            if (stockAlmacen == null) return NotFound();

            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", stockAlmacen.IdAlmacen);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", stockAlmacen.IdProducto);
            return View(stockAlmacen);
        }

        // POST: StockAlmacens/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStock,IdAlmacen,IdProducto,Cantidad,FechaRegistro,FechaActualizacion")] StockAlmacen stockAlmacen)
        {
            if (id != stockAlmacen.IdStock) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockAlmacen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockAlmacenExists(stockAlmacen.IdStock)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", stockAlmacen.IdAlmacen);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", stockAlmacen.IdProducto);
            return View(stockAlmacen);
        }

        // GET: StockAlmacens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var stockAlmacen = await _context.StockAlmacens
                .Include(s => s.IdAlmacenNavigation)
                .Include(s => s.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdStock == id);

            if (stockAlmacen == null) return NotFound();

            return View(stockAlmacen);
        }

        // POST: StockAlmacens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockAlmacen = await _context.StockAlmacens.FindAsync(id);
            if (stockAlmacen != null)
            {
                _context.StockAlmacens.Remove(stockAlmacen);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StockAlmacenExists(int id)
        {
            return _context.StockAlmacens.Any(e => e.IdStock == id);
        }

        public IActionResult Pedido_Producto_Faltante()
        {
            return View();
        }
        public IActionResult ReporteInventario(string periodo = "mensual")
        {
            var hoy = DateTime.Now;

            // Rango de fechas según el período
            DateTime inicio, fin;

            switch (periodo.ToLower())
            {
                case "diario":
                    inicio = hoy.Date;
                    fin = inicio.AddDays(1);
                    break;

                case "semanal":
                    inicio = hoy.AddDays(-(int)hoy.DayOfWeek);
                    fin = inicio.AddDays(7);
                    break;

                case "mensual":
                    inicio = new DateTime(hoy.Year, hoy.Month, 1);
                    fin = inicio.AddMonths(1);
                    break;

                case "anual":
                    inicio = new DateTime(hoy.Year, 1, 1);
                    fin = inicio.AddYears(1);
                    break;

                default:
                    inicio = new DateTime(hoy.Year, hoy.Month, 1);
                    fin = inicio.AddMonths(1);
                    break;
            }

            // Traer TODOS los productos con Left Join a StockAlmacen
            var resultado = _context.Productos
                .Select(p => new
                {
                    Producto = p.Nombre,
                    CantidadTotal = _context.StockAlmacens
                        .Where(s => s.IdProducto == p.IdProducto && s.FechaRegistro >= inicio && s.FechaRegistro < fin)
                        .Sum(s => (int?)s.Cantidad) ?? 0,   // si no hay stock, pone 0
                    UltimaActualizacion = _context.StockAlmacens
                        .Where(s => s.IdProducto == p.IdProducto)
                        .Max(s => (DateTime?)s.FechaActualizacion) // puede ser null
                })
                .ToList();

            return View(resultado);
        }

    }
}
