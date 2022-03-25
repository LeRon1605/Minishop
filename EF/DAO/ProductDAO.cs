using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DAO
{
    public class ProductDAO
    {
        private ShopOnlineDbContext context;
        public ProductDAO()
        {
            context = new ShopOnlineDbContext();
        }
        public void Add(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
        }
    }
}
