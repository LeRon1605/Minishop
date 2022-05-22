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
        [Key]
        public int ID { get; set; }
        [Display(Name ="Giá trị voucher")]
        [Required(ErrorMessage = "Giá trị không được bỏ trống")]
        public int Value { get; set; }
        [Display(Name ="Seri")]
        [Required(ErrorMessage = "Seri không được bỏ trống")]
        public string Seri { get; set; }
        [Display(Name = "Số lượng")]
        [Required(ErrorMessage ="Số lượng không được bỏ trống")]
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}