using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PBL3.Models
{
    public class Voucher
    {
        [Key]
        public int ID { get; set; }
        public int Value { get; set; }
        public string Seri { get; set; }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual List<ProductVoucher> ProductVoucher { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}