using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Context;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        BANPCEntities obj = new BANPCEntities();
        // GET: Admin/Product
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var lstProduct = new List<Product>();
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                lstProduct = obj.Products.Where(n => n.name.Contains(searchString)).ToList();
            }
            else
            {
                lstProduct = obj.Products.ToList();
            }
            ViewBag.CurrentFilter = searchString;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            lstProduct = lstProduct.OrderByDescending(n => n.id).ToList();
            return View(lstProduct);
        }

        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (objProduct.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                        string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                        fileName = fileName + extension + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss"));
                        objProduct.avatar = fileName;
                        objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Controllers/images/"), fileName));
                    }
                 
                    obj.Products.Add(objProduct);
                    obj.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(objProduct);
                }
            }
            return View(objProduct);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var objProduct = obj.Products.Where(n => n.id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objProduct = obj.Products.Where(n => n.id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            var objProduct = obj.Products.Where(n => n.id == objPro.id).FirstOrDefault();
            obj.Products.Remove(objProduct);
            obj.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            var objProduct = obj.Products.Where(n => n.id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product objProduct, FormCollection form)
        {
            if (objProduct.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                fileName = fileName + extension;
                objProduct.avatar = fileName;
                objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
            }
            else
            {
                objProduct.avatar = form["oldimage"];
                obj.Entry(objProduct).State = EntityState.Modified;
                obj.SaveChanges();
                return RedirectToAction("Index");
            }
            obj.Entry(objProduct).State = EntityState.Modified;
            obj.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}