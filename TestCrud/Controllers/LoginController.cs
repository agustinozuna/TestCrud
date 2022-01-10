using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestCrud.Models;

namespace TestCrud.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly TestCrudContext _context;

        public LoginController(TestCrudContext context)
        {
            _context = context;

        }



        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(TUsers user)
        {
            if (ModelState.IsValid)
            {
                //Validación del usuario por username y contrasenha.
                bool IsValidUser = _context.TUsers
               .Any(u => u.TxtUser.ToLower() == user
               .TxtUser.ToLower() && user
               .TxtPassword == user.TxtPassword);
                if (IsValidUser)
                {
                    /*obtener rol del usuario logeado*/
                    var rol = _context.TUsers.Include(r => r.CodRolNavigation).FirstOrDefault(ul => ul.TxtUser == user.TxtUser).CodRolNavigation.TxtDesc;
                   
                    var claims = new[] { new Claim("User", user.TxtUser), new Claim(ClaimTypes.Role, rol) };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    if (rol=="Cliente")
                    {
                        return Redirect("~/Home/IndexCliente");
                    }
                    else
                    {
                        return Redirect("~/Home/Index");
                    }    
                }


            }
            ModelState.AddModelError("", "Usuario o contrasenha incorrecta");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }

    }
}
