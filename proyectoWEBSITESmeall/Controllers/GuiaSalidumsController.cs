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
    public class GuiaSalidumsController : Controller
    {
        private readonly BbddSmeallContext _context;

        public GuiaSalidumsController(BbddSmeallContext context)
        {
            _context = context;
        }

        // GET: GuiaSalidums
        public async Task<IActionResult> Index()
        {
            var bbddSmeallContext = _context.GuiaSalida.Include(g => g.IdAlmacenNavigation);
            return View(await bbddSmeallContext.ToListAsync());
        }

        // GET: GuiaSalidums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guiaSalidum = await _context.GuiaSalida
                .Include(g => g.IdAlmacenNavigation)
                .FirstOrDefaultAsync(m => m.IdGuiaSalida == id);
            if (guiaSalidum == null)
            {
                return NotFound();
            }

            return View(guiaSalidum);
        }

        // GET: GuiaSalidums/Create
        public IActionResult Create()
        {
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen");
            return View();
        }

        // POST: GuiaSalidums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGuiaSalida,FechaSalida,Responsable,Destino,IdAlmacen,FechaRegistro,FechaActualizacion")] GuiaSalidum guiaSalidum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(guiaSalidum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", guiaSalidum.IdAlmacen);
            return View(guiaSalidum);
        }

        // GET: GuiaSalidums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guiaSalidum = await _context.GuiaSalida.FindAsync(id);
            if (guiaSalidum == null)
            {
                return NotFound();
            }
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", guiaSalidum.IdAlmacen);
            return View(guiaSalidum);
        }

        // POST: GuiaSalidums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGuiaSalida,FechaSalida,Responsable,Destino,IdAlmacen,FechaRegistro,FechaActualizacion")] GuiaSalidum guiaSalidum)
        {
            if (id != guiaSalidum.IdGuiaSalida)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guiaSalidum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuiaSalidumExists(guiaSalidum.IdGuiaSalida))
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
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", guiaSalidum.IdAlmacen);
            return View(guiaSalidum);
        }

        // GET: GuiaSalidums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guiaSalidum = await _context.GuiaSalida
                .Include(g => g.IdAlmacenNavigation)
                .FirstOrDefaultAsync(m => m.IdGuiaSalida == id);
            if (guiaSalidum == null)
            {
                return NotFound();
            }

            return View(guiaSalidum);
        }

        // POST: GuiaSalidums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guiaSalidum = await _context.GuiaSalida.FindAsync(id);
            if (guiaSalidum != null)
            {
                _context.GuiaSalida.Remove(guiaSalidum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuiaSalidumExists(int id)
        {
            return _context.GuiaSalida.Any(e => e.IdGuiaSalida == id);
        }
    }
}
