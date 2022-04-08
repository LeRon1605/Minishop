using EF.Models;
using EF.DAO;
using PBL3.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

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
        public ActionResult Add(Product product, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(Image.FileName);
                        string path = Path.Combine(Server.MapPath("~/public/uploads/products"), fileName);
                        Image.SaveAs(path);
                        ProductDAO productDAO = new ProductDAO();
                        product.Image = path;
                        productDAO.Add(product);
                        TempData["notification"] = "Thêm thành công";
                    }
                    else
                    {
                        // return View
                        /*
                        return new JsonResult
                        {
                            Data = new
                            {
                                status = false,
                                message = "Thêm thất bại",
                                detail = "Dữ liệu không hợp lệ"
                            }
                        };
                        */
                    }
                }
                catch
                {
                    /*
                    return new JsonResult
                    {
                        Data = new
                        {
                            status = false,
                            message = "Thêm thất bại",
                            detail = "Tải file thất bại"
                        }
                    };
                    */
                }
            }
            else
            {
                /*
                return new JsonResult
                {
                    Data = new
                    {
                        status = false,
                        message = "Thêm thất bại",
                        detail = "Dữ liệu không hợp lệ"
                    }
                };
                */
            }
            return View("Index");
        }
    }
}