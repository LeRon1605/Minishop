using EF.DAO;
using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                new CategoryDAO().Add(category);
                TempData["Message"] = "Thêm thành công";
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            Category category = new CategoryDAO().find(id);
            return View(category);
        }
        public ActionResult Update(Category category)
        {
            if (ModelState.IsValid)
            {
                new CategoryDAO().Update(category);
            }
            return RedirectToAction("Update");
        }
    }
}