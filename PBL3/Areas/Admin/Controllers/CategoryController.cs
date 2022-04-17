using EF.DAO;
using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PBL3.Models;

namespace PBL3.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index(int page = 1, string keyword = "")
        {
            CategoryDAO categoryDAO = new CategoryDAO();
            int totalPage = 0;
            ViewBag.categories = categoryDAO.getPage(page, 10, keyword, out totalPage);
            ViewBag.Total = categoryDAO.Count();
            ViewBag.pagingData = new PagingModel
            {
                CountPages = totalPage,
                CurrentPage = page,
                GenerateURL = (int pageNum) => $"?page={pageNum}&keyword={keyword}"
            };
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
                CategoryDAO categoryDAO = new CategoryDAO();
                categoryDAO.Add(category);
                TempData["Addstatus"] = true;
            }
            else
            {
                TempData["Addstatus"] = false;
                TempData["AddDetail"] = "Thêm thất bại";
            }
            return RedirectToAction("Index");
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