using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PBL3.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        public string ReceiverAddress { get; set; }
        public int Total { get; set; }
        public Voucher Voucher { get; set; }
        public User User { get; set; }
        public List<ProductOrder> ProductOrder { get; set; }
        public List<StateOrder> StateOrder { get; set; }
    }
}