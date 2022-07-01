using Models.DTO;
using Models.BLL;
using PBL3.Helper;
using PBL3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace PBL3.Areas.Admin.Controllers
{
    [HasLogin(Role = "ADMIN")]
    public class UserController : Controller
    {
        public ActionResult Index(int page = 1, string keyword = "")
        {
            int countPages = 0;
            List<User> list = new UserBUS().getPage(page, 10, keyword, out countPages);
            ViewBag.PagingData = new PagingModel
            {
                CountPages = countPages,
                CurrentPage = page,
                GenerateURL = (pageNum) => $"?page={pageNum}&keyword={keyword}"
            };
            return View(list);
        }
        public ActionResult View(int id)
        {
            User user = new UserBUS().find(id);
            if (user == null) return HttpNotFound();
            return View(user);
        }
        [HttpPost]
        public ActionResult Update(User user, HttpPostedFileBase file)
        {
            if (ModelState["Password"] != null) ModelState["Password"].Errors.Clear();
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/public/uploads/users"), fileName);
                    file.SaveAs(path);
                    user.Image = $"/public/uploads/users/{fileName}";
                }
                if (new UserBUS().Update(user))
                {
                    TempData["Status"] = true;
                    TempData["Message"] = "Cập nhật tài khoản công";
                }
                else
                {
                    TempData["Status"] = false;
                    TempData["Message"] = "Cập nhật tài khoản thất bại";
                }
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Dữ liệu không hợp lệ";
            }
            return RedirectToAction("View", new { id = user.ID });
        }
        public ActionResult Delete(int id)
        {
            if (new UserBUS().Delete(id))
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = true,
                        message = "Xóa thành công tài khoản"
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
                        message = "Xóa thất bại tài khoản"
                    }
                };
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model, int userID)
        {
            if (ModelState["OldPassword"] != null) ModelState["OldPassword"].Errors.Clear();
            if (ModelState.IsValid)
            {
                bool result = new UserBUS().ChangePassword(model.NewPassword, userID);
                if (result)
                {
                    return new JsonResult
                    {
                        Data = new
                        {
                            status = true,
                            message = "Thay đổi mật khẩu thành công"
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
                            message = "Thay đổi mật khẩu thất bại",
                            detail = "Mật khẩu không chính xác"
                        }
                    };
                }
            }
            else
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = false,
                        message = "Thay đổi mật khẩu thất bại",
                        detail = ModelState.Values.SelectMany(v => v.Errors).ToList()[0].ErrorMessage
                    }
                };
            }
        }
    }
}