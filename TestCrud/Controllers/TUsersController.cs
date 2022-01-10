using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestCrud.Models;

namespace TestCrud.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TUsersController : Controller
    {
        private readonly TestCrudContext _context;

        public TUsersController(TestCrudContext context)
        {
            _context = context;
        }

        // GET: TUsers
        public async Task<IActionResult> Index()
        {
            var testCrudContext = _context.TUsers.Include(t => t.CodRolNavigation);
            return View(await testCrudContext.ToListAsync());
        }

        // GET: TUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tUsers = await _context.TUsers
                .Include(t => t.CodRolNavigation)
                .FirstOrDefaultAsync(m => m.CodUsuario == id);
            if (tUsers == null)
            {
                return NotFound();
            }

            return View(tUsers);
        }

        // GET: TUsers/Create
        public IActionResult Create()
        {
            ViewData["CodRol"] = new SelectList(_context.TRol, "CodRol", "CodRol");
            return View();
        }

        // POST: TUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodUsuario,TxtUser,TxtPassword,TxtNombre,TxtApellido,NroDoc,CodRol,SnActivo")] TUsers tUsers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tUsers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodRol"] = new SelectList(_context.TRol, "CodRol", "CodRol", tUsers.CodRol);
            return View(tUsers);
        }

        // GET: TUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tUsers = await _context.TUsers.FindAsync(id);
            if (tUsers == null)
            {
                return NotFound();
            }
            ViewData["CodRol"] = new SelectList(_context.TRol, "CodRol", "CodRol", tUsers.CodRol);
            return View(tUsers);
        }

        // POST: TUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodUsuario,TxtUser,TxtPassword,TxtNombre,TxtApellido,NroDoc,CodRol,SnActivo")] TUsers tUsers)
        {
            if (id != tUsers.CodUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tUsers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TUsersExists(tUsers.CodUsuario))
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
            ViewData["CodRol"] = new SelectList(_context.TRol, "CodRol", "CodRol", tUsers.CodRol);
            return View(tUsers);
        }

        // GET: TUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tUsers = await _context.TUsers
                .Include(t => t.CodRolNavigation)
                .FirstOrDefaultAsync(m => m.CodUsuario == id);
            if (tUsers == null)
            {
                return NotFound();
            }

            return View(tUsers);
        }

        // POST: TUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tUsers = await _context.TUsers.FindAsync(id);
            _context.TUsers.Remove(tUsers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TUsersExists(int id)
        {
            return _context.TUsers.Any(e => e.CodUsuario == id);
        }
    }
}
