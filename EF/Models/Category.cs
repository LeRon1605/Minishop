using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EF.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Loại sản phẩm")]
        [Required(ErrorMessage = "Loại sản phẩm không được để trống")]
        public string Name { get; set; }
        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; }
    }
}