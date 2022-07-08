using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ecommercestorewithaspcoremvc.DataAccess;
using ecommercestorewithaspcoremvc.Models;
using ecommercestorewithaspcoremvc.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ecommercestorewithaspcoremvc.Areas.Admin.Controllers
{
    //[Route("[controller]")]
    [Area("admin")]
    [Route("admin/login")]
    public class LoginController : Controller
    {
        /*private readonly ILogger<LoginController> _logger;
        public LoginController(ILogger<LoginController> logger)
        {
        _logger = logger;
        }*/

        private EcommerceContext _db = new EcommerceContext();
        private SecurityManager securityManager = new SecurityManager();
        public LoginController(EcommerceContext db)
        {
            _db = db;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("index")]
        public IActionResult Index(string username, string password)
        {
            var account = ProcessLogin(username, password);
            if(account != null)
            {
                securityManager.SignIn(this.HttpContext, account);
                return RedirectToAction("index", "dashboard", new { area = "admin" });
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return View("Index");
            }
        }

        private Account ProcessLogin(string username, string password)
        {
            var account = _db.Accounts.SingleOrDefault(a => a.Username.Equals(username) && a.Status == true);
            if(account != null)
            {
                if(BCrypt.Net.BCrypt.Verify(password, account.Password))
                {
                    return account;
                }
            }
            return null;
        }

        [Route("signout")]
        public IActionResult SignOut()
        {
            securityManager.SignOut(this.HttpContext);
            return RedirectToAction("index", "login", new { area = "admin" });
        }

        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            securityManager.SignOut(this.HttpContext);
            return RedirectToAction("index", "login", new { area = "admin" });
        }

        [Route("accessdenied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }



        /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }*/
    }
}