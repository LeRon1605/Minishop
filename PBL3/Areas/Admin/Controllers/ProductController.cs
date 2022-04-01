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
            ViewBag.categories = new CategoryDAO().findAll();
            return View();
        }

        [HttpPost]
        public ActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                new ProductDAO().Add(product);
                return new JsonResult
                {
                    Data = new
                    {
                        status = true,
                        message = "Thêm thành công"
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
                        message = "Thêm thất bại",
                        detail = "Dữ liệu không hợp lệ"
                    }
                };
            }
        }
    }
}