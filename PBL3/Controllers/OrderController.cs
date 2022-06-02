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
            Order order = new OrderBLL().find(id);
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
            List<CartProduct> item = new CartBLL().GetProductCart((int)Session["USER"], true);
            if (item.Count > 0)
            {
                ViewBag.Items = item;
                ViewBag.Total = new CartBLL().getTotal((int)Session["USER"], true);
                return View();
            }
            else
            {
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
                int orderID = new OrderBLL().add(order, (int)Session["USER"], out message);
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
            if (new OrderBLL().cancel(id, (int)Session["USER"]))
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
            if (new OrderBLL().confirmReceived(id, (int)Session["USER"]))
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
            if (new OrderBLL().declineReceived(id, (int)Session["USER"]))
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
        public ActionResult Comment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                if (new CommentBLL().add((int)Session["USER"], comment))
                {
                    return new JsonResult
                    {
                        Data = new
                        {
                            status = true,
                            message = "Đánh giá sản phẩm thành công"
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
                            message = "Đánh giá sản phẩm thất bại"
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
                        message = "Đánh giá sản phẩm thất bại",
                        detail = "Dữ liệu không hợp lệ"
                    }
                };
            }
        }    

    }
}