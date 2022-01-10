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
    [Authorize(Roles = "Administrador,Cliente")]
    public class TPeliculasController : Controller
    {
        private readonly TestCrudContext _context;
        public IConfiguration Configuration { get; }

        public TPeliculasController(TestCrudContext context, IConfiguration configuration)
        {
             Configuration = configuration;
            _context = context;
        }

        // GET: TPeliculas
        public async Task<IActionResult> Index()
        {
            return View(await _context.TPelicula.ToListAsync());
        }

        // GET: TPeliculas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tPelicula = await _context.TPelicula
                .FirstOrDefaultAsync(m => m.CodPelicula == id);
            if (tPelicula == null)
            {
                return NotFound();
            }

            return View(tPelicula);
        }

        // GET: TPeliculas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TPeliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodPelicula,TxtDesc,CantDisponiblesAlquiler,CantDisponiblesVenta,PrecioAlquiler,PrecioVenta")] TPelicula tPelicula)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tPelicula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tPelicula);
        }

        // GET: TPeliculas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tPelicula = await _context.TPelicula.FindAsync(id);
            if (tPelicula == null)
            {
                return NotFound();
            }
            return View(tPelicula);
        }

        // POST: TPeliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodPelicula,TxtDesc,CantDisponiblesAlquiler,CantDisponiblesVenta,PrecioAlquiler,PrecioVenta")] TPelicula tPelicula)
        {
            if (id != tPelicula.CodPelicula)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tPelicula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TPeliculaExists(tPelicula.CodPelicula))
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
            return View(tPelicula);
        }

        // GET: TPeliculas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tPelicula = await _context.TPelicula
                .FirstOrDefaultAsync(m => m.CodPelicula == id);
            if (tPelicula == null)
            {
                return NotFound();
            }

            return View(tPelicula);
        }

        // POST: TPeliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tPelicula = await _context.TPelicula.FindAsync(id);
            _context.TPelicula.Remove(tPelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet]
        public ActionResult stockAlquiler()
        {
            List<TPelicula> stockAlquiler = new List<TPelicula>();
            using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("stockAlquiler", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    sql.Open();
                    var datareader = cmd.ExecuteReader();


                    while (datareader.Read())
                    {
                        TPelicula tPelicula = new TPelicula
                        {
                            CodPelicula = Convert.ToInt32(datareader.GetValue(0)),
                            TxtDesc = datareader.GetValue(1).ToString(),
                            CantDisponiblesAlquiler = Convert.ToInt32(datareader.GetValue(2)),
                            CantDisponiblesVenta = Convert.ToInt32(datareader.GetValue(3)),
                            PrecioAlquiler = Convert.ToDecimal(datareader.GetValue(4)),
                            PrecioVenta = Convert.ToDecimal(datareader.GetValue(5))

                        };
                        stockAlquiler.Add(tPelicula);
                    }
                    cmd.Dispose();
                    sql.Close();
                    datareader.Close();
                }
            }
            return View(stockAlquiler);
        }


        [Authorize(Roles = "Administrador,Cliente")]
        [HttpGet]
        public ActionResult stockVenta()
        {
            List<TPelicula> stockAlquiler = new List<TPelicula>();
            using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("stockVenta", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    sql.Open();
                    var datareader = cmd.ExecuteReader();


                    while (datareader.Read())
                    {
                        TPelicula tPelicula = new TPelicula
                        {
                            CodPelicula = Convert.ToInt32(datareader.GetValue(0)),
                            TxtDesc = datareader.GetValue(1).ToString(),
                            CantDisponiblesAlquiler = Convert.ToInt32(datareader.GetValue(2)),
                            CantDisponiblesVenta = Convert.ToInt32(datareader.GetValue(3)),
                            PrecioAlquiler = Convert.ToDecimal(datareader.GetValue(4)),
                            PrecioVenta = Convert.ToDecimal(datareader.GetValue(5))

                        };
                        stockAlquiler.Add(tPelicula);
                    }
                    cmd.Dispose();
                    sql.Close();
                    datareader.Close();
                }
            }
            return View(stockAlquiler);
        }



        private bool TPeliculaExists(int id)
        {
            return _context.TPelicula.Any(e => e.CodPelicula == id);
        }
    }
}
