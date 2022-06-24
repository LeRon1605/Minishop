using Models.BLL;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PBL3.Models;
using PBL3.Helper;

namespace PBL3.Areas.Admin.Controllers
{
    [HasLogin(Role = "ADMIN")]
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index(int page = 1, string keyword = "")
        {
            CategoryBUS categoryBUS = new CategoryBUS();
            int totalPage = 0;
            ViewBag.categories = categoryBUS.getPage(page, 10, keyword, out totalPage);
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
            Category category = new CategoryBUS().find((int)id);
            if (category == null)
            {
                TempData["Message"] = "Loại sản phẩm không tồn tại";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.isEdit = isEdit;
                return View(category);
            }
        }
        [HttpPost]
        public ActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                new CategoryBUS().Add(category);
                TempData["Addstatus"] = true;
            }
            else
            {
                TempData["Addstatus"] = false;
                TempData["AddDetail"] = "Thêm thất bại";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Update(Category category)
        {
            if (ModelState.IsValid)
            {
                if(new CategoryBUS().Update(category))
                {
                    TempData["Status"] = true;
                    TempData["Message"] = "Cập nhật loại sản phẩm thành công";
                    return RedirectToAction("View",new {id = category.ID});
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Cập nhật loại sản phẩm thất bại";
                return RedirectToAction("View", new { id = category.ID, isEdit = true });
            } 
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if(new CategoryBUS().Delete(id) == true)
            {
                return new JsonResult
                {
                    Data =  new 
                    {
                         status = true,
                         message = "Xóa thành công loại sản phẩm"
                    }
                };
            } 
            else
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = false,
                        message = "Xóa thất bại loại sản phẩm"
                    }
                };
            }
        }
    }
}