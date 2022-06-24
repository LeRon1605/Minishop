using Models.BLL;
using Models.DTO;
using PBL3.Helper;
using PBL3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Areas.Admin.Controllers
{
    [HasLogin(Role = "ADMIN")]
    public class ProfileController : Controller
    {
        // GET: Admin/Profile
        
        public ActionResult Index()
        {
            return View(new UserBUS().find((int)Session["USER"]));
        }
        public ActionResult Update([Bind(Exclude ="Password")]User user, HttpPostedFileBase file)
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
                bool users = new UserBUS().Update(user);
                if (users)
                {
                    TempData["Status"] = true;
                    TempData["Message"] = "Cập nhật tài khoản công";
                }
                else
                {
                    TempData["Status"] = false;
                    TempData["Message"] = "Cập nhật tài thất bại";
                }
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Dữ liệu không hợp lệ";
            }
            return RedirectToAction("Index");
        }
    }
}