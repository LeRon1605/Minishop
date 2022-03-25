using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EF.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }
        [Display(Name ="Địa chỉ nhận")]
        [Required(ErrorMessage ="Địa chỉ nhận không được để trống")]
        public string ReceiverAddress { get; set; }
        [Display(Name ="Tổng")]
        [Required(ErrorMessage ="Không dược để trống")]
        [Range(0,int.MaxValue)]
        public int Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Voucher Voucher { get; set; }
        public User User { get; set; }
        public List<ProductOrder> ProductOrder { get; set; }
        public List<StateOrder> StateOrder { get; set; }
    }
}