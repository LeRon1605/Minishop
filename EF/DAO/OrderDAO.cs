using EF.Models;
using EF.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DAO
{
    public class OrderDAO
    {
        public Order find(int id)
        {
            using(ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.Orders.AsNoTracking().Select(order => new Order 
                {
                    ID = order.ID,
                    isCancel = order.isCancel,
                    isReceived = order.isReceived,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    UserID = order.UserID,
                    ReceiverAddress = order.ReceiverAddress,
                    Total = order.Total,
                    ProductOrder = order.ProductOrder.Select(productOrder => new ProductOrder 
                    {
                        ID = productOrder.ID,
                        ProductID = productOrder.ProductID,
                        Product = productOrder.Product,
                        Price = productOrder.Price,
                        Quantity = productOrder.Quantity
                    }).ToList(),
                    StateOrder = order.StateOrder.Select(stateOrder => new StateOrder
                    {
                        ID = stateOrder.ID,
                        StateID = stateOrder.StateID,
                        State = stateOrder.State,
                        Date = stateOrder.Date
                    }).ToList(),
                }).FirstOrDefault(order => order.ID == id);
            }    
        }
        public List<Order> getUserOrders(int userID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.Orders.AsNoTracking().Where(order => order.UserID == userID).Select(order => new Order
                {
                    ID = order.ID,
                    isCancel = order.isCancel,
                    isReceived = order.isReceived,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    UserID = order.UserID,
                    ReceiverAddress = order.ReceiverAddress,
                    Total = order.Total,
                    ProductOrder = order.ProductOrder.Select(productOrder => new ProductOrder
                    {
                        ID = productOrder.ID,
                        ProductID = productOrder.ProductID,
                        Product = productOrder.Product,
                        Price = productOrder.Price,
                        Quantity = productOrder.Quantity
                    }).ToList(),
                    StateOrder = order.StateOrder.Select(stateOrder => new StateOrder
                    {
                        ID = stateOrder.ID,
                        StateID = stateOrder.StateID,
                        State = stateOrder.State,
                        Date = stateOrder.Date
                    }).ToList(),
                }).ToList();
            }
        }
        public List<Order> getPage(int page, int pageSize, int userID, int stateID, out int totalRow)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                totalRow = (int)Math.Ceiling((double)context.Orders.AsNoTracking().Count() / pageSize);
                return context.Orders.AsNoTracking()
                                     .Select(order => new Order
                                     {
                                          ID = order.ID,
                                          isCancel = order.isCancel,
                                          isReceived = order.isReceived,
                                          CreatedAt = order.CreatedAt,
                                          UpdatedAt = order.UpdatedAt,
                                          UserID = order.UserID,
                                          ReceiverAddress = order.ReceiverAddress,
                                          Total = order.Total,
                                          ProductOrder = order.ProductOrder.Select(productOrder => new ProductOrder
                                          {
                                               ID = productOrder.ID,
                                               ProductID = productOrder.ProductID,
                                               Product = productOrder.Product,
                                               Price = productOrder.Price,
                                               Quantity = productOrder.Quantity
                                          }).ToList(),
                                          StateOrder = order.StateOrder.Select(stateOrder => new StateOrder
                                          {
                                               ID = stateOrder.ID,
                                               StateID = stateOrder.StateID,
                                               State = stateOrder.State,
                                               Date = stateOrder.Date
                                          }).ToList(),
                                     })
                                     .Where(order => (order.UserID == userID || userID == 0) && (order.StateOrder.Last().State.ID == stateID || stateID == 0))
                                     .Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            }
        }    
        public int add(List<ProductOrder> model, int UserID, string receiverAddress, out string message)
        {
            User user = new UserDAO().find(UserID);
            if (user != null)
            {
                List<ProductOrder> productOrders = new List<ProductOrder>();
                int TotalPrice = 0;
                if (user.isActivated)
                {
                    if (model != null && model.Count > 0)
                    {
                        foreach (ProductOrder productOrder in model)
                        {
                            Product product = new ProductDAO().find((int)productOrder.ProductID);
                            if (product != null)
                            {
                                if (productOrder.Quantity <= product.Stock)
                                {
                                    productOrders.Add(new ProductOrder
                                    {
                                        ProductID = product.ID,
                                        Quantity = productOrder.Quantity,
                                        Price = product.Price
                                    });
                                    TotalPrice += productOrder.Quantity * product.Price;
                                }
                            }
                        }
                        if (productOrders.Count > 0)
                        {
                            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
                            {
                                Order order = new Order
                                {
                                    UserID = user.ID,
                                    ReceiverAddress = receiverAddress,
                                    Total = TotalPrice,
                                    ProductOrder = productOrders,
                                    isCancel = false,
                                    isReceived = false,
                                    CreatedAt = DateTime.Now
                                };
                                context.Orders.Add(order);
                                context.SaveChanges();
                                new StateDAO().addProductState(order.ID, "Đang chờ xác nhận");
                                message = "Tạo đơn hàng thành công";
                                return order.ID;
                            }
                        }
                        else
                        {
                            message = "Số lượng sản phẩm không phù hợp";
                        }
                    }
                    else
                    {
                        message = "Số lượng sản phẩm không phù hợp";
                    }
                }
                else
                {
                    message = "Tài khoản chưa kích hoạt, không thể tạo đơn hàng";
                }
            }
            else
            {
                message = "Người dùng không tồn tại";
            }
            return -1;
        }
        public bool addState(int orderID, string lastState, string newState, bool isCancel = false, bool isReceived = false)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                Order order = context.Orders.Find(orderID);
                if (order != null)
                {
                    if (!order.isCancel && !order.isReceived && order.StateOrder.Last().State.Name == lastState)
                    {
                        if (isCancel)
                        {
                            order.isCancel = true;
                            context.SaveChanges();
                        }   
                        if (isReceived)
                        {
                            order.isReceived = true;
                            context.SaveChanges();
                        }    
                        new StateDAO().addProductState(orderID, newState);
                        return true;
                    }
                }
                return false;
            }
        }    
        public bool delete(int id)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                Order order = context.Orders.Find(id);
                if (order == null) return false;
                context.Remove(order);
                context.SaveChanges();
                return true;
            }    
        }    
    }
}
