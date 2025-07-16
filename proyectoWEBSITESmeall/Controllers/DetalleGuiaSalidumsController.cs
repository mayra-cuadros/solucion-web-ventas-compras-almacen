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
    public class DetalleGuiaSalidumsController : Controller
    {
        private readonly BbddSmeallContext _context;

        public DetalleGuiaSalidumsController(BbddSmeallContext context)
        {
            _context = context;
        }

        // GET: DetalleGuiaSalidums
        public async Task<IActionResult> Index()
        {
            var bbddSmeallContext = _context.DetalleGuiaSalida.Include(d => d.IdGuiaSalidaNavigation).Include(d => d.IdProductoNavigation);
            return View(await bbddSmeallContext.ToListAsync());
        }

        // GET: DetalleGuiaSalidums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleGuiaSalidum = await _context.DetalleGuiaSalida
                .Include(d => d.IdGuiaSalidaNavigation)
                .Include(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdDetalleGuia == id);
            if (detalleGuiaSalidum == null)
            {
                return NotFound();
            }

            return View(detalleGuiaSalidum);
        }

        // GET: DetalleGuiaSalidums/Create
        public IActionResult Create()
        {
            ViewData["IdGuiaSalida"] = new SelectList(_context.GuiaSalida, "IdGuiaSalida", "IdGuiaSalida");
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto");
            return View();
        }

        // POST: DetalleGuiaSalidums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetalleGuia,IdGuiaSalida,IdProducto,Cantidad,FechaRegistro,FechaActualizacion")] DetalleGuiaSalidum detalleGuiaSalidum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleGuiaSalidum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdGuiaSalida"] = new SelectList(_context.GuiaSalida, "IdGuiaSalida", "IdGuiaSalida", detalleGuiaSalidum.IdGuiaSalida);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detalleGuiaSalidum.IdProducto);
            return View(detalleGuiaSalidum);
        }

        // GET: DetalleGuiaSalidums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleGuiaSalidum = await _context.DetalleGuiaSalida.FindAsync(id);
            if (detalleGuiaSalidum == null)
            {
                return NotFound();
            }
            ViewData["IdGuiaSalida"] = new SelectList(_context.GuiaSalida, "IdGuiaSalida", "IdGuiaSalida", detalleGuiaSalidum.IdGuiaSalida);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detalleGuiaSalidum.IdProducto);
            return View(detalleGuiaSalidum);
        }

        // POST: DetalleGuiaSalidums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetalleGuia,IdGuiaSalida,IdProducto,Cantidad,FechaRegistro,FechaActualizacion")] DetalleGuiaSalidum detalleGuiaSalidum)
        {
            if (id != detalleGuiaSalidum.IdDetalleGuia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleGuiaSalidum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleGuiaSalidumExists(detalleGuiaSalidum.IdDetalleGuia))
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
            ViewData["IdGuiaSalida"] = new SelectList(_context.GuiaSalida, "IdGuiaSalida", "IdGuiaSalida", detalleGuiaSalidum.IdGuiaSalida);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "IdProducto", detalleGuiaSalidum.IdProducto);
            return View(detalleGuiaSalidum);
        }

        // GET: DetalleGuiaSalidums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleGuiaSalidum = await _context.DetalleGuiaSalida
                .Include(d => d.IdGuiaSalidaNavigation)
                .Include(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdDetalleGuia == id);
            if (detalleGuiaSalidum == null)
            {
                return NotFound();
            }

            return View(detalleGuiaSalidum);
        }

        // POST: DetalleGuiaSalidums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalleGuiaSalidum = await _context.DetalleGuiaSalida.FindAsync(id);
            if (detalleGuiaSalidum != null)
            {
                _context.DetalleGuiaSalida.Remove(detalleGuiaSalidum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleGuiaSalidumExists(int id)
        {
            return _context.DetalleGuiaSalida.Any(e => e.IdDetalleGuia == id);
        }
    }
}
