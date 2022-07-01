using PBL3.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models.DTO
{     
    public class Voucher
    {
        public Voucher()
        {
            Orders = new HashSet<Order>();
        }
        [Key]
        public int ID { get; set; }
        [Display(Name ="Giá trị voucher")]
        [Required(ErrorMessage = "Giá trị không được bỏ trống")]
        public int Value { get; set; }
        [Display(Name ="Mã voucher")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Độ dài mã voucher phải bằng 6")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Mã voucher không chứa kí tự đặc biệt")]
        [Required(ErrorMessage = "Mã voucher không được bỏ trống")]
        public string Seri { get; set; }
        [Display(Name = "Số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Không nhận giá trị âm")]
        [Required(ErrorMessage ="Số lượng không được bỏ trống")]
        public int Quantity { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        [DateGreaterThan("EndDate", ErrorMessage = "Ngày kết thúc không được bé hơn Ngày bắt đầu")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = null;
        public virtual ICollection<Order> Orders { get; set; }
    }
}