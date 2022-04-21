using EF.DAO;
using EF.Models;
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
            //ShopOnlineDbContext context = new ShopOnlineDbContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
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
    }
}