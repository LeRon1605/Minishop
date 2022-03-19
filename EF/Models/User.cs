using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EF.Models
{
    [Serializable]
    public class User
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DisplayName("Tên người dùng")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Tên người dùng phải có độ dài từ 10 - 50")]
        [Required(ErrorMessage = "Tên người dùng không được để trống")]
        public string Name { get; set; }
        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "Địa chỉ người dùng không được để trống")]
        public string Address { get; set; }
        [DisplayName("Giới tính")]
        [Required(ErrorMessage = "Giới tính không được để trống")]
        public string Gender { get; set; }
        public DateTime Birth { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        
        public string Password { get; set; }
        public bool isActivated { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public Role Role { get; set; }
        public List<Order> Orders { get; set; }
        public List<Comment> Comments { get; set; }
    }
}