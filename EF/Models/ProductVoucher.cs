using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EF.Models
{
    public class ProductVoucher
    {
        [Key]
        public int ID { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public int VoucherID { get; set; }
        [ForeignKey("VoucherID")]
        public Voucher Voucher { get; set; }
    }
}