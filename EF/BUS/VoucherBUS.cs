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
    public class VoucherBUS
    {
        private ShopOnlineDbContext context;
        public VoucherBUS()
        {
            context = new ShopOnlineDbContext();
        }
        public Voucher find(int id)
        {
            return context.Vouchers.Find(id);
        }
        public List<Voucher> findAll()
        {
            return context.Vouchers.AsNoTracking().ToList();
        }
        public bool Add(Voucher entity)
        {
            Voucher voucher = context.Vouchers.FirstOrDefault(x => x.Seri == entity.Seri);
            if (voucher != null)
            {
                if (entity.EndDate.Date < entity.StartDate.Date || entity.StartDate.Date > voucher.EndDate.Date)
                {
                    entity.CreatedAt = DateTime.Now;
                    context.Vouchers.Add(entity);
                    context.SaveChanges();
                    return true;
                }    
            }
            else
            {
                entity.CreatedAt = DateTime.Now;
                context.Vouchers.Add(entity);
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool Delete(int id)
        {
            Voucher voucher = context.Vouchers.Find(id);
            if (voucher != null)
            {
                context.Vouchers.Remove(voucher);
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool Update(Voucher entity)
        {
            Voucher voucher = context.Vouchers.Find(entity.ID);
            if (voucher != null)
            {
                voucher.Value = entity.Value;
                voucher.Seri = entity.Seri;
                voucher.Quantity = entity.Quantity;
                voucher.StartDate = entity.StartDate;
                voucher.EndDate = entity.EndDate;
                if (voucher.StartDate > voucher.EndDate)
                {
                    return false;
                }
                voucher.UpdatedAt = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public int Count()
        {
            return context.Vouchers.AsNoTracking().Count();
        }
        public List<Voucher> getPage(int page, int pageSize, string keyword, string value, string state, out int totalRow)
        {
            totalRow = 0;
            if (page > 0)
            {
                List<Voucher> Vouchers = context.Vouchers.AsNoTracking().Select(voucher => new Voucher
                {
                    ID = voucher.ID,
                    Value = voucher.Value,
                    Seri = voucher.Seri,
                    Quantity = voucher.Quantity,
                    StartDate = voucher.StartDate,
                    EndDate = voucher.EndDate,
                }).Where(voucher => 
                    (voucher.Seri.Contains(keyword) || keyword == "") &&
                    (value == "All" || voucher.Value <= int.Parse(value)) &&
                    (state == "All" || (DateTime.Now.Date < voucher.StartDate.Date && state == "inActivated") || (DateTime.Now.Date >= voucher.StartDate.Date && DateTime.Now.Date <= voucher.EndDate.Date && state == "valid" && voucher.Quantity > 0) || (DateTime.Now.Date > voucher.EndDate.Date && state == "invalid"))
                 ).ToList();    
                totalRow = (int)Math.Ceiling((double)Vouchers.Count() / pageSize);
                if (Vouchers.Count() <= pageSize) return Vouchers;
                else
                {
                    try
                    {
                        return Vouchers.GetRange((page - 1) * pageSize, pageSize);
                    }
                    catch (Exception e)
                    {
                        return Vouchers.GetRange((page - 1) * pageSize, Vouchers.Count() - (page - 1) * pageSize);
                    }
                }
            }
            return null;
        }
        public Voucher check(string Seri)
        {
            return context.Vouchers.FirstOrDefault(voucher => voucher.Seri == Seri && voucher.EndDate.Date >= DateTime.Now.Date && voucher.StartDate.Date <= DateTime.Now.Date && voucher.Quantity > 0);
        }
        public List<Voucher> getLasted(int quantity)
        {
            return context.Vouchers.AsNoTracking().Where(voucher => voucher.EndDate.Date >= DateTime.Now.Date && voucher.StartDate.Date <= DateTime.Now.Date && voucher.Quantity > 0).OrderByDescending(voucher => voucher.Value).Take(quantity).ToList();
        }


    }
}