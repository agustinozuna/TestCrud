using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestCrud.Models;
using System.Web;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace TestCrud.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TAlquilersController : Controller
    {
        private readonly TestCrudContext _context;
        public IConfiguration Configuration { get; }

        public TAlquilersController(TestCrudContext context, IConfiguration configuration)
        {
            Configuration = configuration;
            _context = context;
        }

        // GET: TAlquilers
        public async Task<IActionResult> Index()
        {
            var testCrudContext = _context.TAlquiler.Include(t => t.CodUsuarioNavigation);
            return View(await testCrudContext.ToListAsync());
        }

        // GET: TAlquilers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tAlquiler = await _context.TAlquiler
                .Include(t => t.CodUsuarioNavigation)
                .Include(t => t.TdetalleAlquiler)
                .FirstOrDefaultAsync(m => m.CodAlquiler == id);
            if (tAlquiler == null)
            {
                return NotFound();
            }

            return View(tAlquiler);
        }

        // GET: TAlquilers/Create
        public IActionResult Create()
        {
            ViewData["CodUsuario"] = new SelectList(_context.TUsers, "CodUsuario", "TxtUser", 0);
            ViewData["CodPelicula"] = new SelectList(_context.TPelicula.Where(p => p.CantDisponiblesAlquiler > 0), "CodPelicula", "TxtDesc");
            ViewData["PrecioAlquiler"] = new SelectList(_context.TPelicula.Where(p => p.CantDisponiblesAlquiler > 0), "CodPelicula", "PrecioAlquiler", 0);
            //ViewData["CodUsuario"] = new SelectList(_context.TUsers, "CodUsuario", "CodUsuario");
            return View();
        }

        //[HttpPost]
        public ActionResult GuardarTransaccion([FromBody]DetalleAlquilerJson da)
        {
            try
            {
                var cod_alquiler= "";
                using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    /*Verificacion de stock alquiler por procedimiento almacenado*/

                    for (int i = 0; i < da.CodPelicula.Length; i++)
                    {
                        using (SqlCommand cmd = new SqlCommand("verificarStock", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@tipo_stock", 1));
                            cmd.Parameters.Add(new SqlParameter("@cod_pelicula", da.CodPelicula[i]));
                            cmd.Parameters.Add(new SqlParameter("@cantidad", 1));
                            sql.Open();
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            sql.Close();
                        }
                    }

                    /*Cabecera detalle alquiler*/
                    using (SqlCommand cmd = new SqlCommand("alquilarPelicula", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@cod_usuario", da.CodUsuario));
                        sql.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            cod_alquiler=(Convert.ToString(dr.GetValue(0)));
                        }
                        cmd.Dispose();
                        sql.Close();
                    }
                    /*insercion de detalle alquiler*/
                    for (int i = 0; i < da.CodPelicula.Length; i++)
                    {
                        using (SqlCommand cmd = new SqlCommand("detalleAlquilerPelicula", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@cod_alquiler", cod_alquiler));
                            cmd.Parameters.Add(new SqlParameter("@cod_detalleAlquiler", i+1));
                            cmd.Parameters.Add(new SqlParameter("@cod_pelicula", da.CodPelicula[i]));
                            sql.Open();
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            sql.Close();
                        }


                    }

                    }
            }

            catch (Exception ex)
            {
                return Json(ex.Message);
            }

                return Json(true);         
        }


        public ActionResult DevolverPelicula(int codPelicula, int codAlquiler, int codDetalleAlquiler)
        {
            using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand cmd = new SqlCommand("devolverPelicula", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cod_alquiler", codAlquiler));
                    cmd.Parameters.Add(new SqlParameter("@cod_detalleAlquiler", codDetalleAlquiler));
                    cmd.Parameters.Add(new SqlParameter("@cod_pelicula", codPelicula));
                    sql.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sql.Close();
                }
            }
                return Redirect("~/TAlquilers/Details/"+codAlquiler);
            
        }
            private bool TAlquilerExists(int id)
        {
            return _context.TAlquiler.Any(e => e.CodAlquiler == id);
        }
    }
}
