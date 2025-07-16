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
    public class UsuariosController : Controller
    {
        private readonly BbddSmeallContext _context;

        public UsuariosController(BbddSmeallContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,NombreUsuario,Contrasena,Dni,Nombres,Apellidos,Telefono,Email,Genero,AreaAsignada,Rol,FechaRegistro,FechaActualizacion")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,NombreUsuario,Contrasena,Dni,Nombres,Apellidos,Telefono,Email,Genero,AreaAsignada,Rol,FechaRegistro,FechaActualizacion")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction("Index"); 
            }

            return View(usuario);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AccionMultiple(Usuario usuario, string accion)
        {
            if (accion == "Buscar")
            {
                var usuarioEncontrado = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == usuario.IdUsuario);

                if (usuarioEncontrado != null)
                {
                    return View("Registrar", usuarioEncontrado);
                }

                ModelState.AddModelError("", "Usuario no encontrado.");
                return View("Registrar");
            }

            if (accion == "Actualizar")
            {
                var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == usuario.IdUsuario);

                if (usuarioExistente != null)
                {
                    usuarioExistente.Nombres = usuario.Nombres;
                    usuarioExistente.Apellidos = usuario.Apellidos;
                    usuarioExistente.Dni = usuario.Dni;
                    usuarioExistente.Telefono = usuario.Telefono;
                    usuarioExistente.Email = usuario.Email;
                    usuarioExistente.Contrasena = usuario.Contrasena;
                    usuarioExistente.AreaAsignada = usuario.AreaAsignada;
                    usuarioExistente.Genero = usuario.Genero;

                    _context.SaveChanges();

                    ViewBag.Mensaje = "Usuario actualizado correctamente.";
                    return View("Registrar", usuarioExistente);
                }

                ModelState.AddModelError("", "No se encontró el usuario para actualizar.");
                return View("Registrar", usuario);
            }

            if (accion == "Registrar")
            {
                var existe = _context.Usuarios.Any(u => u.IdUsuario == usuario.IdUsuario);
                if (existe)
                {
                    ModelState.AddModelError("", "El usuario ya existe.");
                    return View("Registrar", usuario);
                }

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                ViewBag.Mensaje = "Usuario registrado correctamente.";
                return View("Registrar", new Usuario()); // formulario vacío
            }

            return View("Registrar", usuario);
        }



    }
}
