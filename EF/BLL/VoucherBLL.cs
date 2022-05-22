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
    public class VoucherBLL
    {
        private ShopOnlineDbContext context;
        public VoucherBLL()
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
        public void Add(Voucher voucher)
        {
            context.Vouchers.Add(voucher);
            context.SaveChanges();
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
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public List<Voucher> getValid()
        {
            return context.Vouchers.AsNoTracking().Where(voucher => (voucher.EndDate < DateTime.Now)).ToList();
        }
        public int Count()
        {
            return context.Vouchers.AsNoTracking().Count();
        }
        public int countDay(int id)
        {
            Voucher voucher = context.Vouchers.Find(id);
            return (int)(voucher.EndDate - voucher.StartDate).TotalDays;
        }
        public List<Voucher> getPage(int page, int pageSize, string keyword, out int totalRow)
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
                }).Where(voucher => voucher.Seri.Contains(keyword) || keyword == "").ToList();

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
    }
}