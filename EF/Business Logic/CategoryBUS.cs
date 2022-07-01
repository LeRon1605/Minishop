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
    public class CategoryBUS
    {
        private ShopOnlineDbContext context;
        public CategoryBUS()
        {
            context = new ShopOnlineDbContext();
        }

        public Category find(int id)
        {
            return context.Categories.Find(id);
        }    

        public List<Category> findAll()
        {
            return context.Categories.AsNoTracking().ToList();
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
            Category category = context.Categories.FirstOrDefault(c => c.ID == entity.ID);
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
            List<Product> list = context.Products.AsNoTracking().Where(product => product.CategoryID == id).ToList();
            return list;
        }

        public int Count()
        {
            return context.Categories.AsNoTracking().Count();
        }

        public List<Category> getPage(int page, int pageSize, string keyword, out int totalRow)
        {
            totalRow = 0;
            if(page > 0)
            {
                List<Category> categories = context.Categories.AsNoTracking().Select(category => new Category
                {
                    ID = category.ID,
                    Name = category.Name,
                    Description = category.Description,
                }).Where(category => (category.Name.Contains(keyword)))
                  .ToList();
                totalRow = (int)Math.Ceiling((double)categories.Count() / pageSize);
                if (categories.Count() <= pageSize) return categories;
                else
                {
                    try
                    {
                        return categories.GetRange((page - 1) * pageSize, pageSize);
                    }
                    catch (Exception e)
                    {
                        return categories.GetRange((page - 1) * pageSize, categories.Count() - (page - 1) * pageSize);
                    }
                }
            }
            return null;
        }
    }
}
