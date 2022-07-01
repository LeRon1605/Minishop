using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class Reply
    {
        [ForeignKey("Comment")]
        public int ID { get; set; }
        [Required(ErrorMessage = "Bình luận phản hồi không dược để trống")]
        [StringLength(255, MinimumLength = 10, ErrorMessage = "Bình luận có độ dài ít nhất 10 kí tự")]
        public string Content { get; set; }
        [DisplayName("Thời gian phản hồi")]
        public DateTime CreatedAt { get; set; }
        [DisplayName("Thời gian chỉnh sửa")]
        public DateTime? UpdatedAt { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
