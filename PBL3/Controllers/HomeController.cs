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
            ViewBag.products = productDAO.getPage(page, 20, "", "All", "All", out totalPage);
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
            List<Product> products = new List<Product>();
            List<Category> categories = new List<Category>();
            context.Roles.Add(new Role
            {
                Name = "USER",
                Description = "Người dùng"
            });
            Role adminRole = new Role
            {
                Name = "ADMIN",
                Description = "Quản trị viên"
            };
            context.Roles.Add(adminRole);
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
                Role = adminRole
            });
            for (int i = 1;i <= 10;i++)
            {
                categories.Add(new Category
                {
                    Name = $"Loại sản phẩm {i}",
                    Description = $"Mô tả cho loại sản phẩm, được viết ở đây"
                });
            }
            context.Categories.AddRange(categories);
            for (int i = 1;i <= 500;i++)
            {
                products.Add(new Product
                {
                    Name = $"Sản phẩm {i}",
                    Description = $"Mô tả cho sản phẩm, được viết ở đây",
                    Power = 3,
                    MaintenanceTime = 3,
                    Producer = "Nhà sản xuất",
                    ProducerDate = DateTime.Now,
                    CategoryID = (i % 10) + 1,
                    Price = new Random().Next(1000, 9999) * 30000 / i,
                    Image = "/public/uploads/products/agridrone.png",
                    CreatedAt = DateTime.Now,
                    Stock = new Random().Next(3, 50),
                    Mass = 4
                });
            }
            context.Products.AddRange(products);
            context.SaveChanges();
            return RedirectToAction("Index");
        }    
    }
}