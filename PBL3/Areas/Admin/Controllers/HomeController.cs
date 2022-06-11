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
    public class HomeController : Controller
    {
        // GET: Admin/Home
        [HttpGet]
        public ActionResult Index()
        {
            List<StatisticsModel> list = new StatisticsBO().GetStatistics(DateTime.Now.AddDays(-6), DateTime.Now, "dd/MM");
            ViewBag.statistics = list;
            ViewBag.statisticsInDay = new StatisticsBO().GetStatisticsInDay(DateTime.Now, "dd/MM");
            ViewBag.totalOrder = new OrderBO().Count();
            ViewBag.totalBenifit = new StatisticsBO().getTotalBenifit();
            ViewBag.totalUser = new UserBO().Count();
            ViewBag.totalActivatedUser = new UserBO().Count(true);
            return View();
        }

        public ActionResult GetStatisticsRows(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null) startDate = DateTime.Now;
            if (endDate == null) endDate = DateTime.Now;
            return PartialView("_RowStatistics", new StatisticsBO().GetStatistics((DateTime)startDate, (DateTime)endDate, "dd/MM")); 
        }

        public ActionResult GetStatistics(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null) startDate = DateTime.Now;
            if (endDate == null) endDate = DateTime.Now;
            return new JsonResult {
                Data = new
                {
                    data = new StatisticsBO().GetStatistics((DateTime)startDate, (DateTime)endDate, "dd/MM").Select(x => new
                    {
                        Date = x.Date.ToString(x.Format),
                        OrderCount = x.OrderCount,
                        Benifit = x.Benifit,
                        Revenue = x.Revenue,
                        NewUser = x.NewUser,
                        OrderInProcess = x.OrderInProcess,
                        OrderCancelCount = x.OrderCancelCount
                    })
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}