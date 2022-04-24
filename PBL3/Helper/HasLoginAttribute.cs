using EF.DAO;
using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Helper
{
    public class HasLoginAttribute : AuthorizeAttribute
    {
        public string Role { get; set; } = "USER";
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            object userID = HttpContext.Current.Session["USER"];
            if (userID == null) return false;
            else
            {
                User user = new UserDAO().find((int)userID);
                if (user == null) return false;
                else
                {
                    Role userRole = new RoleDAO().find((int)user.RoleID);
                    if (userRole == null) return false;
                    else return (userRole.Name.ToLower() == Role.ToLower());
                }
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/UnAuthorize.cshtml"
            };
        }
    }
}