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
        public ActionResult View(int? id, bool isEdit = false)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.isEdit = isEdit;
            Category category = new CategoryDAO().find((int)id);
            if (category == null)
            {
                TempData["Message"] = " Loại sản phẩm không tồn tại";
                return RedirectToAction("Index");
            }
            return View(category);
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
        public ActionResult Delete(int id)
        {
            CategoryDAO categoryDAO =new CategoryDAO();
            if (categoryDAO.Delete(id))
            {
                return new JsonResult
                {
                    Data = new
                    {
                        message = "Xóa thành công loại sản phẩm",
                        status = true
                    }
                };
            }
            else
            {
                return new JsonResult
                {
                    Data = new
                    {
                        message = "Xóa thất bại loại sản phẩm",
                        detail = "Loại sản phẩm không tồn tại",
                        status = false
                    }
                };
            }
        }
    }
}