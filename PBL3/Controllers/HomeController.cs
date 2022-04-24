using EF.DAO;
using EF.Models;
using PBL3.Helper;
using PBL3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int page = 1)
        {
            List<Category> categories = new CategoryDAO().findAll();
            ProductDAO productDAO = new ProductDAO();
            int totalPage = 0;
            ViewBag.Total = productDAO.Count();
            ViewBag.categories = new CategoryDAO().findAll();
            ViewBag.products = productDAO.getPage(page, 10, "", "All", "All", out totalPage);
            ViewBag.lastedProduct = productDAO.getLasted(5);
            ViewBag.pagingData = new PagingModel
            {
                CountPages = totalPage,
                CurrentPage = page,
                GenerateURL = (int pageNum) => $"?page={pageNum}"
            };
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Reload()
        {
            ShopOnlineDbContext context = new ShopOnlineDbContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Roles.Add(new Role
            {
                Name = "USER",
                Description = "Người dùng"
            });
            context.Roles.Add(new Role
            {
                Name = "ADMIN",
                Description = "Quản trị viên"
            });
            context.Users.Add(new User
            {
                Name = "Admin",
                Email = "admin",
                Phone = "0905857760",
                Password = Encryptor.MD5Hash("admin"),
                Image = "~/public/images/Default.jpg",
                Address = "83 Bà Triệu, Thành Phố Huế",
                Birth = new DateTime(2022, 3, 15),
                Gender = "Nam",
                RoleID = 2
            });
            context.SaveChanges();
            return RedirectToAction("Index");
        }    
    }
}