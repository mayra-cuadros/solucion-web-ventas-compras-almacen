﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using proyectoWEBSITESmeall.Models;

namespace proyectoWEBSITESmeall.Controllers
{
    public class VentumsController : Controller
    {
        private readonly BbddSmeallContext _context;

        public VentumsController(BbddSmeallContext context)
        {
            _context = context;
        }

        // GET: Ventums
        public async Task<IActionResult> Index()
        {
            var bbddSmeallContext = _context.Venta.Include(v => v.IdAlmacenNavigation).Include(v => v.IdClienteNavigation).Include(v => v.IdUsuarioNavigation);
            return View(await bbddSmeallContext.ToListAsync());
        }

        // GET: Ventums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventum = await _context.Venta
                .Include(v => v.IdAlmacenNavigation)
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (ventum == null)
            {
                return NotFound();
            }

            return View(ventum);
        }

        // GET: Ventums/Create
        public IActionResult Create()
        {
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen");
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: Ventums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVenta,FechaVenta,IdAlmacen,IdUsuario,IdCliente,FechaRegistro,FechaActualizacion")] Ventum ventum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ventum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", ventum.IdAlmacen);
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", ventum.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", ventum.IdUsuario);
            return View(ventum);
        }

        // GET: Ventums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventum = await _context.Venta.FindAsync(id);
            if (ventum == null)
            {
                return NotFound();
            }
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", ventum.IdAlmacen);
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", ventum.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", ventum.IdUsuario);
            return View(ventum);
        }

        // POST: Ventums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVenta,FechaVenta,IdAlmacen,IdUsuario,IdCliente,FechaRegistro,FechaActualizacion")] Ventum ventum)
        {
            if (id != ventum.IdVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ventum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentumExists(ventum.IdVenta))
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
            ViewData["IdAlmacen"] = new SelectList(_context.Almacens, "IdAlmacen", "IdAlmacen", ventum.IdAlmacen);
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "IdCliente", ventum.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", ventum.IdUsuario);
            return View(ventum);
        }

        // GET: Ventums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventum = await _context.Venta
                .Include(v => v.IdAlmacenNavigation)
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (ventum == null)
            {
                return NotFound();
            }

            return View(ventum);
        }

        // POST: Ventums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ventum = await _context.Venta.FindAsync(id);
            if (ventum != null)
            {
                _context.Venta.Remove(ventum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentumExists(int id)
        {
            return _context.Venta.Any(e => e.IdVenta == id);
        }
    }
}
