using EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DAO
{
    public class StateDAO
    {
        public List<State> findAll()
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.States.AsNoTracking().ToList();
            }
        }
        public State findByName(string name)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.States.AsNoTracking().FirstOrDefault(state => state.Name == name);
            }    
        }    
        public void add(State state)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                context.States.Add(state);
                context.SaveChanges();
            }
        }
        public bool addProductState(int orderID, string stateName)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                Order order = context.Orders.Find(orderID);
                if (order != null)
                {
                    State state = findByName(stateName);
                    if (state == null) add(new State
                    {
                        Name = stateName,
                        Description = "Mô tả cho " + stateName
                    });
                    context.StateOrder.Add(new StateOrder
                    {
                        StateID = findByName(stateName).ID,
                        OrderID = orderID,
                        Date = DateTime.Now
                    });
                    order.UpdatedAt = DateTime.Now;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }    
    }
}
