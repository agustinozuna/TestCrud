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
    public class TVentasController : Controller
    {
        private readonly TestCrudContext _context;
        public IConfiguration Configuration { get; }
        public TVentasController(TestCrudContext context, IConfiguration configuration)
        {
            Configuration = configuration;
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
            ViewData["CodUsuario"] = new SelectList(_context.TUsers, "CodUsuario", "TxtUser", 0);
            ViewData["CodPelicula"] = new SelectList(_context.TPelicula.Where(p => p.CantDisponiblesVenta > 0), "CodPelicula", "TxtDesc");
            ViewData["PrecioVenta"] = new SelectList(_context.TPelicula.Where(p => p.CantDisponiblesVenta > 0), "CodPelicula", "PrecioVenta", 0);
            return View();
        }

        public ActionResult GuardarTransaccion([FromBody] DetalleVentaJson da)
        {
            try
            {
                var cod_venta = "";
                using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    /*Verificacion de stock alquiler por procedimiento almacenado*/

                    for (int i = 0; i < da.CodPelicula.Length; i++)
                    {
                        using (SqlCommand cmd = new SqlCommand("verificarStock", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@tipo_stock", 2));
                            cmd.Parameters.Add(new SqlParameter("@cod_pelicula", da.CodPelicula[i]));
                            cmd.Parameters.Add(new SqlParameter("@cantidad", da.Cantidad[i]));
                            sql.Open();
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            sql.Close();
                        }
                    }

                    /*Cabecera detalle alquiler*/
                    using (SqlCommand cmd = new SqlCommand("venderPelicula", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@cod_usuario", da.CodUsuario));
                        sql.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            cod_venta = (Convert.ToString(dr.GetValue(0)));
                        }
                        cmd.Dispose();
                        sql.Close();
                    }
                    /*insercion de detalle alquiler*/
                    for (int i = 0; i < da.CodPelicula.Length; i++)
                    {
                        using (SqlCommand cmd = new SqlCommand("detalleVentaPelicula", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@cod_venta", cod_venta));
                            cmd.Parameters.Add(new SqlParameter("@cod_detalleVenta", i + 1));
                            cmd.Parameters.Add(new SqlParameter("@cod_pelicula", da.CodPelicula[i]));
                            cmd.Parameters.Add(new SqlParameter("@cantidad", da.Cantidad[i]));
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






        private bool TVentaExists(int id)
        {
            return _context.TVenta.Any(e => e.CodVenta == id);
        }
    }
}
