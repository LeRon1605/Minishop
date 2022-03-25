using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.DAO
{
    public class VoucherDAO
    {
        private ShopOnlineDbContext context;

        public void Add(Voucher voucher)
        {
            context.Vouchers.Add(voucher);
            context.SaveChanges();
        }
        List<Voucher> findAll()
        {
            return context.Vouchers.ToList();
        }
        public bool Delete(int id)
        {
            Voucher voucher = context.Vouchers.Find(id);
            if(voucher != null)
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
            if( voucher != null)
            {
                voucher.Value = entity.Value;
                voucher.Seri = entity.Seri;
                voucher.Quantity = entity.Quantity;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public Voucher find(int id)
        {
            return context.Vouchers.Find(id);
        }

        public bool isValid(int id)
        {
            Voucher voucher = context.Vouchers.Find(id);
            if (DateTime.Now > voucher.EndDate)
            {
                return false;
            }
            else
                return true;
        }
        public int countDay(int id)
        {
            Voucher voucher = context.Vouchers.Find(id);
            return (int)(voucher.EndDate - voucher.StartDate).TotalDays;
        }
    }
}
