using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommercestorewithaspcoremvc.DataAccess;
using ecommercestorewithaspcoremvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace ecommercestorewithaspcoremvc.ViewComponents
{
    [ViewComponent(Name ="Search")]
    public class SearchViewComponent : ViewComponent
    {
        private readonly EcommerceContext _db;

        public SearchViewComponent(EcommerceContext db)
        {
            this._db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Category> categories = _db.Categories.Where(c => c.Status && c.parent == null).ToList();
            return View("Index", categories);
        }
    }
}