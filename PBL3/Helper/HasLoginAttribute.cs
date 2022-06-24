using Models.DTO;
using Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Helper
{
    public class HasLoginAttribute : AuthorizeAttribute
    {
        public string Role { get; set; } = "ALL";
        public string Message { get; set; }
        public string ContentType { get; set; } = "View";
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            object userID = HttpContext.Current.Session["USER"];
            if (userID == null)
            {
                Message = "Bạn chưa đăng nhập";
                return false;
            }
            else
            {
                User user = new UserBUS().find((int)userID);
                if (user == null)
                {
                    Message = "Người dùng không tồn tại";
                    return false;
                }
                else
                {
                    if (Role == "ALL") return true;
                    Role userRole = new RoleBUS().find((int)user.RoleID);
                    if (userRole == null)
                    {
                        Message = "Lỗi hệ thống";
                        return false;
                    }
                    else {
                        if (!(userRole.Name.ToLower() == Role.ToLower()))
                        {
                            Message = "Bạn không có quyền truy cập tài nguyên này";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    };
                }
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (ContentType == "View")
            {
                ViewResult result = new ViewResult();
                result.ViewName = "~/Views/Shared/UnAuthorize.cshtml";
                result.ViewBag.Message = this.Message;
                filterContext.Result = result;
            }    
            else if (ContentType.ToLower() == "json")
            {
                JsonResult result = new JsonResult
                {
                    Data = new
                    {
                        status = false,
                        message = this.Message
                    }
                };
                filterContext.Result = result;
            }    
        }
    }
}