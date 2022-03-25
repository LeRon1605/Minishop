using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DAO
{
    public class CategoryDAO
    {
        private ShopOnlineDbContext context;
        public CategoryDAO()
        {
            context = new ShopOnlineDbContext();
        }

        public Category find(int id)
        {
            return context.Categories.Find(id);
        }    

        public List<Category> findAll()
        {
            return context.Categories.ToList();
        }

        public void Add(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public bool Delete(int id)
        {
            Category category = context.Categories.Find(id);
            if (category != null)
            {
                context.Remove(category);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
                
        }
        public bool Update(Category entity)
        {
            Category category = (from c in context.Categories where (c.ID == entity.ID) select c).FirstOrDefault();
            if (category != null)
            {
                category.Name = entity.Name;
                category.Description = entity.Description;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Product> ListProducts(int id)
        {
            List<Product> list = context.Products.ToList().Where(product => product.CategoryID == id) as List<Product>;
            return list;
        }

    }
}
