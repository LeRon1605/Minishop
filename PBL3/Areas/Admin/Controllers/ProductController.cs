using EF.Models;
using EF.DAO;
using PBL3.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public ActionResult Index()
        {
            List<Product> products = new ProductDAO().findAll();
            foreach (Product product in products)
            {
                product.Category = new CategoryDAO().find(product.CategoryID);
            }
            ViewBag.products = products;
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            List<Category> categories = new CategoryDAO().findAll();
            ViewBag.categories = categories;
            return View();
        }
        [HttpPost]
        public ActionResult Add(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = model.Product;
                product.Detail = model.Detail;

                TempData["Message"] = "Thêm thành công";
                new ProductDAO().Add(product);
            }    

            return RedirectToAction("Add");
        }
    }
}