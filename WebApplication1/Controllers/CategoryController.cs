using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;

namespace ASP.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Course

        BANPCEntities objBANPCEntities = new BANPCEntities();
        public ActionResult Index()
        {
            var lstCategory = objBANPCEntities.Categories.ToList();
            return View(lstCategory);
        }
        public ActionResult ProductCategory(int Id)
        {
            var listProduct = objBANPCEntities.Products.Where(n => n.CategoryId == Id).ToList();
            return View(listProduct);
        }
    }
}