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
    public class ProductController : Controller
    {
        ShopOnlineDbContext context = new ShopOnlineDbContext();
        // GET: Product
        public ActionResult Index(int id)
        {
            Product product = new ProductDAO().find(id);
            if(product == null)
            {
                return RedirectToAction("Index", new
                {
                    controller = "Home"
                });
            }
            else
            {
                return View(product);
            }
        }
        public ActionResult Search(string keyword = "", string categoryID = "All", string price = "All", int page = 1)
        {
            ProductDAO productDAO = new ProductDAO();
            int totalPage = 0;
            List<Product> products = productDAO.getPage(page, 20, keyword, categoryID, price, out totalPage);
            ViewBag.categories = new CategoryDAO().findAll();
            ViewBag.pagingData = new PagingModel
            {
                CountPages = totalPage,
                CurrentPage = page,
                GenerateURL = (int pageNum) => $"?page={pageNum}&keyword={keyword}&CategoryID={categoryID}&Price={price}"
            };
            return View(products);
        }
    }
}