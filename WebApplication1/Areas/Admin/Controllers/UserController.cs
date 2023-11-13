using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        BANPCEntities obj = new BANPCEntities();
        // GET: Admin/User
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstUser = new List<User>();
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
                lstUser = obj.Users.Where(n => n.FirstName.Contains(SearchString)).ToList();
            }
            else
            {
                lstUser = obj.Users.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstUser = lstUser.OrderByDescending(n => n.idUser).ToList();
            return View(lstUser);
        }

        [HttpGet]
        public ActionResult Create()
        {
            //this.LoadData();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User _user)
        {
            //this.LoadData();
            if (ModelState.IsValid)
            {
                var check = obj.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    _user.IsAdmin = false; //IsAdmin = false = người dùng
                    obj.Configuration.ValidateOnSaveEnabled = false;
                    // add user
                    obj.Users.Add(_user);
                    //lưu thông tin lại
                    obj.SaveChanges();
                    // trả về trang chủ
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email đã tồn tại";


                    return View();
                }

            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var Users = obj.Users.Where(n => n.idUser == id).FirstOrDefault();
            return View(Users);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var Users = obj.Users.Where(n => n.idUser == id).FirstOrDefault();
            return View(Users);
        }
        [HttpPost]
        public ActionResult Delete(User objuser)
        {
            var objUser = obj.Users.Where(n => n.idUser == objuser.idUser).FirstOrDefault();
            obj.Users.Remove(objUser);
            obj.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            //this.LoadData();

            var Users = obj.Users.Where(n => n.idUser == id).FirstOrDefault();
            return View(Users);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, User objUser)
        {
            objUser.Password = GetMD5(objUser.Password);
            obj.Entry(objUser).State = EntityState.Modified;
            obj.SaveChanges();

            return RedirectToAction("Index");
        }
        // mã hóa pass word
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;

        }


    }
}