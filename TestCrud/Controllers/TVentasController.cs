using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestCrud.Models;

namespace TestCrud.Controllers
{
    public class TVentasController : Controller
    {
        private readonly TestCrudContext _context;

        public TVentasController(TestCrudContext context)
        {
            _context = context;
        }

        // GET: TVentas
        public async Task<IActionResult> Index()
        {
            var testCrudContext = _context.TVenta.Include(t => t.CodUsuarioNavigation);
            return View(await testCrudContext.ToListAsync());
        }

        // GET: TVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tVenta = await _context.TVenta
                .Include(t => t.CodUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.CodVenta == id);
            if (tVenta == null)
            {
                return NotFound();
            }

            return View(tVenta);
        }

        // GET: TVentas/Create
        public IActionResult Create()
        {
            ViewData["CodUsuario"] = new SelectList(_context.TUsers, "CodUsuario", "CodUsuario");
            return View();
        }

        // POST: TVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodVenta,CodUsuario,Total,Fecha")] TVenta tVenta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tVenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodUsuario"] = new SelectList(_context.TUsers, "CodUsuario", "CodUsuario", tVenta.CodUsuario);
            return View(tVenta);
        }

        
        private bool TVentaExists(int id)
        {
            return _context.TVenta.Any(e => e.CodVenta == id);
        }
    }
}
