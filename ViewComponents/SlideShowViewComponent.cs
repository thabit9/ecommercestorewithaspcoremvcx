using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommercestorewithaspcoremvc.DataAccess;
using ecommercestorewithaspcoremvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace ecommercestorewithaspcoremvc.ViewComponents
{
    [ViewComponent(Name ="SlideShow")]
    public class SlideShowViewComponent : ViewComponent
    {
        private readonly EcommerceContext _db;

        public SlideShowViewComponent(EcommerceContext db)
        {
            this._db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<SlideShow> slideShow = _db.SlideShows.Where(c => c.Status).ToList();
            return View("Index", slideShow);
        }
    }
}