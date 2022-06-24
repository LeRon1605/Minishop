using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models.DTO
{
    public class Order
    {
        public Order()
        {
            ProductOrder = new List<ProductOrder>();
            StateOrder = new List<StateOrder>();
        }
        [Key]
        public int ID { get; set; }
        [Display(Name ="Địa chỉ nhận")]
        [Required(ErrorMessage ="Địa chỉ nhận không được để trống")]
        public string ReceiverAddress { get; set; }
        [Display(Name = "Tên người nhận")]
        [Required(ErrorMessage = "Tên người nhận không được để trống")]
        public string ReceiverName { get; set; }
        [Display(Name = "Số điện thoại người nhận")]
        [Required(ErrorMessage = "Số điện thoại người nhận không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string ReceiverPhone { get; set; }
        public string Note { get; set; }
        [Display(Name ="Tổng")]
        [Required(ErrorMessage ="Không dược để trống")]
        [Range(0,int.MaxValue)]
        public int Total { get; set; }
        public int Sale { get; set; }
        public int UserID { get; set; }
        public int? VoucherID { get; set; }
        public bool isCancel { get; set; }
        public bool isReceived { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        [ForeignKey("VoucherID")]
        public virtual Voucher Voucher { get; set; }
        public virtual List<ProductOrder> ProductOrder { get; set; }
        public virtual List<StateOrder> StateOrder { get; set; }
    }
}