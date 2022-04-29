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

        public List<Product> getPage(int page, int pageSize, string keyword, string categoryID, string price, out int totalRow)
        {
            totalRow = 0;
            if (page > 0)
            {
                CategoryDAO categoryDAO = new CategoryDAO();
                List<Product> products = context.Products.Where(product => (
                    (product.Name.Contains(keyword) || keyword == "") &&
                    (categoryID == "All" || categoryID == null ||product.CategoryID == int.Parse(categoryID)) &&
                    (price == "All" || product.Price <= int.Parse(price))
                )).ToList();
                totalRow = (int)Math.Ceiling((double)products.Count() / pageSize);
                return products.Select(product => new Product {
                    ID = product.ID,
                    Name = product.Name,
                    Stock = product.Stock,
                    Price = product.Price,
                    Image = product.Image,
                    Category = (product.CategoryID != null) ? categoryDAO.find((int)product.CategoryID) : null
                }).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            return null;
        }

        public List<Product> getLasted(int quantity)
        {
            if (quantity < 1) return null;
            else
            {
                int skip = context.Products.Count() - quantity;
                if (skip <= 0) return context.Products.ToList(); 
                else return context.Products.Skip(context.Products.Count() - quantity).ToList();
            }
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
                product.CategoryID = entity.CategoryID;
                product.Power = entity.Power;
                product.Image = entity.Image;
                product.Mass = entity.Mass;
                product.MaintenanceTime = entity.MaintenanceTime;
                product.Producer = entity.Producer;
                product.Description = entity.Description;
                product.UpdatedAt = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Product find(int id, bool isLoadCategory = false)
        {
            Product product = context.Products.Find(id);
            if (isLoadCategory)
            {
                CategoryDAO categoryDAO = new CategoryDAO();
                product.Category = (product.CategoryID == null) ? null : categoryDAO.find((int)product.CategoryID);
            }
            return product;
        }

        public int Count()
        {
            return context.Products.Count();
        }

        public bool import(int id, int quantity)
        {
            Product product = context.Products.Find(id);
            if (product == null) return false;
            else
            {
                product.Stock += quantity;
                product.UpdatedAt = DateTime.Now;
                context.SaveChanges();
                return true;
            }
        }
    }   
}
