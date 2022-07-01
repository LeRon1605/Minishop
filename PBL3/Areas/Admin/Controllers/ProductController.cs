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
        public ActionResult Index(string minValue, string maxValue, int page = 1, bool? state = null, string keyword = "", string CategoryID = "All")
        {
            ProductBUS productBUS = new ProductBUS();
            int totalPage = 0;
            ViewBag.products = productBUS.getPage(page, 10, state, keyword, CategoryID, minValue, maxValue, out totalPage);
            ViewBag.Total = productBUS.Count();
            ViewBag.categories = new CategoryBUS().findAll();
            ViewBag.pagingData = new PagingModel
            {
                CountPages = totalPage,
                CurrentPage = page,
                GenerateURL = (int pageNum) => $"?page={pageNum}&state={state}&keyword={keyword}&CategoryID={CategoryID}&minValue={minValue}&maxValue={maxValue}"
            };
            return View();
        }
        [HttpGet]
        public ActionResult View(int id, bool isEdit = false)
        {
            ViewBag.isEdit = isEdit;
            ViewBag.categories = new CategoryBUS().findAll();
            ViewBag.importBills = new ImportBillBUS().getBillOfProduct(id);
            Product product = new ProductBUS().find(id, true);
            if (product == null)
            {
                return HttpNotFound();
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
                        product.Image = $"/public/uploads/products/{fileName}";
                        new ProductBUS().Add(product);
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
                TempData["AddDetail"] = ModelState.Values.SelectMany(v => v.Errors).ToList()[0].ErrorMessage;
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Update(Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/public/uploads/products"), fileName);
                    file.SaveAs(path);
                    product.Image = $"/public/uploads/products/{fileName}";
                }
                if (new ProductBUS().Update(product))
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

        [HttpPost]
        public ActionResult Find(int id)
        {
            Product product = new ProductBUS().find(id);
            if (product != null)
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = true,
                        data = product
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
                        message = "Sản phẩm không tồn tại"
                    }
                };
            }
        }

        public ActionResult Import(int productID, int quantity, int totalPrice)
        {
            if (ModelState.IsValid)
            {
                if (new ProductBUS().exist(productID) && quantity > 0 && totalPrice > 0)
                {
                    ImportBill bill = new ImportBill
                    {
                        TotalPrice = totalPrice,
                        ImportBillDetails = new List<ImportBillDetail> { new ImportBillDetail { ProductID = productID, Quantity = quantity } }
                    };
                    new ImportBillBUS().Add(bill);
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
                            detail = "Dữ liệu không hợp lệ"
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
            if (new ProductBUS().Delete(id))
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