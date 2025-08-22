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

        // -------------------------------
        // 🚀 Reporte de Inventario
        // -------------------------------
        public IActionResult ReporteInventario(string periodo = "mensual", DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            var query = _context.StockAlmacens
                .Include(s => s.IdProductoNavigation)
                .AsQueryable();

            DateTime hoy = DateTime.Now;

            // ✅ Filtro por rango de fechas
            if (fechaDesde.HasValue && fechaHasta.HasValue)
            {
                query = query.Where(s => s.FechaRegistro >= fechaDesde.Value && s.FechaRegistro <= fechaHasta.Value);
            }
            else
            {
                // ✅ Filtro por período
                switch (periodo.ToLower())
                {
                    case "diario":
                        DateTime inicioDia = hoy.Date;
                        DateTime finDia = inicioDia.AddDays(1);
                        query = query.Where(s => s.FechaRegistro >= inicioDia && s.FechaRegistro < finDia);
                        break;

                    case "semanal":
                        DateTime inicioSemana = hoy.Date.AddDays(-(int)hoy.DayOfWeek);
                        DateTime finSemana = inicioSemana.AddDays(7);
                        query = query.Where(s => s.FechaRegistro >= inicioSemana && s.FechaRegistro < finSemana);
                        break;

                    case "quincenal":
                        DateTime inicioQuincena = hoy.Date.AddDays(-15);
                        query = query.Where(s => s.FechaRegistro >= inicioQuincena);
                        break;

                    case "mensual":
                        DateTime inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
                        DateTime finMes = inicioMes.AddMonths(1);
                        query = query.Where(s => s.FechaRegistro >= inicioMes && s.FechaRegistro < finMes);
                        break;

                    case "anual":
                        DateTime inicioAnio = new DateTime(hoy.Year, 1, 1);
                        DateTime finAnio = inicioAnio.AddYears(1);
                        query = query.Where(s => s.FechaRegistro >= inicioAnio && s.FechaRegistro < finAnio);
                        break;
                }
            }

            var resultado = query
                .GroupBy(s => s.IdProductoNavigation.Nombre)
                .Select(g => new
                {
                    Producto = g.Key,
                    CantidadTotal = g.Sum(x => x.Cantidad),
                    UltimaActualizacion = g.Max(x => x.FechaActualizacion)
                })
                .ToList();

            return View(resultado);
        }
    }
}
