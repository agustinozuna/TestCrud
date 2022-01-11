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
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.TPelicula.ToListAsync());
        }

        // GET: TPeliculas/Details/5
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TPeliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CodPelicula,TxtDesc,CantDisponiblesAlquiler,CantDisponiblesVenta,PrecioAlquiler,PrecioVenta")] TPelicula tPelicula)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("crearPelicula", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@txt_desc", tPelicula.TxtDesc));
                        cmd.Parameters.Add(new SqlParameter("@cant_disponibles_alquiler", tPelicula.CantDisponiblesAlquiler));
                        cmd.Parameters.Add(new SqlParameter("@cant_disponibles_venta", tPelicula.CantDisponiblesVenta));
                        cmd.Parameters.Add(new SqlParameter("@precio_alquiler", tPelicula.PrecioAlquiler));
                        cmd.Parameters.Add(new SqlParameter("@precio_venta", tPelicula.PrecioVenta));
                        sql.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        sql.Close();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tPelicula);
        }

        // GET: TPeliculas/Edit/5
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CodPelicula,TxtDesc,CantDisponiblesAlquiler,CantDisponiblesVenta,PrecioAlquiler,PrecioVenta")] TPelicula tPelicula)
        {
            if (id != tPelicula.CodPelicula)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                    {
                        using (SqlCommand cmd = new SqlCommand("modificarPelicula", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@cod_pelicula", tPelicula.CodPelicula));
                            cmd.Parameters.Add(new SqlParameter("@txt_desc", tPelicula.TxtDesc));
                            cmd.Parameters.Add(new SqlParameter("@cant_disponibles_alquiler", tPelicula.CantDisponiblesAlquiler));
                            cmd.Parameters.Add(new SqlParameter("@cant_disponibles_venta", tPelicula.CantDisponiblesVenta));
                            cmd.Parameters.Add(new SqlParameter("@precio_alquiler", tPelicula.PrecioAlquiler));
                            cmd.Parameters.Add(new SqlParameter("@precio_venta", tPelicula.PrecioVenta));
                            sql.Open();
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            sql.Close();
                        }
                    }

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
        [Authorize(Roles = "Administrador")]
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
        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("borrarPelicula", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cod_pelicula", id));
                    sql.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sql.Close();
                }
            }
            return RedirectToAction(nameof(Index));
        }


        /*Asignación de genero*/
        [Authorize(Roles = "Administrador")]
        public ActionResult AsignarGenero(int id)
        {
            var pelicula = _context.TPelicula.Include(p => p.TGeneroPelicula).FirstOrDefault(p => p.CodPelicula == id);



            ViewData["CodGenero"] = new SelectList(_context.TGenero, "CodGenero", "TxtDesc");

            ViewBag.CodGenero = new SelectList(_context.TGenero, "CodGenero", "TxtDesc");
            return View(pelicula);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult AsignarGenero([Bind("CodPelicula")] TPelicula tPelicula, int CodGenero)
        {
            ViewBag.CodGenero = new SelectList(_context.TGenero, "CodGenero", "TxtDesc");
            try
            {
                using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand cmd = new SqlCommand("asignarGenero", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@cod_pelicula", tPelicula.CodPelicula));
                        cmd.Parameters.Add(new SqlParameter("@cod_genero", CodGenero));
                        sql.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        sql.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
                return View(_context.TPelicula.Include(p => p.TGeneroPelicula).FirstOrDefault(p => p.CodPelicula == tPelicula.CodPelicula));
            }
            var pelicula = _context.TPelicula.Include(p => p.TGeneroPelicula).FirstOrDefault(p => p.CodPelicula == tPelicula.CodPelicula);

            return View(pelicula);
        }




        [Authorize(Roles = "Administrador, Cliente")]
        [HttpGet]
        public ActionResult StockAlquiler()
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


        [Authorize(Roles = "Administrador, Cliente")]
        [HttpGet]
        public ActionResult StockVenta()
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


        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public ActionResult RecaudoPeliculasAlquiladas()
        {
            DataTable dt = new DataTable();
            using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("recaudoPeliculasAlquiladas", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    sql.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    cmd.Dispose();
                    sql.Close();

                }
            }

            return View(dt);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public ActionResult PeliculasSinDevoluciones()
        {
            DataTable dt = new DataTable();
            using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("peliculasSinDevoluciones", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    sql.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    cmd.Dispose();
                    sql.Close();

                }
            }
            return View(dt);
        }


        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public ActionResult UsuariosSinDevolucionPorPelicula(int cod_pelicula)
        {
            ViewBag.Pelicula = _context.TPelicula.FirstOrDefault(p => p.CodPelicula == cod_pelicula).TxtDesc;

            DataTable dt = new DataTable();
            using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("usuariosSinDevolucionPorPelicula", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cod_pelicula", cod_pelicula));
                    sql.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    cmd.Dispose();
                    sql.Close();
                }
            }
            return View(dt);
        }

        /**/
        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult GetPeliculasAlquiler()
        {

            //List<TPelicula> lst = new List<TPelicula>();

            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            // where agregado 
            var peliculasAlquiler = (from tempPeliAlquiler in _context.TPelicula select tempPeliAlquiler).Where(p=>p.CantDisponiblesAlquiler>0);

            if (!string.IsNullOrEmpty(searchValue))
            {
                //where agregado
                peliculasAlquiler = peliculasAlquiler.Where(p => p.TxtDesc.ToLower().Contains(searchValue.ToLower().Trim()));
            }

            recordsTotal = peliculasAlquiler.Count();
            var data = peliculasAlquiler.Skip(skip).Take(pageSize).ToList();
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Json(jsonData);
        }



        private bool TPeliculaExists(int id)
        {
            return _context.TPelicula.Any(e => e.CodPelicula == id);
        }
    }
}
