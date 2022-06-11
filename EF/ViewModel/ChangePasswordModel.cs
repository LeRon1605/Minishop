using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBL3.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Mật khẩu hiện tại không được để trống")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Mật khẩu không chứa kí tự đặc biệt")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Mật khẩu có độ dài từ 8 - 32")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Mật khẩu không chứa kí tự đặc biệt")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Mật khẩu có độ dài từ 8 - 32")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Mật khẩu xác nhận không được để trống")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Mật khẩu không chứa kí tự đặc biệt")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Mật khẩu có độ dài từ 8 - 32")]
        public string ConfirmPassword { get; set; }
    }
}