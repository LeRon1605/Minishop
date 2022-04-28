using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DAO
{
    public class CartDAO
    {
        private ShopOnlineDbContext context;
        public CartDAO()
        {
            context = new ShopOnlineDbContext();
        }
        public bool add_Update(int id, int quantity, int CartID, bool isUpdate = false)
        {
            Cart cart = context.Carts.Find(CartID);
            Product product = context.Products.Find(id);
            if (product != null && cart != null)
            {
                CartProduct productInCart = context.CartProduct.Where(e => e.CartID == CartID && e.ProductID == id).FirstOrDefault();
                if (productInCart != null)
                {
                    if (isUpdate)
                    {
                        productInCart.Quantity = quantity;
                    }
                    else
                    {
                        productInCart.Quantity += quantity;
                    }
                    productInCart.UpdatedAt = DateTime.Now;
                }
                else
                {
                    CartProduct cartProduct = new CartProduct
                    {
                        CartID = CartID,
                        ProductID = id,
                        Quantity = quantity,
                        Status = true,
                        InsertedAt = DateTime.Now,
                        UpdatedAt = null
                    };
                    context.CartProduct.Add(cartProduct);
                }
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
