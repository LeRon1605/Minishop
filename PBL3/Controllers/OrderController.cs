using EF.Models;
using EF.DAO;
using PBL3.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Controllers
{
    [HasLogin]
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index(int id)
        {           
            Order order = new OrderDAO().find(id);
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
                    return View(order);
                }
            }
        }
        [HttpGet]
        public ActionResult Add([Bind(Include = "ProductID,Quantity,Price")] List<ProductOrder> item)
        {
            return View(item);
        }
        [HttpPost]
        public ActionResult Add([Bind(Include = "ProductID,Quantity")] List<ProductOrder> item, string receiverAddress)
        {
            if (ModelState.IsValid)
            {
                string message;
                int orderID = new OrderDAO().add(item, (int)Session["USER"], receiverAddress, out message);
                if (orderID != -1)
                {
                    return RedirectToAction("Index", new { id = orderID });
                }
                else
                {
                    TempData["Message"] = message;
                    return View();
                }
            }
            else
            {
                TempData["Message"] = "Dữ liệu không hợp lệ";
                return View();
            }
        }

    }
}