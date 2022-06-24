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
            int CartID = Convert.ToInt32(Session["USER"]);
            ViewBag.Total = new CartBUS().getTotal(CartID, true);
            return View(new CartBUS().GetProductCart(CartID));
        }
        [HttpPost]
        [HasLogin(Role = "USER", ContentType = "json")]
        public ActionResult Add(int productID, int quantity)
        {
            int CartID = Convert.ToInt32(Session["USER"]);
            bool result = new CartBUS().save(productID, quantity, CartID);
            if (result)
            {
                return new JsonResult {
                    Data = new {
                        status = true,
                        message = "Thêm sản phẩm vào giỏ hàng thành công",
                        total = new CartBUS().getTotal(Convert.ToInt32(Session["USER"]), true)
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
        [HasLogin(Role = "USER")]
        public ActionResult Delete(int id)
        {
            CartBUS cartDAO = new CartBUS();
            int CartID = Convert.ToInt32(Session["USER"]);
            if (cartDAO.DeleteProduct(id, CartID) == true )
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = true,
                        message = "Xóa thành công sản phẩm trong giỏ hàng",
                        total = new CartBUS().getTotal(Convert.ToInt32(Session["USER"]), true)
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
            CartBUS cartDAO = new CartBUS();
            int CartID = Convert.ToInt32(Session["USER"]);
            if(cartDAO.removeAll(CartID) == true)
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
            int CartID = Convert.ToInt32(Session["USER"]);
            return new JsonResult
            {
                Data = new
                {
                    status = new CartBUS().select(id, CartID),
                    total = new CartBUS().getTotal(Convert.ToInt32(Session["USER"]), true)
                }
            };
        }
        [HasLogin(Role = "USER")]
        public ActionResult selectAll()
        {
            CartBUS cartDAO = new CartBUS();
            int CartID = Convert.ToInt32(Session["USER"]);
            if (cartDAO.selectAll(CartID) == true)
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