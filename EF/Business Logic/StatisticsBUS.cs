using Microsoft.EntityFrameworkCore;
using Models.DAL;
using Models.DTO;
using PBL3.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BLL
{
    public class StatisticsBUS
    {
        public List<StatisticsModel> GetStatistics(DateTime startDate, DateTime endDate, string format)
        {

            List<StatisticsModel> data = new List<StatisticsModel>();
            for (var date = startDate.Date;date.Date <= endDate.Date;date = date.AddDays(1))
            {
                data.Add(GetStatisticsInDay(date, format));
            }
            return data;
        }
        public StatisticsModel GetStatisticsInDay(DateTime date, string format)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                List<Order> listOrder = context.Orders.Where(order => order.isReceived && order.CreatedAt.Date == date.Date).ToList();
                var listImport = context.ImportBills.Where(bill => bill.CreatedAt.Date == date.Date);
                int orderCount = listOrder.Count();
                int orderCancelCount = context.Orders.Where(order => order.isCancel && order.CreatedAt.Date == date.Date).Count();
                int revenue = (orderCount == 0) ? 0 : listOrder.Sum(o => o.Total - o.Sale);
                int benifit = revenue - (listImport.Count() == 0 ? 0 : listImport.Sum(bill => bill.TotalPrice));
                return new StatisticsModel
                {
                    Format = format,
                    Date = date,
                    NewUser = context.Users.Where(u => u.CreatedAt.Date == date.Date).Count(),
                    Revenue = revenue,
                    Benifit = benifit,
                    OrderCount = orderCount,
                    OrderCancelCount = context.Orders.Where(order => order.isCancel && order.CreatedAt.Date == date.Date).Count(),
                    OrderInProcess = context.Orders.Where(order => order.CreatedAt.Date == date.Date).Count() - orderCount - orderCancelCount
                };
            }
        }
        public int getTotalBenifit()
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.Orders.Where(x => x.isReceived).Sum(x => x.Total - x.Sale) - context.ImportBills.Sum(x => x.TotalPrice);
            }
        }

    }
}
