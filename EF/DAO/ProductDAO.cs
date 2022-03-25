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
        public List<Product> findAll()
        {
            return context.Products.ToList();
        }
        public void Add(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
        }

        public bool Delete(int id)
        {
            Product product = context.Products.Find(id);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Update(Product entity)
        {
            Product product = context.Products.Find(entity.ID);
            if (product != null)
            {
                product.Name = entity.Name;
                product.Price = entity.Price;
                product.Stock = entity.Stock;
                product.CategoryID = entity.CategoryID;
                product.Detail = entity.Detail;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Product find(int id)
        {
            return context.Products.Find(id);
        }
    }
}
