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
            StatisticsBUS statisticsBUS = new StatisticsBUS();
            ViewBag.statistics = statisticsBUS.GetStatistics(DateTime.Now.AddDays(-7), DateTime.Now, "dd/MM");
            ViewBag.statisticsInDay = statisticsBUS.GetStatisticsInDay(DateTime.Now, "dd/MM");
            ViewBag.totalOrder = new OrderBUS().Count();
            ViewBag.totalBenifit = statisticsBUS.getTotalBenifit();
            return View();
        }

        public ActionResult GetStatisticsRows(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null) startDate = DateTime.Now;
            if (endDate == null) endDate = DateTime.Now;
            return PartialView("_RowStatistics", new StatisticsBUS().GetStatistics((DateTime)startDate, (DateTime)endDate, "dd/MM")); 
        }

        public ActionResult GetStatistics(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null) startDate = DateTime.Now;
            if (endDate == null) endDate = DateTime.Now;
            return new JsonResult {
                Data = new
                {
                    data = new StatisticsBUS().GetStatistics((DateTime)startDate, (DateTime)endDate, "dd/MM").Select(x => new
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