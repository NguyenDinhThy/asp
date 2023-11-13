using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;

namespace WebApplication1.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        BANPCEntities objBANPCEntities1 = new BANPCEntities();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
            {
            //POST: Register
           
                
                
                {
                    if (ModelState.IsValid)
                    {
                        var check = objBANPCEntities1.Users.FirstOrDefault(s => s.Email == _user.Email);
                        if (check == null)
                        {
                            objBANPCEntities1.Configuration.ValidateOnSaveEnabled = false;
                            objBANPCEntities1.Users.Add(_user);
                            objBANPCEntities1.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.error = "Email already exists";
                            return View();
                        }


                    }
                    return View();

                }

            


            
        }
    }
}