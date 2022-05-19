using EF.DAO;
using EF.Models;
using PBL3.Helper;
using PBL3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Areas.Admin.Controllers
{
    [HasLogin(Role = "ADMIN")]
    public class UserController: Controller
    {
        public ActionResult Index(int page = 1, string keyword = "")
        {
            int countPages = 0;
            List<User> list = new UserDAO().getPage(page, 10, keyword, out countPages);
            ViewBag.PagingData = new PagingModel
            {
                CountPages = countPages,
                CurrentPage = page,
                GenerateURL = (pageNum) => $"/?page={pageNum}&keyword={keyword}"
            };
            return View(list);
        }
        public ActionResult View(int id)
        {
            return View(new UserDAO().find(id));
        }
        public ActionResult Delete(int id)
        {
            if(new UserDAO().Delete(id))
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
    }
}