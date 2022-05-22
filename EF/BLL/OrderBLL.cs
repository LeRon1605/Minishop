using Models.DTO;
using Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BLL
{
    public class OrderBLL
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
                    ReceiverName = order.ReceiverName,
                    ReceiverPhone = order.ReceiverPhone,
                    Note = order.Note,
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
        public List<Order> getUserOrders(int userID, int stateID, string keyword)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                List<Order> orders = context.Orders.AsNoTracking().Select(order => new Order
                {
                    ID = order.ID,
                    isCancel = order.isCancel,
                    isReceived = order.isReceived,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    UserID = order.UserID,
                    ReceiverName = order.ReceiverName,
                    ReceiverPhone = order.ReceiverPhone,
                    Note = order.Note,
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
                return orders.Where(order => order.UserID == userID && (stateID == 0 || order.StateOrder.Last().StateID == stateID) && order.ID.ToString().Contains(keyword)).ToList();
            }
        }
        public List<Order> getPage(int page, int pageSize, string keyword, int stateID, out int totalRow)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                totalRow = (int)Math.Ceiling((double)context.Orders.AsNoTracking().Count() / pageSize);
                List<Order> orders = context.Orders.AsNoTracking()
                                     .Select(order => new Order
                                     {
                                         ID = order.ID,
                                         isCancel = order.isCancel,
                                         isReceived = order.isReceived,
                                         CreatedAt = order.CreatedAt,
                                         UpdatedAt = order.UpdatedAt,
                                         UserID = order.UserID,
                                         User = order.User,
                                         ReceiverName = order.ReceiverName,
                                         ReceiverPhone = order.ReceiverPhone,
                                         Note = order.Note,
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
                return orders.Where(order => (order.UserID.ToString().Contains(keyword) || order.User.Name.Contains(keyword) || order.ID.ToString().Contains(keyword)) && (stateID == 0 || order.StateOrder.Last().StateID == stateID))
                                     .Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            }
        }    
        public int add(Order order, int UserID, out string message)
        {
            User user = new UserBLL().find(UserID);
            if (user != null)
            {
                List<ProductOrder> productOrders = new List<ProductOrder>();
                int TotalPrice = 0;
                if (user.isActivated)
                {
                    if (order != null && order.ProductOrder.Count > 0)
                    {
                        foreach (ProductOrder productOrder in order.ProductOrder)
                        {
                            Product product = new ProductBLL().find((int)productOrder.ProductID);
                            if (product != null)
                            {
                                if (productOrder.Quantity <= product.Stock)
                                {
                                    product.Stock -= productOrder.Quantity;
                                    productOrders.Add(new ProductOrder
                                    {
                                        ProductID = product.ID,
                                        Quantity = productOrder.Quantity,
                                        Price = product.Price
                                    });
                                    TotalPrice += productOrder.Quantity * product.Price;
                                    new ProductBLL().Update(product);
                                    new CartBLL().DeleteProduct(product.ID, user.ID);
                                }
                            }
                        }
                        if (productOrders.Count > 0)
                        {
                            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
                            {
                                // Find Voucher -> if valid add voucherID else not
                                Order newOrder = new Order
                                {
                                    UserID = user.ID,
                                    ReceiverAddress = order.ReceiverAddress,
                                    ReceiverName = order.ReceiverName,
                                    ReceiverPhone = order.ReceiverPhone,
                                    Note = order.Note,
                                    Total = TotalPrice,
                                    ProductOrder = productOrders,
                                    isCancel = false,
                                    isReceived = false,
                                    VoucherID = null,
                                    CreatedAt = DateTime.Now
                                };
                                context.Orders.Add(newOrder);
                                context.SaveChanges();
                                new StateBLL().addProductState(newOrder.ID, "Đặt đơn hàng thành công");
                                new StateBLL().addProductState(newOrder.ID, "Đang chờ xác nhận");
                                message = "Tạo đơn hàng thành công";
                                return newOrder.ID;
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
                Order order = context.Orders.Select(o => new Order { 
                    ID = o.ID,
                    StateOrder = o.StateOrder.Select(stateOrder => new StateOrder { State = stateOrder.State }).ToList(),
                    isCancel = o.isCancel,
                    isReceived = o.isReceived
                }).FirstOrDefault(o => o.ID == orderID);
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
                        new StateBLL().addProductState(orderID, newState);
                        return true;
                    }
                }
                return false;
            }
        }    
        public bool confirm(int orderID)
        {
            return (addState(orderID, "Đang chờ xác nhận", "Đã xác nhận đơn hàng") && addState(orderID, "Đã xác nhận đơn hàng", "Đang chuẩn bị đơn hàng"));
        }
        public bool deliver(int orderID)
        {
            return addState(orderID, "Đang chuẩn bị đơn hàng", "Đang giao hàng");
        }
        public bool confirmDeliver(int orderID)
        {
            return (addState(orderID, "Đang giao hàng", "Đã giao hàng") && addState(orderID, "Đã giao hàng", "Đang chờ xác nhận nhận hàng"));
        }
        public bool confirmReceived(int orderID)
        {
            return addState(orderID, "Đang chờ xác nhận nhận hàng", "Đã nhận hàng", false, true);
        }
        public bool cancel(int orderID)
        {
            return addState(orderID, "Đang chờ xác nhận", "Đã hủy đơn hàng", true);
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
