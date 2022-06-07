using Models.DAL;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BLL
{
    public class ImportBillBO
    {
        public List<ImportBill> getBillsOfProduct(int productID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.ImportBills.Where(bill => bill.ProductID == productID).ToList();
            }    
        }
        public List<ImportBill> findAll(int page, int pageSize, string keyword, DateTime startDate, DateTime endDate, out int totalRow)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                List<ImportBill> list = context.ImportBills.AsNoTracking().Include(bill => bill.Product)
                                                           .Where(bill => (bill.Product.Name.Contains(keyword) || bill.ID.ToString().Equals(keyword)) && (bill.CreatedAt.Date >= startDate && bill.CreatedAt.Date <= endDate))
                                                           .ToList();
                totalRow = (int)Math.Ceiling((double)list.Count() / pageSize);
                if (list.Count() <= pageSize) return list;
                else
                {
                    try
                    {
                        return list.GetRange((page - 1) * pageSize, pageSize);
                    }
                    catch (Exception e)
                    {
                        return list.GetRange((page - 1) * pageSize, list.Count() - (page - 1) * pageSize);
                    }
                }
            }
        }

        public bool Update(ImportBill bill)
        {
            // 
            return false;
        }

        public bool Remove(ImportBill bill)
        {
            // 
            return false;
        }
    }
}
