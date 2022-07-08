using Microsoft.AspNetCore.Mvc;
//using BCrypt.Net;


namespace EcommerceStoreWithASPCoreMVC.Controllers
{
    //[Route("home")]
    public class HomeController : Controller
    {
        //[Route("index")]
        //[Route("")]
        //[Route("~/")]
        public IActionResult Index()
        {
            //string password = BCrypt.Net.BCrypt.HashPassword("123");
            return View();
        }
    }
}