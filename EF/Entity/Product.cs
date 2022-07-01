using Models.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models.DTO
{
    public class Product
    {
        public Product()
        {
            CartProduct = new HashSet<CartProduct>();
            ProductOrder = new HashSet<ProductOrder>();
            ImportBillDetails = new List<ImportBillDetail>();
        }
        [Key]
        [Display(Name = "Mã sản phẩm")]
        public int ID { get; set; }

        [Display(Name = "Tên Sản Phẩm")]
        [Required(ErrorMessage ="Tên sản phẩm không được để trống")]
        public string Name { get; set; }

        [Display(Name = "Giá sản phẩm")]
        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Không nhận giá trị âm")]
        public int Price { get; set; }

        [Display(Name = "Số lượng hàng hóa")]
        [Range(0, int.MaxValue, ErrorMessage = "Không nhận giá trị âm")]
        public int Stock { get; set; }

        [Display(Name = "Khối lượng")]
        [Required(ErrorMessage = "Khối lượng hàng hóa không được để trống")]
        [Range(0, float.MaxValue, ErrorMessage = "Không nhận giá trị âm")]
        public double Mass { get; set; }

        [Display(Name = "Công suất")]
        [Required(ErrorMessage = "Công suất hàng hóa không được để trống")]
        [Range(0, float.MaxValue, ErrorMessage = "Không nhận giá trị âm")]
        public double Power { get; set; }
        
        [Display(Name = "Ngày sản xuất")]
        public DateTime ProducerDate { get; set; }

        [Display(Name = "Mô tả sản phẩm")]
        [Required(ErrorMessage = "Mô tả sản phẩm không được để trống")]
        [StringLength(300, MinimumLength = 30, ErrorMessage = "Mô phải dài từ 30 - 300 kí tự")]
        public string Description { get; set; }

        [Display(Name = "Hình ảnh")]
        [Required(ErrorMessage = "Ảnh sản phẩm không được để trống")]
        public string Image { get; set; }

        [Display(Name = "Thời gian bảo hành")]
        [Required(ErrorMessage = "Thời gian bảo hành không được để trống")]
        [Range(0, float.MaxValue, ErrorMessage = "Không nhận giá trị âm")]
        public double MaintenanceTime { get; set; }

        [Display(Name = "Nhà sản xuất")]
        [Required(ErrorMessage = "Nhà sản xuất không được để trống")]

        public string Producer { get; set; }
        [Display(Name = "Số lượng đã bán")]
        public int Sold { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = null;

        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductOrder> ProductOrder { get; set; }
        public virtual ICollection<CartProduct> CartProduct { get; set; }
        public virtual List<ImportBillDetail> ImportBillDetails { get; set; }
    }
}