using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models.DTO
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        public int ID { get; set; }
        [Display(Name = "Loại sản phẩm")]
        [Required(ErrorMessage = "Loại sản phẩm không được để trống")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Mô tả loại sản phẩm không được để trống")]
        [StringLength(300, MinimumLength = 30, ErrorMessage = "Mô phải dài từ 30 - 300 kí tự")]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Product> Products { get; set; }


    }
}