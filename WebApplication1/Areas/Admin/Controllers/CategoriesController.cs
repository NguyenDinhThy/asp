using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        BANPCEntities obj = new BANPCEntities();
        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(obj.Categories.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category objCategory)
        {
            try
            {
                if (objCategory.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                    string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                    fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                    objCategory.Avatar = fileName;
                    objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Controllers/images/"), fileName));
                }
                obj.Categories.Add(objCategory);
                obj.SaveChanges();
                return RedirectToAction("Index");

            }

            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }

        public ActionResult Delete(int Id)
        {
            var kh = obj.Categories.FirstOrDefault(p => p.Id == Id);
            obj.Categories.Remove(kh);
            obj.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult Details()
        {
            var objCategory = obj.Categories.FirstOrDefault();
            return View(objCategory);
        }


        [HttpGet]
        public ActionResult Edit()
        {
            var objCategory = obj.Categories.FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Edit(Category objCategory)
        {

            if (objCategory.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objCategory.Avatar = fileName;
                objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
            }
            obj.Entry(objCategory).State = System.Data.Entity.EntityState.Modified;
            obj.SaveChanges();
            return RedirectToAction("Index");



        }
    }
}