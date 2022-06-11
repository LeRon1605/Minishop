using PBL3.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.BLL;
using Models.DTO;

namespace PBL3.Controllers
{
    [HasLogin]
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index(int id)
        {           
            Order order = new OrderBO().find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (order.UserID != (int)Session["USER"])
                {
                    ViewBag.Message = "Bạn không có quyền xem giỏ hàng của người khác";
                    return View("~/Views/Shared/UnAuthorize.cshtml");
                }
                else
                {
                    return View("View", order);
                }
            }
        }
        [HttpGet]
        public ActionResult Add()
        {
            List<CartProduct> item = new CartBO().GetProductCart((int)Session["USER"], true);
            if (item.Count > 0)
            {
                foreach (CartProduct product in item)
                {
                    if (product.Quantity > new ProductBO().find(product.ID).Stock)
                    {
                        TempData["Status"] = false;
                        TempData["Message"] = "Số lượng trong kho không đủ.";
                        return RedirectToAction("Index", "Cart");
                    }    
                }    
                ViewBag.Items = item;
                ViewBag.Total = new CartBO().getTotal((int)Session["USER"], true);
                return View();
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Phải chọn ít nhất một sản phẩm";
                return RedirectToAction("Index", "Cart");
            }
        }
        [HttpPost]
        public ActionResult Add(Order order)
        {
            if (ModelState["Voucher.Seri"] != null) ModelState["Voucher.Seri"].Errors.Clear();
            if (ModelState["Voucher.StartDate"] != null) ModelState["Voucher.StartDate"].Errors.Clear();
            if (ModelState.IsValid)
            {
                string message;
                int orderID = new OrderBO().add(order, (int)Session["USER"], out message);
                if (orderID != -1)
                {
                    return RedirectToAction("Index", new { id = orderID });
                }
                else
                {
                    TempData["Message"] = message;
                    return RedirectToAction("Add");
                }
            }
            else
            {
                TempData["Message"] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Add");
            }
        }
        [HttpPost]
        public ActionResult Cancel(int id)
        {
            if (new OrderBO().cancel(id, (int)Session["USER"]))
            {
                TempData["Message"] = "Hủy đơn hàng thành công";
                TempData["Status"] = true;
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Hủy đơn hàng thất bại";
            }
            return RedirectToAction("Index", new { id = id });
        }
        [HttpPost]
        public ActionResult ConfirmReceive(int id)
        {
            if (new OrderBO().confirmReceived(id, (int)Session["USER"]))
            {
                TempData["Message"] = "Xác nhận nhận hàng thành công";
                TempData["Status"] = true;
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Xác nhận nhận hàng thất bại";
            }
            return RedirectToAction("Index", new { id = id });
        }
        [HttpPost]
        public ActionResult DeclineReceive(int id)
        {
            if (new OrderBO().declineReceived(id, (int)Session["USER"]))
            {
                TempData["Message"] = "Đã xác nhận nhận hàng thất bại";
                TempData["Status"] = true;
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Xác nhận không thành công";
            }
            return RedirectToAction("Index", new { id = id });
        }
        [HttpPost]
        public ActionResult OrderDetail(int id)
        {
            ProductOrder productOrder = new OrderBO().getOrderDetail(id);
            if (productOrder == null)
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = false,
                        message = "Not found"
                    },
                };
            }
            else
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = true,
                        data = productOrder
                    }
                };
            }
        }

      

    }
}