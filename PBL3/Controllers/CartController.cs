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
            ViewBag.Total = new CartBLL().getTotal(CartID, true);
            return View(new CartBLL().GetProductCart(CartID));
        }
        [HttpPost]
        [HasLogin(Role = "USER", ContentType = "json")]
        public ActionResult Add(int productID, int quantity)
        {
            int CartID = Convert.ToInt32(Session["USER"]);
            bool result = new CartBLL().save(productID, quantity, CartID);
            if (result)
            {
                return new JsonResult {
                    Data = new {
                        status = true,
                        message = "Thêm sản phẩm vào giỏ hàng thành công",
                        total = new CartBLL().getTotal(Convert.ToInt32(Session["USER"]), true)
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
                        message = "Thêm sản phẩm vào giỏ hàng thất bại"
                    }
                };
            }
        }
        [HasLogin(Role = "USER")]
        public ActionResult Delete(int id)
        {
            CartBLL cartDAO = new CartBLL();
            int CartID = Convert.ToInt32(Session["USER"]);
            if (cartDAO.DeleteProduct(id, CartID) == true )
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = true,
                        message = "Xóa thành công sản phẩm trong giỏ hàng",
                        total = new CartBLL().getTotal(Convert.ToInt32(Session["USER"]), true)
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
            CartBLL cartDAO = new CartBLL();
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
                    status = new CartBLL().select(id, CartID),
                    total = new CartBLL().getTotal(Convert.ToInt32(Session["USER"]), true)
                }
            };
        }
        [HasLogin(Role = "USER")]
        public ActionResult selectAll()
        {
            CartBLL cartDAO = new CartBLL();
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