using System.IO;
using System.Linq;
using ecommercestorewithaspcoremvc.DataAccess;
using ecommercestorewithaspcoremvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecommercestorewithaspcoremvc.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    [Area("admin")]
    [Route("admin/slideshow")]
    public class SlideShowController : Controller
    {
        private readonly IHostingEnvironment _env;
        private EcommerceContext _db = new EcommerceContext();
        public SlideShowController(EcommerceContext db, IHostingEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.SlideShows = _db.SlideShows.ToList();
            return View();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var slideShow = new SlideShow();
            return View("Add", slideShow);
        }
        
        [HttpPost]
        [Route("add")]
        public IActionResult Add(SlideShow slideShow, IFormFile photo)
        {
            //source code for uploading an image
            var path = Path.Combine(this._env.WebRootPath, "SlideShows", photo.FileName);
            var stream = new FileStream(path, FileMode.Create);
            photo.CopyToAsync(stream);
            //slideShow.Name = photo.FileName;    
            //I need the URL of the Image
            slideShow.Photo = photo.FileName;

            _db.SlideShows.Add(slideShow);
            _db.SaveChanges();
            return RedirectToAction("Index", "slideshow", new { area = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var slideShow = _db.SlideShows.Find(id);
            _db.SlideShows.Remove(slideShow);
            _db.SaveChanges();
            return RedirectToAction("Index", "slideshow", new { area = "admin" });
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var slideShow = _db.SlideShows.Find(id);
            return View("Edit", slideShow);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, SlideShow slideShow, IFormFile photo)
        {
            var currentSlideShow = _db.SlideShows.Find(id);
            currentSlideShow.Name = slideShow.Name;
            currentSlideShow.Status = slideShow.Status;
            currentSlideShow.Title = slideShow.Title;
            currentSlideShow.Description = slideShow.Description;
            if(photo != null && !string.IsNullOrEmpty(photo.FileName))
            {
                //source code for uploading an image
                var path = Path.Combine(this._env.WebRootPath, "SlideShows", photo.FileName);
                var stream = new FileStream(path, FileMode.Create);
                photo.CopyToAsync(stream);
                //slideShow.Name = photo.FileName;    
                //I need the URL of the Image
                currentSlideShow.Photo = photo.FileName;
            }
            _db.SaveChanges();
            return RedirectToAction("Index", "slideshow", new { area = "admin" });
        }
    }
}