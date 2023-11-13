using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        BANPCEntities obj = new BANPCEntities();
        // GET: Product
        public ActionResult Detail(int Id)
        {
            var objProduct = obj.Products.Where(n => n.id == Id).FirstOrDefault();
            return View(objProduct);
        }
    }
}