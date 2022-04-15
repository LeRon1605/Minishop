using EF.DAO;
using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ShopOnlineDbContext context = new ShopOnlineDbContext();
            //context.Database.EnsureCreated();
            List<Product> products = new ProductDAO().findAll();
            ViewBag.products = products;
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