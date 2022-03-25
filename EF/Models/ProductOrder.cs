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
        [Display(Name ="Số lượng")]
        [Required(ErrorMessage ="Số lượng không được để trống")]
        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        [ForeignKey("OrderID")]
        public Order Order { get; set; }
    }
}