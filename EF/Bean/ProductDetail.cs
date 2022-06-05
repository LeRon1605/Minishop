using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models.DTO
{
    public class ProductDetail
    {
        [ForeignKey("Product")]
        public int ID { get; set; }
        [Display(Name ="Khối lượng")]
        [Range(0,int.MaxValue)]
        public int Mass { get; set; }
        [Display(Name ="Năng lượng")]
        [Range(0,int.MaxValue)]
        public int Power { get; set; }
        [Display(Name ="Kích thước")]
        public int Size { get; set; }
        [Display(Name = "Nhà cung cấp")]
        public string Producer { get; set; }
        [Display(Name ="Thời gian bảo hành")]
        public int MaintenanceTime { get; set; }
        [Display(Name ="Hình ảnh")]
        public string Image { get; set; }
        public Product Product { get; set; }
    }
}