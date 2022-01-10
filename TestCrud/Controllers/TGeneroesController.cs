using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestCrud.Models;

namespace TestCrud.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TGeneroesController : Controller
    {
        private readonly TestCrudContext _context;
        public IConfiguration Configuration { get; }
        public TGeneroesController(TestCrudContext context, IConfiguration configuration)
        {
            Configuration = configuration;
            _context = context;
        }

        // GET: TGeneroes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TGenero.ToListAsync());
        }

        // GET: TGeneroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tGenero = await _context.TGenero
                .FirstOrDefaultAsync(m => m.CodGenero == id);
            if (tGenero == null)
            {
                return NotFound();
            }

            return View(tGenero);
        }

        // GET: TGeneroes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TGeneroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodGenero,TxtDesc")] TGenero tGenero)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("crearGenero", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@txt_desc", tGenero.TxtDesc));

                        sql.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        sql.Close();
                    }
                }



                return RedirectToAction(nameof(Index));
            }
            return View(tGenero);
        }

        // GET: TGeneroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tGenero = await _context.TGenero.FindAsync(id);
            if (tGenero == null)
            {
                return NotFound();
            }
            return View(tGenero);
        }

        // POST: TGeneroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodGenero,TxtDesc")] TGenero tGenero)
        {
            if (id != tGenero.CodGenero)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tGenero);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TGeneroExists(tGenero.CodGenero))
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
            return View(tGenero);
        }

        // GET: TGeneroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tGenero = await _context.TGenero
                .FirstOrDefaultAsync(m => m.CodGenero == id);
            if (tGenero == null)
            {
                return NotFound();
            }

            return View(tGenero);
        }

        // POST: TGeneroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tGenero = await _context.TGenero.FindAsync(id);
            _context.TGenero.Remove(tGenero);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TGeneroExists(int id)
        {
            return _context.TGenero.Any(e => e.CodGenero == id);
        }
    }
}
