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
        public ActionResult Index(int page = 1, int stateID = 0, string keyword = "")
        {
            int countPages = 0;
            List<Order> orders = new OrderBLL().getPage(page, 10, keyword, stateID, out countPages);
            ViewBag.states = new StateBLL().findAll();
            ViewBag.pagingModel = new PagingModel
            {
                CountPages = countPages,
                CurrentPage = page,
                GenerateURL = (pageNum) => $"?page={pageNum}&stateID={stateID}&keyword={keyword}"
            };
            return View(orders);
        }
        [HttpPost]
        public ActionResult Confirm(int ID)
        {
            if (new OrderBLL().confirm(ID))
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = true,
                        message = "Xác nhận đơn hàng thành công"
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
                        message = "Xác nhận đơn hàng thất bại"
                    }
                };
            }
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
    }
}