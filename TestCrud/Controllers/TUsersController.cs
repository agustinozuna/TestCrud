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
    public class TUsersController : Controller
    {
        private readonly TestCrudContext _context;
        public IConfiguration Configuration { get; }
        public TUsersController(TestCrudContext context, IConfiguration configuration)
        {
            Configuration = configuration;
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
            ViewData["CodRol"] = new SelectList(_context.TRol, "CodRol", "TxtDesc");
            return View();
        }

        // POST: TUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodUsuario,TxtUser,TxtPassword,ConfirmPassword,TxtNombre,TxtApellido,NroDoc,CodRol")] TUsers tUsers)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    using (SqlConnection sql = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                    {
                        using (SqlCommand cmd = new SqlCommand("crearUsuario", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@txtUser", tUsers.TxtUser));
                            cmd.Parameters.Add(new SqlParameter("@txtPassword", tUsers.TxtPassword));
                            cmd.Parameters.Add(new SqlParameter("@txtNombre", tUsers.TxtNombre));
                            cmd.Parameters.Add(new SqlParameter("@txtApellido", tUsers.TxtApellido));
                            cmd.Parameters.Add(new SqlParameter("@txtnro_doc", tUsers.NroDoc));
                            cmd.Parameters.Add(new SqlParameter("@cod_rol", tUsers.CodRol));
                            sql.Open();
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                            sql.Close();
                        }
                    }

                }
                catch (Exception ex)
                {
                    ViewData["CodRol"] = new SelectList(_context.TRol, "CodRol", "TxtDesc", tUsers.CodRol);
                    ViewData["error"] = ex.Message;
                    return View(tUsers);
                }


                return RedirectToAction(nameof(Index));
            }
            ViewData["CodRol"] = new SelectList(_context.TRol, "CodRol", "TxtDesc", tUsers.CodRol);
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
            ViewData["CodRol"] = new SelectList(_context.TRol, "CodRol", "TxtDesc", tUsers.CodRol);
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
            ViewData["CodRol"] = new SelectList(_context.TRol, "CodRol", "TxtDesc", tUsers.CodRol);
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

        [HttpPost]
        public ActionResult GetUsers()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

          
            var users = (from tempUsers in _context.TUsers select tempUsers);

            if (!string.IsNullOrEmpty(searchValue))
            {
                //busqueda por usuario
                users = users.Where(p => p.TxtUser.ToLower().Contains(searchValue.ToLower().Trim()));
            }

            recordsTotal = users.Count();
            var data = users.Skip(skip).Take(pageSize).ToList();
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Json(jsonData);
        }




        private bool TUsersExists(int id)
        {
            return _context.TUsers.Any(e => e.CodUsuario == id);
        }
    }
}
