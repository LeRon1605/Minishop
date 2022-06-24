using Models.BLL;
using Models.DTO;
using PBL3.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        [HasLogin(Role = "USER")]
        public ActionResult Index()
        {
            CartBUS cartBUS = new CartBUS();
            ViewBag.Total = cartBUS.getTotal((int)Session["USER"], true);
            return View(cartBUS.GetProductCart((int)Session["USER"]));
        }
        [HttpPost]
        [HasLogin(Role = "USER", ContentType = "json")]
        public ActionResult Add(int productID, int quantity)
        {
            CartBUS cartBUS = new CartBUS();
            bool result = cartBUS.save(productID, quantity, (int)Session["USER"]);
            if (result)
            {
                return new JsonResult {
                    Data = new {
                        status = true,
                        message = "Thêm sản phẩm vào giỏ hàng thành công",
                        total = cartBUS.getTotal((int)Session["USER"], true)
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
                        message = "Số lượng trong kho không đủ"
                    }
                };
            }
        }
        [HttpPost]
        [HasLogin(Role = "USER")]
        public ActionResult Delete(int id)
        {
            CartBUS cartBUS = new CartBUS();
            if (cartBUS.DeleteProduct(id, (int)Session["USER"]))
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = true,
                        message = "Xóa thành công sản phẩm trong giỏ hàng",
                        total = cartBUS.getTotal((int)Session["USER"], true)
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
                        message = "Xóa thất bại sản phẩm trong giỏ hàng"
                    }
                };
            }
        }
        [HasLogin(Role = "USER")]
        public ActionResult removeAll()
        {
            CartBUS cartBUS = new CartBUS();
            if(cartBUS.removeAll((int)Session["USER"]))
            {
                return RedirectToAction("Index");   
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HasLogin(Role = "USER")]
        public ActionResult Select(int id)
        {
            CartBUS cartBUS = new CartBUS();
            return new JsonResult
            {
                Data = new
                {
                    status = cartBUS.select(id, (int)Session["USER"]),
                    total = cartBUS.getTotal((int)Session["USER"], true)
                }
            };
        }
        [HasLogin(Role = "USER")]
        public ActionResult selectAll()
        {
            if (new CartBUS().selectAll((int)Session["USER"]))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}