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
    public class CartBUS
    {
        private ShopOnlineDbContext context;
        public CartBUS()
        {
            context = new ShopOnlineDbContext();
        }
        public List<CartProduct> GetProductCart(int CartID, bool isSelected = false)
        {
            return context.CartProduct.AsNoTracking().Select(cp => new CartProduct
            {
                ID = cp.ID,
                CartID = cp.CartID,
                ProductID = cp.ProductID,
                Quantity = cp.Quantity,
                InsertedAt = cp.InsertedAt,
                isSelected = cp.isSelected,
                Product = cp.Product
            }).Where(cp => cp.CartID == CartID && (isSelected == false || cp.isSelected == isSelected)).ToList();
        }
        public bool save(int id, int quantity, int CartID)
        {
            Cart cart = context.Carts.Find(CartID);
            Product product = context.Products.Find(id);
            if (product != null && cart != null)
            {
                CartProduct productInCart = context.CartProduct.FirstOrDefault(e => e.CartID == CartID && e.ProductID == id);
                if (productInCart != null)
                {
                    if (productInCart.Quantity + quantity <= product.Stock)
                    {
                        if (productInCart.Quantity + quantity <= 0)
                        {
                            if (product.Stock >= 1)
                            {
                                productInCart.Quantity = 1;
                                productInCart.UpdatedAt = DateTime.Now;
                                context.SaveChanges();
                            }
                            else
                            {
                                context.CartProduct.Remove(productInCart);
                                context.SaveChanges();
                                return true;
                            }
                        }
                        else
                        {
                            productInCart.Quantity += quantity;
                            productInCart.UpdatedAt = DateTime.Now;
                            context.SaveChanges();
                        }
                        return true;
                    }
                }
                else
                {
                    if(quantity <= product.Stock)
                    {
                        CartProduct cartProduct = new CartProduct
                        {
                            CartID = CartID,
                            ProductID = id,
                            Quantity = quantity,
                            InsertedAt = DateTime.Now,
                            UpdatedAt = null,
                            isSelected = false
                        };
                        context.CartProduct.Add(cartProduct);
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }
        public bool DeleteProduct(int id, int cartID)
        {
            CartProduct cartProduct = context.CartProduct.FirstOrDefault(c => c.ID == id && c.CartID == cartID);
            if (cartProduct != null)
            {
                context.Remove(cartProduct);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool removeAll(int cartID)
        {
            var cartProducts = context.CartProduct.Where(p => p.CartID == cartID);
            if (cartProducts != null)
            {
                context.CartProduct.RemoveRange(cartProducts);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool select(int id, int cartID)
        {
            CartProduct cartProduct = context.CartProduct.FirstOrDefault(c => c.ID == id && c.CartID == cartID);
            if (cartProduct != null)
            {
                cartProduct.isSelected = !cartProduct.isSelected;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool selectAll(int cartID)
        {
            List<CartProduct> cartProducts = context.CartProduct.Where(p => p.CartID == cartID).ToList();
            if (cartProducts != null)
            {
                foreach(CartProduct cartProduct in cartProducts)
                {
                    cartProduct.isSelected = true;
                }
                context.UpdateRange(cartProducts);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public int getTotal(int userID, bool isSelected = false)
        {
            return context.CartProduct.Where(cp => cp.CartID == userID && (isSelected == false || cp.isSelected == isSelected)).Sum(x => x.Product.Price * x.Quantity);
        }
       
    }
}
