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

        public List<Category> findAll()
        {
            return context.Categories.ToList();
        }

        public void Add(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }
    }
}
