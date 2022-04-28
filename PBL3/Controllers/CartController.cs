using EF.DAO;
using PBL3.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Controllers
{
    [HasLogin(Role = "USER")]
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
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
    }
}