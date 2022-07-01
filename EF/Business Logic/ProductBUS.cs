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
    public class ProductBUS
    {
        private ShopOnlineDbContext context;
        public ProductBUS()
        {
            context = new ShopOnlineDbContext();
        }
        public List<Product> findAll()
        {
            return context.Products.AsNoTracking().ToList();
        }
        public List<Product> getPage(int page, int pageSize, bool? state, string keyword, string categoryID, string minValue, string maxValue, out int totalRow)
        {
            totalRow = 0;
            if (page > 0)
            {
                CategoryBUS categoryDAO = new CategoryBUS();
                int min, max;
                if (!int.TryParse(minValue, out min)) min = 0;
                if (!int.TryParse(maxValue, out max)) max = int.MaxValue;
                List<Product> products = context.Products.AsNoTracking().Select(product => new Product
                {
                    ID = product.ID,
                    Name = product.Name,
                    Stock = product.Stock,
                    Price = product.Price,
                    Image = product.Image,
                    Sold = product.Sold,
                    Category = (product.CategoryID == null) ? null : categoryDAO.find((int)product.CategoryID),
                    CategoryID = product.CategoryID
                }).Where(product => (
                    (product.Name.Contains(keyword) || keyword == "") &&
                    (categoryID == "All" || categoryID == null || product.CategoryID == int.Parse(categoryID)) &&
                    (product.Price >= min && product.Price <= max) && (state == null || (bool)state == (product.Stock > 0))
                )).ToList();
                totalRow = (int)Math.Ceiling((double)products.Count() / pageSize);
                if (pageSize >= products.Count()) return products;
                else
                    try{
                        return products.GetRange((page - 1) * pageSize, pageSize);
                    }
                    catch(Exception e)
                    {
                        return products.GetRange((page - 1) * pageSize, products.Count() - (page - 1) * pageSize);
                    }
            }
            return null;
        }

        public List<Product> getLasted(int quantity)
        {
            if (quantity < 1) return null;
            else
            {
                int skip = context.Products.AsNoTracking().Count() - quantity;
                if (skip <= 0) return context.Products.AsNoTracking().ToList(); 
                else return context.Products.AsNoTracking().Skip(context.Products.AsNoTracking().Count() - quantity).ToList();
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

        public bool Sold(int id, int quantity)
        {
            Product product = context.Products.Find(id);
            if (product != null && quantity > 0)
            {
                product.Sold += quantity;
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
                context.Entry(product).Reference(p => p.Category).Load();
            }
            return product;
        }

        public int Count()
        {
            return context.Products.AsNoTracking().Count();
        }

        //public bool import(int id, ImportBill importBill)
        //{
        //    Product product = context.Products.Find(id);
        //    if (product == null) return false;
        //    else
        //    {
        //        product.ImportBills.Add(new ImportBill { 
        //            Quantity = importBill.Quantity,
        //            TotalPrice = importBill.TotalPrice,
        //            CreatedAt = DateTime.Now,
        //            UpdatedAt = null
        //        });
        //        product.Stock += importBill.Quantity;
        //        product.UpdatedAt = DateTime.Now;
        //        context.SaveChanges();
        //        return true;
        //    }
        //}
        public bool import(int id, int quantity)
        {
            Product product = context.Products.Find(id);
            if (product != null && quantity > 0)
            {
                product.Stock += quantity;
                product.UpdatedAt = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool export(int id, int quantity)
        {
            Product product = context.Products.Find(id);
            if (product != null && quantity < 0)
            {
                product.Stock += quantity;
                product.UpdatedAt = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool exist(int id)
        {
            return (context.Products.Find(id) != null);
        }
    }   
}
