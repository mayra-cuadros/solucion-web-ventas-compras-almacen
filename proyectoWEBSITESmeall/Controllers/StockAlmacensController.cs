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
            var bbddSmeallContext = _context.StockAlmacens.Include(s => s.IdAlmacenNavigation).Include(s => s.IdProductoNavigation);
            return View(await bbddSmeallContext.ToListAsync());
        }

        // GET: StockAlmacens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockAlmacen = await _context.StockAlmacens
                .Include(s => s.IdAlmacenNavigation)
                .Include(s => s.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdStock == id);
            if (stockAlmacen == null)
            {
                return NotFound();
            }

            return View(stockAlmacen);
        }

        // GET: StockAlmacens/Create
        public IActionResult Create()
        {
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen");
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto");
            return View();
        }

        // POST: StockAlmacens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStock,IdAlmacen,IdProducto,Cantidad,FechaRegistro,FechaActualizacion")] StockAlmacen stockAlmacen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockAlmacen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", stockAlmacen.IdAlmacen);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", stockAlmacen.IdProducto);
            return View(stockAlmacen);
        }

        // GET: StockAlmacens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockAlmacen = await _context.StockAlmacens.FindAsync(id);
            if (stockAlmacen == null)
            {
                return NotFound();
            }
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", stockAlmacen.IdAlmacen);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", stockAlmacen.IdProducto);
            return View(stockAlmacen);
        }

        // POST: StockAlmacens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStock,IdAlmacen,IdProducto,Cantidad,FechaRegistro,FechaActualizacion")] StockAlmacen stockAlmacen)
        {
            if (id != stockAlmacen.IdStock)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockAlmacen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockAlmacenExists(stockAlmacen.IdStock))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", stockAlmacen.IdAlmacen);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", stockAlmacen.IdProducto);
            return View(stockAlmacen);
        }

        // GET: StockAlmacens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockAlmacen = await _context.StockAlmacens
                .Include(s => s.IdAlmacenNavigation)
                .Include(s => s.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdStock == id);
            if (stockAlmacen == null)
            {
                return NotFound();
            }

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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockAlmacenExists(int id)
        {
            return _context.StockAlmacens.Any(e => e.IdStock == id);
        }
    }
}
