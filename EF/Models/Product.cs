using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EF.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }
        public List<Comment> Comments { get; set; }
        public ProductDetail Detail { get; set; }
        public virtual List<ProductVoucher> ProductVoucher { get; set; }
        public List<ProductOrder> ProductOrder { get; set; }
        public virtual List<CartProduct> CartProduct { get; set; }
    }
}