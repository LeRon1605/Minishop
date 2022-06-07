using Models.BLL;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PBL3.Models;
using PBL3.Helper;

namespace PBL3.Areas.Admin.Controllers
{
    [HasLogin(Role = "ADMIN")]
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public ActionResult Index(int page = 1, string keyword = "", string CategoryID = "All", string Price = "All")
        {
            ProductBO productDAO = new ProductBO();
            int totalPage = 0;
            ViewBag.products = productDAO.getPage(page, 10, keyword, CategoryID, Price, out totalPage);
            ViewBag.Total = productDAO.Count();
            ViewBag.categories = new CategoryBO().findAll();
            ViewBag.pagingData = new PagingModel
            {
                CountPages = totalPage,
                CurrentPage = page,
                GenerateURL = (int pageNum) => $"?page={pageNum}&keyword={keyword}&CategoryID={CategoryID}&Price={Price}"
            };
            return View();
        }
        [HttpGet]
        public ActionResult View(int id, bool isEdit = false)
        {
            CategoryBO categoryDAO = new CategoryBO();
            ProductBO productDAO = new ProductBO();
            ViewBag.isEdit = isEdit;
            ViewBag.categories = categoryDAO.findAll();
            ViewBag.importBills = new ImportBillBO().getBillsOfProduct(id);
            Product product = productDAO.find(id, true);
            if (product == null)
            {
                TempData["Message"] = "Sản phẩm không tồn tại";
                return RedirectToAction("Index");
            }    
            return View(product);
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
                        ProductBO ProductBO = new ProductBO();
                        CategoryBO CategoryBO = new CategoryBO();
                        product.Image = $"/public/uploads/products/{fileName}";
                        ProductBO.Add(product);
                        product.Category = (product.CategoryID == null) ? null : CategoryBO.find((int)product.CategoryID);
                        TempData["AddStatus"] = true;
                    }
                    else
                    {
                        TempData["AddStatus"] = false;
                        TempData["AddDetail"] = "Dữ liệu không hợp lệ";
                    }
                }
                catch
                {
                    TempData["AddStatus"] = false;
                    TempData["AddDetail"] = "Tải thất bại";
                }
            }
            else
            {

                TempData["AddStatus"] = false;
                TempData["AddDetail"] = "Dữ liệu không hợp lệ";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Update(Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                ProductBO productDAO = new ProductBO();
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/public/uploads/products"), fileName);
                    file.SaveAs(path);
                    product.Image = $"/public/uploads/products/{fileName}";
                }
                if (productDAO.Update(product))
                {
                    TempData["Status"] = true;
                    TempData["Message"] = "Cập nhật sản phẩm thành công";
                    return RedirectToAction("View", new { id = product.ID });
                }
                else
                {
                    TempData["Status"] = false;
                    TempData["Message"] = "Cập nhật thất bại";
                    return RedirectToAction("View", new { id = product.ID, isEdit = true });
                }
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Dữ liệu không hợp lệ";
                return RedirectToAction("View", new { id = product.ID, isEdit = true });
            }
        }

        public ActionResult Import(int id, ImportBill bill)
        {
            if (ModelState.IsValid)
            {
                ProductBO productDAO = new ProductBO();
                if (productDAO.import(id, bill))
                {
                    return new JsonResult
                    {
                        Data = new
                        {
                            status = true,
                            message = "Nhập hàng vào kho thành công"
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
                            message = "Nhập hàng vào kho thất bại",
                            detail = "Sản phẩm không tồn tại"
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
                        message = "Nhập hàng vào kho thất bại",
                        detail = ModelState.Values.SelectMany(v => v.Errors).ToList()[0].ErrorMessage
                    }
                };
            }
        }

        public ActionResult Delete(int id)
        {
            ProductBO productDAO = new ProductBO();
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