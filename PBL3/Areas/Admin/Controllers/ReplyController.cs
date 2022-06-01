using PBL3.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Areas.Admin.Controllers
{
    //[HasLogin(Role = "ADMIN")]
    public class ReplyController : Controller
    {
        // GET: Admin/Reply
        public ActionResult Index()
        {
            return View();
        }
    }
}