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
        [Display(Name ="Điểm đánh giá")]
        [Required(ErrorMessage ="Điểm đánh giá không được để trống")]
        [Range(0,5)]
        public int Rate { get; set; }
        [Display(Name ="Nội dung")]
        public string Content { get; set; }
        [Display(Name ="Phản hồi")]
        public string ReplyContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}