using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EF.Models
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }
        public int Rate { get; set; }
        public string Content { get; set; }
        public string ReplyContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}