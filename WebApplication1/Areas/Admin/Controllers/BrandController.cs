using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        BANPCEntities obj = new BANPCEntities();
        // GET: Admin/Brand
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstBrand = new List<Brand>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstBrand = obj.Brands.Where(n => n.Name.Contains(SearchString)).ToList();

                //lstBrand = obj.Brands.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstBrand = obj.Brands.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstBrand = lstBrand.OrderByDescending(n => n.Id).ToList();
            return View(lstBrand);
        }
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }

        [ValidateInput(false)]

        [HttpPost]
        public ActionResult Create(Brand objBrand)

        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (objBrand.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                        //ten hinh
                        string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                        //jpg
                        fileName = fileName + extension;
                        //ten hinh.jpg
                        objBrand.Avatar = fileName;
                        objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Controller/images/"), fileName));
                    }
                    objBrand.CreatedOnUtc = DateTime.Now;
                    obj.Brands.Add(objBrand);
                    obj.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(objBrand);

                }

            }

            return View(objBrand);
        }


        [HttpGet]
        public ActionResult Details(int id)
        {
            var objBrand = obj.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objBrand = obj.Brands.Where(n => n.Id == id).FirstOrDefault();

            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Delete(Brand objPro)
        {
            var objBrand = obj.Brands.Where(n => n.Id == objPro.Id).FirstOrDefault();
            obj.Brands.Remove(objBrand);
            obj.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            var objBrand = obj.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brand objBrand, FormCollection form)
        {

            if (objBrand.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                //tenhinh
                string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                //png
                fileName = fileName + extension;
                //tenhinh.png
                objBrand.Avatar = fileName;
                objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Controller/images/"), fileName));

            }
            else
            {
                objBrand.Avatar = form["oldimage"];
                obj.Entry(objBrand).State = EntityState.Modified;
                obj.SaveChanges();
                return RedirectToAction("Index");
            }
            obj.Entry(objBrand).State = EntityState.Modified;
            obj.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}