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
            Order order = new OrderBUS().find(id);
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
            User user = new UserBUS().find((int)Session["USER"]);
            if (user.isActivated)
            {
                List<CartProduct> item = new CartBUS().GetProductCart((int)Session["USER"], true);
                if (item.Count > 0)
                {
                    foreach (CartProduct product in item)
                    {
                        if (product.Quantity > new ProductBUS().find(product.ProductID).Stock)
                        {
                            TempData["Status"] = false;
                            TempData["Message"] = "Số lượng sản phẩm trong kho không đủ.";
                            return RedirectToAction("Index", "Cart");
                        }
                    }
                    ViewBag.Items = item;
                    ViewBag.Total = new CartBUS().getTotal((int)Session["USER"], true);
                    return View();
                }
                else
                {
                    TempData["Status"] = false;
                    TempData["Message"] = "Phải chọn ít nhất một sản phẩm";
                    return RedirectToAction("Index", "Cart");
                }
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Tài khoản chưa kích hoạt";
                return RedirectToAction("Index", "Cart");
            }
        }
        [HttpPost]
        public ActionResult Add(Order order)
        {
            if (ModelState["Voucher.Seri"] != null) ModelState["Voucher.Seri"].Errors.Clear();
            if (ModelState["Voucher.StartDate"] != null) ModelState["Voucher.StartDate"].Errors.Clear();
            if (ModelState["Voucher.Quantity"] != null) ModelState["Voucher.Quantity"].Errors.Clear();
            if (ModelState.IsValid)
            {
                string message;
                int orderID = new OrderBUS().add(order, (int)Session["USER"], out message);
                if (orderID != -1)
                {
                    TempData["Status"] = true;
                    TempData["Message"] = "Đặt hàng thành công";
                    return RedirectToAction("Index", new { id = orderID });
                }
                else
                {
                    TempData["Status"] = false;
                    TempData["Message"] = message;
                    return RedirectToAction("Add");
                }
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Add");
            }
        }
        [HttpPost]
        public ActionResult Cancel(int id)
        {
            if (new OrderBUS().cancel(id, (int)Session["USER"]))
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
            if (new OrderBUS().confirmReceived(id, (int)Session["USER"]))
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
            if (new OrderBUS().declineReceived(id, (int)Session["USER"]))
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
            ProductOrder productOrder = new OrderBUS().getOrderDetail(id);
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