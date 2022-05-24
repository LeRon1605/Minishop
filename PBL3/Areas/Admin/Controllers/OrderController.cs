using Models.DTO;
using Models.BLL;
using PBL3.Helper;
using PBL3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Areas.Admin.Controllers
{
    [HasLogin(Role = "ADMIN")]
    public class OrderController : Controller
    {
        // GET: Admin/Order
        public ActionResult Index(int page = 1, int stateID = 0, string keyword = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            int countPages = 0;
            if (startDate == null) startDate = DateTime.MinValue;
            if (endDate == null) endDate = DateTime.Now;
            List<Order> orders = new OrderBLL().getPage(page, 10, keyword, stateID, out countPages, startDate, endDate);
            ViewBag.states = new StateBLL().findAll();
            ViewBag.pagingModel = new PagingModel
            {
                CountPages = countPages,
                CurrentPage = page,
                GenerateURL = (pageNum) => $"?page={pageNum}&stateID={stateID}&keyword={keyword}"
            };
            return View(orders);
        }
        [HttpGet]
        public ActionResult View(int ID)
        {
            Order order = new OrderBLL().find(ID);
            if (order == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(order);
            }
        }
        [HttpPost]
        public ActionResult Decline(int ID)
        {
            if (new OrderBLL().decline(ID))
            {
                TempData["Message"] = "Từ chối đơn hàng thành công";
                TempData["Status"] = true;
            }
            else
            {
                TempData["Message"] = "Từ chối đơn hàng thất bại";
                TempData["Status"] = false;
            }
            return RedirectToAction("View", new { id = ID });
        }
        [HttpPost]
        public ActionResult Confirm(int id)
        {
            if (new OrderBLL().confirm(id))
            {
                TempData["Message"] = "Xác nhận đơn hàng thành công";
                TempData["Status"] = true;
            }
            else
            {
                TempData["Message"] = "Xác nhận đơn hàng thất bại";
                TempData["Status"] = false;
            }
            return RedirectToAction("View", new { id = id });
        }
        [HttpPost]
        public ActionResult Deliver(int ID)
        {
            if (new OrderBLL().deliver(ID))
            {
                TempData["Message"] = "Xác nhận giao hàng thành công";
                TempData["Status"] = true;
            }
            else
            {
                TempData["Message"] = "Không thể xác nhận giao hàng";
                TempData["Status"] = false;
            }
            return RedirectToAction("View", new { id = ID });
        }
        [HttpPost]
        public ActionResult DeclineDeliver(int ID)
        {
            if (new OrderBLL().declineDeliver(ID))
            {
                TempData["Message"] = "Đã xác nhận giao hàng thất bại";
                TempData["Status"] = true;
            }
            else
            {
                TempData["Message"] = "Không thể xác nhận giao hàng thất bại";
                TempData["Status"] = false;
            }
            return RedirectToAction("View", new { id = ID });
        }
        [HttpPost]
        public ActionResult ConfirmDeliver(int ID)
        {
            if (new OrderBLL().confirmDeliver(ID))
            {
                TempData["Message"] = "Xác nhận đã giao hàng thành công";
                TempData["Status"] = true;
            }
            else
            {
                TempData["Message"] = "Xác nhận đã giao hàng thất bại";
                TempData["Status"] = false;
            }
            return RedirectToAction("View", new { id = ID });
        }
    }
}