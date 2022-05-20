using EF.DAO;
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
            ViewBag.Total = new CartDAO().getTotal(CartID, true);
            return View(new CartDAO().GetProductCart(CartID));
        }
        [HttpPost]
        [HasLogin(Role = "USER", ContentType = "json")]
        public ActionResult Add(int productID, int quantity)
        {
            int CartID = Convert.ToInt32(Session["USER"]);
            bool result = new CartDAO().add_Update(productID, quantity, CartID);
            if (result)
            {
                return new JsonResult {
                    Data = new {
                        status = true,
                        message = "Thêm sản phẩm vào giỏ hàng thành công"
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
            CartDAO cartDAO = new CartDAO();
            if (cartDAO.DeleteProduct(id) == true)
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = true,
                        message = "Xóa thành công sản phẩm trong giỏ hàng"
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
        public ActionResult Select(int id)
        {
            return new JsonResult
            {
                Data = new
                {
                    status = new CartDAO().select(id)
                }
            };
        }
    }
}