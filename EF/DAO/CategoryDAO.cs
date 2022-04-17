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
                category.UpdatedAt = DateTime.Now;
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

        public int Count()
        {
            return context.Categories.Count();
        }

        public List<Category> getPage(int page, int pageSize, string keyword,out int totalRow)
        {
            totalRow = 0;
            if(page > 0)
            {
                List<Category> categories = context.Categories.Where(category => (
                (category.Name.Contains(keyword) || keyword == ""))).ToList();
                totalRow = (int)Math.Ceiling((double)categories.Count() / pageSize);
                return categories.Select(product => new Category
                {
                    ID = product.ID,
                    Name = product.Name,  
                    Description = product.Description,
                }).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            return null;
        }
    }
}
