using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EF.Models
{
    public class CartProduct
    {
        [Key]
        public int ID { get; set; }
        public int CartID { get; set; }
        [ForeignKey("CartID")]
        public Cart Cart { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public bool Status { get; set; }
        public DateTime InsertedAt { get; set; }
    }
}