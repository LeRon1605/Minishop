using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DAO
{
    public class StateDAO
    {
        public State findByName(string name)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.States.FirstOrDefault(state => state.Name == name);
            }    
        }    
        public bool addProductState(int orderID, string stateName)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                if (context.Orders.Find(orderID) != null)
                {
                    context.StateOrder.Add(new StateOrder
                    {
                        StateID = findByName("Đang chờ xác nhận").ID,
                        OrderID = orderID,
                        Date = DateTime.Now
                    });
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }    
    }
}
