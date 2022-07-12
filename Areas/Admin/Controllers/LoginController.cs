using System.Security.Claims;
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

        [Route("_signout")]
        public IActionResult _SignOut()
        {
            securityManager.SignOut(this.HttpContext);
            return RedirectToAction("index", "login", new { area = "admin" });
        }

        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var Username = user.Value;
            var account = _db.Accounts.SingleOrDefault(a => a.Username.Equals(Username));
            return View("Profile", account);
        }

        [HttpPost]
        [Route("profile")]
        public IActionResult Profile(Account account)
        {
            //var currentAccount = _db.Accounts.SingleOrDefault(a => a.Username.Equals(account.Username));
            var currentAccount = _db.Accounts.SingleOrDefault(a => a.Id == account.Id);
            if(!string.IsNullOrEmpty(account.Password))
            {
                currentAccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            }
            currentAccount.Username = account.Username;
            //currentAccount.Password = account.Password;
            currentAccount.Email = account.Email;
            currentAccount.FullName = account.FullName;
            //currentAccount.Status = account.Status;
            _db.SaveChanges();

            ViewBag.msg = "Profile updated successfully(Done)";
            return View("Profile");
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