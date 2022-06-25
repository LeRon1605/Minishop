using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models.DTO
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Điểm đánh giá")]
        [Required(ErrorMessage ="Điểm đánh giá không được để trống")]
        [Range(1, 5)]
        public int Rate { get; set; }
        [Display(Name = "Nội dung đánh giá")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Nội dung đánh giá có độ dài từ 10 - 255 kí tự.")]
        [Required(ErrorMessage = "Nội dung đánh giá không được để trống")]
        public string Content { get; set; }
        public bool isReply { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        public virtual Reply Reply { get; set; }
        public virtual ProductOrder ProductOrder { get; set; }
    }
}