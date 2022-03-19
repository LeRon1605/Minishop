using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EF.Models
{
    public class ProductOrder
    {
        [Key]
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int OrderID { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        [ForeignKey("OrderID")]
        public Order Order { get; set; }
    }
}