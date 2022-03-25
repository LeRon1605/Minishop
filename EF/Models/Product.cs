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
        [Display(Name = "Tên Sản Phẩm")]
        [Required(ErrorMessage ="Tên sản phẩm không được để trống")]
        public string Name { get; set; }
        [Display(Name = "Giá sản phẩm")]
        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(0,int.MaxValue)]
        public int Price { get; set; }
        [Display(Name = "Hàng trong kho")]
        [Range(0,int.MaxValue)]
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