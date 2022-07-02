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
    public class OrderBUS
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
                    Sale = order.Sale,
                    Note = order.Note,
                    ReceiverAddress = order.ReceiverAddress,
                    Total = order.Total,
                    Voucher = new Voucher
                    {
                        ID = order.Voucher.ID,
                        Value = order.Voucher.Value,
                        Seri = order.Voucher.Seri
                    },
                    ProductOrder = order.ProductOrder.Select(productOrder => new ProductOrder 
                    {
                        ID = productOrder.ID,
                        ProductID = productOrder.ProductID,
                        Product = productOrder.ProductID == null ? null : productOrder.Product,
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
                    Sale = order.Sale,
                    ReceiverName = order.ReceiverName,
                    ReceiverPhone = order.ReceiverPhone,
                    Note = order.Note,
                    ReceiverAddress = order.ReceiverAddress,
                    Total = order.Total,
                    VoucherID = order.VoucherID,
                    Voucher = new Voucher
                    {
                        ID = order.Voucher.ID,
                        Value = order.Voucher.Value,
                        Seri = order.Voucher.Seri
                    },
                    ProductOrder = order.ProductOrder.Select(productOrder => new ProductOrder
                    {
                        ID = productOrder.ID,
                        ProductID = productOrder.ProductID,
                        Product = productOrder.Product,
                        Price = productOrder.Price,
                        Quantity = productOrder.Quantity,
                        isComment = productOrder.isComment
                    }).ToList(),
                    StateOrder = new List<StateOrder>()
                    {
                        new StateOrder { State = new StateBUS().getCurrentProductState(order.ID) }
                    },
                }).ToList();
                return orders.Where(order => order.UserID == userID && (stateID == 0 || order.StateOrder.First().State.ID == stateID) && order.ID.ToString().Contains(keyword)).ToList();
            }
        }
        public List<Order> getPage(int page, int pageSize, string keyword, int stateID, out int totalRow, DateTime? startDate, DateTime? endDate)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                StateBUS StateBUS = new StateBUS();
                List<Order> orders = context.Orders.AsNoTracking()
                                     .Select(order => new Order
                                     {
                                         ID = order.ID,
                                         isCancel = order.isCancel,
                                         isReceived = order.isReceived,
                                         CreatedAt = order.CreatedAt,
                                         Sale = order.Sale,
                                         UpdatedAt = order.UpdatedAt,
                                         UserID = order.UserID,
                                         User = order.User,
                                         ReceiverName = order.ReceiverName,
                                         ReceiverPhone = order.ReceiverPhone,
                                         Note = order.Note,
                                         ReceiverAddress = order.ReceiverAddress,
                                         Total = order.Total,
                                         Voucher = new Voucher
                                         {
                                             ID = order.Voucher.ID,
                                             Value = order.Voucher.Value
                                         },
                                         StateOrder = new List<StateOrder>()
                                         {
                                             new StateOrder { State = StateBUS.getCurrentProductState(order.ID) }
                                         }
                                     }).AsEnumerable().Where(order => (order.UserID.ToString().Contains(keyword) || order.User.Name.Contains(keyword) || order.ID.ToString().Contains(keyword)) && (stateID == 0 || order.StateOrder.First().State.ID == stateID) && (order.CreatedAt.Date >= ((DateTime)startDate).Date && order.CreatedAt.Date <= ((DateTime)endDate).Date))
                                       .ToList();
                totalRow = (int)Math.Ceiling((double)orders.Count() / pageSize);
                return orders.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            }
        }    

        public int getUserOrderCount(int userID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.Orders.Where(order => order.UserID == userID).Count();
            }    
        }
        public int add(Order order, int UserID, out string message)
        {
            User user = new UserBUS().find(UserID);
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
                            Product product = new ProductBUS().find((int)productOrder.ProductID);
                            if (product != null)
                            {
                                if (productOrder.Quantity <= product.Stock)
                                {
                                    productOrders.Add(new ProductOrder
                                    {
                                        ProductID = product.ID,
                                        Quantity = productOrder.Quantity,
                                        Price = product.Price,
                                        isComment = false
                                    });
                                    TotalPrice += productOrder.Quantity * product.Price;
                                    new ProductBUS().export(product.ID, -productOrder.Quantity);
                                    new CartBUS().save(product.ID, -productOrder.Quantity, user.ID);
                                }
                            }
                        }
                        if (productOrders.Count > 0)
                        {
                            int? voucherID = null;
                            int sale = 0;
                            if (!string.IsNullOrEmpty(order.Voucher.Seri))
                            {
                                Voucher voucher = new VoucherBUS().check(order.Voucher.Seri);
                                if (voucher != null)
                                {
                                    voucher.Quantity -= 1;
                                    voucherID = voucher.ID;
                                    sale = voucher.Value;
                                    new VoucherBUS().Update(voucher);
                                }    
                            }
                            if (sale > TotalPrice) sale = TotalPrice;
                            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
                            {
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
                                    VoucherID = voucherID,
                                    Sale = sale,
                                    CreatedAt = DateTime.Now
                                };
                                context.Orders.Add(newOrder);
                                context.SaveChanges();
                                new StateBUS().addProductState(newOrder.ID, "Đặt đơn hàng thành công");
                                new StateBUS().addProductState(newOrder.ID, "Đang chờ xác nhận");
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
        public bool confirm(int orderID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                Order order = context.Orders.Select(o => new Order
                {
                    ID = o.ID,
                    StateOrder = o.StateOrder.Select(stateOrder => new StateOrder { State = stateOrder.State }).ToList(),
                    isCancel = o.isCancel,
                    isReceived = o.isReceived,
                }).FirstOrDefault(o => o.ID == orderID);
                if (order != null && !order.isCancel && !order.isReceived && order.StateOrder.Last().State.Name == "Đang chờ xác nhận")
                {
                    new StateBUS().addProductState(order.ID, "Đã xác nhận đơn hàng");
                    new StateBUS().addProductState(order.ID, "Đang chuẩn bị đơn hàng");
                    return true;
                }
                return false;
            }
        }
        public bool decline(int orderID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                Order order = context.Orders.Find(orderID);
                if (order == null) return false;
                StateBUS StateBUS = new StateBUS();
                if (!order.isCancel && !order.isReceived && StateBUS.getCurrentProductState(orderID).Name == "Đang chờ xác nhận")
                {
                    StateBUS.addProductState(order.ID, "Đơn hàng bị từ chối");
                    order.isCancel = true;
                    context.SaveChanges();
                    ProductBUS ProductBUS = new ProductBUS();
                    List<ProductOrder> productOrders = find(orderID).ProductOrder as List<ProductOrder>;
                    foreach (ProductOrder productOrder in productOrders)
                    {
                        ProductBUS.import((int)productOrder.ProductID, productOrder.Quantity);
                    }
                    return true;
                }
                return false;
            }
        }
        public bool deliver(int orderID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                Order order = context.Orders.Find(orderID);
                if (order == null) return false;
                StateBUS StateBUS = new StateBUS();
                if (!order.isCancel && !order.isReceived && StateBUS.getCurrentProductState(orderID).Name == "Đang chuẩn bị đơn hàng")
                {
                    StateBUS.addProductState(orderID, "Đang giao hàng");
                    return true;
                }
                return false;
            }
        }
        public bool declineDeliver(int orderID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                Order order = context.Orders.Find(orderID);
                if (order == null) return false;
                StateBUS StateBUS = new StateBUS();
                if (!order.isCancel && !order.isReceived && StateBUS.getCurrentProductState(orderID).Name == "Đang giao hàng")
                {
                    order.isCancel = true;
                    context.SaveChanges();
                    StateBUS.addProductState(orderID, "Giao hàng thất bại");
                    ProductBUS ProductBUS = new ProductBUS();
                    List<ProductOrder> productOrders = find(orderID).ProductOrder as List<ProductOrder>;
                    foreach (ProductOrder productOrder in productOrders)
                    {
                        ProductBUS.import((int)productOrder.ProductID, productOrder.Quantity);
                    }
                    return true;
                }
                return false;
            }
        }
        public bool confirmDeliver(int orderID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                Order order = context.Orders.Find(orderID);
                if (order == null) return false;
                StateBUS StateBUS = new StateBUS();
                if (!order.isCancel && !order.isReceived && StateBUS.getCurrentProductState(orderID).Name == "Đang giao hàng")
                {
                    StateBUS.addProductState(orderID, "Đã giao hàng");
                    StateBUS.addProductState(orderID, "Đang chờ xác nhận nhận hàng");
                    return true;
                }
                return false;
            }
        }
        public bool confirmReceived(int orderID, int userID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                Order order = context.Orders.FirstOrDefault(o => o.ID == orderID && o.UserID == userID);
                if (order == null) return false;
                StateBUS StateBUS = new StateBUS();
                if (!order.isCancel && !order.isReceived && StateBUS.getCurrentProductState(orderID).Name == "Đang chờ xác nhận nhận hàng")
                {
                    order.isReceived = true;
                    context.SaveChanges();
                    StateBUS.addProductState(orderID, "Đã nhận hàng");
                    ProductBUS ProductBUS = new ProductBUS();
                    List<ProductOrder> productOrders = find(orderID).ProductOrder;
                    foreach (ProductOrder productOrder in productOrders)
                    {
                        ProductBUS.Sold((int)productOrder.ProductID, productOrder.Quantity);
                    }
                    return true;
                }
                return false;
            }
        }
        public bool declineReceived(int orderID, int userID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                Order order = context.Orders.FirstOrDefault(o => o.ID == orderID && o.UserID == userID);
                if (order == null) return false;
                StateBUS StateBUS = new StateBUS();
                if (!order.isCancel && !order.isReceived && StateBUS.getCurrentProductState(orderID).Name == "Đang chờ xác nhận nhận hàng")
                {
                    order.isCancel = true;
                    context.SaveChanges();
                    StateBUS.addProductState(orderID, "Nhận hàng thất bại");
                    ProductBUS ProductBUS = new ProductBUS();
                    List<ProductOrder> productOrders = find(orderID).ProductOrder as List<ProductOrder>;
                    foreach (ProductOrder productOrder in productOrders)
                    {
                        ProductBUS.import((int)productOrder.ProductID, productOrder.Quantity);
                    }
                    return true;
                }
                return false;
            }
        }
        public bool cancel(int orderID, int userID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                Order order = context.Orders.FirstOrDefault(o => o.ID == orderID && o.UserID == userID);
                if (order == null) return false;
                StateBUS StateBUS = new StateBUS();
                if (!order.isCancel && !order.isReceived && StateBUS.getCurrentProductState(orderID).Name == "Đang chờ xác nhận")
                {
                    order.isCancel = true;
                    context.SaveChanges();
                    StateBUS.addProductState(orderID, "Đơn hàng đã bị hủy");
                    foreach (ProductOrder productOrder in find(orderID).ProductOrder)
                    {
                        new ProductBUS().import((int)productOrder.ProductID, productOrder.Quantity);
                    }
                    return true;
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

        public ProductOrder getOrderDetail(int id)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.ProductOrder.Select(p => new ProductOrder
                {
                    ID = p.ID,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    Comment = new Comment
                    {
                        ID = p.Comment.ID,
                        Content = p.Comment.Content,
                        CreatedAt = p.Comment.CreatedAt,
                        Reply = p.Comment.Reply,
                        UpdatedAt = p.Comment.UpdatedAt,
                        DeletedAt = p.Comment.DeletedAt,
                        isDeleted = p.Comment.isDeleted,
                        Rate = p.Comment.Rate,
                        UserID = p.Comment.UserID
                    },
                    Product = new Product
                    {
                        Name = p.Product.Name,
                        ID = p.Product.ID,
                        Image = p.Product.Image
                    }
                }).FirstOrDefault(p => p.ID == id);
            }
        }
        public int Count()
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.Orders.Count();
            }
        }
    }
}
