using EF.Models;
using EF.DAO;
using PBL3.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PBL3.Models;

namespace PBL3.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public ActionResult Index(int page = 1)
        {
            ProductDAO productDAO = new ProductDAO();
            int totalPage;
            ViewBag.products = productDAO.getPage(page, 10, out totalPage);
            ViewBag.categories = new CategoryDAO().findAll();
            ViewBag.pagingData = new PagingModel
            {
                CountPages = totalPage,
                CurrentPage = page,
                GenerateURL = (int pageNum) => $"?page={pageNum}"
            };
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
                        return new JsonResult
                        {
                            Data = new
                            {
                                status = true,
                                message = "Thêm sản phẩm thành công"
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
                                message = "Thêm sản phẩm thất bại",
                                detail = "Dữ liệu không hợp lệ"
                            }
                        };
                    }
                }
                catch
                {                  
                    return new JsonResult
                    {
                        Data = new
                        {
                            status = false,
                            message = "Thêm sản phẩm thất bại",
                            detail = "Tải file thất bại"
                        }
                    };
                }
            }
            else
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = false,
                        message = "Thêm sản phẩm thất bại",
                        detail = "Dữ liệu không hợp lệ"
                    }
                };
            }
        }

        public ActionResult Delete(int id)
        {
            ProductDAO productDAO = new ProductDAO();
            if (productDAO.Delete(id))
            {
                return new JsonResult{
                    Data = new
                    {
                        message = "Xóa thành công sản phẩm",
                        status = true
                    }
                };
            }    
            else
            {
                return new JsonResult
                {
                    Data = new
                    {
                        message = "Xóa thất bại sản phẩm",
                        detail = "Sản phẩm không tồn tại",
                        status = false
                    }
                };
            }    
        }
    }
}