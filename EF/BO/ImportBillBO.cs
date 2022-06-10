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
        public List<ImportBillDetail> getBillOfProduct(int productID)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.ImportBillDetails.AsNoTracking().Where(x => x.ProductID == productID).OrderByDescending(x => x.ImportBill.CreatedAt).ToList();
            }    
        }
        public ImportBill find(int id)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                return context.ImportBills.AsNoTracking().Select(x => new ImportBill
                {
                    ID = x.ID,
                    TotalPrice = x.TotalPrice,
                    ImportBillDetails = x.ImportBillDetails.Select(detail => new ImportBillDetail { 
                        ID = detail.ID,
                        ProductID = detail.ProductID,
                        ImportBillID = detail.ImportBillID,
                        Product = (detail.ProductID == null) ? null : detail.Product,
                        Quantity = detail.Quantity
                    }).ToList(),
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).FirstOrDefault(x => x.ID == id);
            }    
        }
        public List<ImportBill> getPage(int page, int pagesize, string keyword, DateTime startdate, DateTime enddate, out int totalrow)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                List<ImportBill> list = context.ImportBills.AsNoTracking()
                                                           .Where(bill => (bill.ID.ToString().Equals(keyword) || keyword == "") && (bill.CreatedAt.Date >= startdate && bill.CreatedAt.Date <= enddate))
                                                           .OrderByDescending(bill => bill.CreatedAt)
                                                           .ToList();
                totalrow = (int)Math.Ceiling((double)list.Count() / pagesize);
                if (list.Count() <= pagesize) return list;
                else
                {
                    try
                    {
                        return list.GetRange((page - 1) * pagesize, pagesize);
                    }
                    catch (Exception e)
                    {
                        return list.GetRange((page - 1) * pagesize, list.Count() - (page - 1) * pagesize);
                    }
                }
            }
        }

        public void Add(ImportBill bill)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                bill.CreatedAt = DateTime.Now;
                context.ImportBills.Add(bill);
                context.SaveChanges();
                ProductBO productBO = new ProductBO();
                foreach (ImportBillDetail detail in bill.ImportBillDetails)
                {
                    productBO.import((int)detail.ProductID, detail.Quantity);
                }
            }    
        }

        //public bool Update(ImportBill bill)
        //{

        //    using (ShopOnlineDbContext context = new ShopOnlineDbContext())
        //    {
        //        ImportBill importBill = context.ImportBills.Find(bill.ID);
        //        if (importBill == null) return false;
        //        else
        //        {
        //            ProductBO productBO = new ProductBO();
        //            for (int i = 0;i < importBill.ImportBillDetails.Count;i++)
        //            {

        //                productBO.export(importBill.ImportBillDetails[i].ProductID, -importBill.ImportBillDetails[i].Quantity);
        //                productBO.import(bill.ImportBillDetails[i].ProductID, bill.ImportBillDetails[i].Quantity);
        //                importBill.ImportBillDetails[i].Quantity = bill.ImportBillDetails[i].Quantity;
        //            }
        //            importBill.TotalPrice = bill.TotalPrice;
        //            importBill.UpdatedAt = DateTime.Now;
        //            context.SaveChanges();
        //            return true;
        //        }
        //    }
        //}

        public bool delete(int id)
        {
            using (ShopOnlineDbContext context = new ShopOnlineDbContext())
            {
                ImportBill importbill = context.ImportBills.Select(x => new ImportBill { 
                    ID = x.ID,
                    ImportBillDetails = x.ImportBillDetails
                }).FirstOrDefault(x => x.ID == id);
                if (importbill != null)
                {
                    ProductBO productBO = new ProductBO();
                    foreach (ImportBillDetail detail in importbill.ImportBillDetails)
                    {
                        if (detail.ProductID != null)
                        {
                            productBO.export((int)detail.ProductID, -detail.Quantity);
                        }    
                    }
                    context.ImportBills.Remove(importbill);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
