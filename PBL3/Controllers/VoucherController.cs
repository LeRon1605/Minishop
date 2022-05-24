using PBL3.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Controllers
{
    public class VoucherController : Controller
    {
        [HasLogin(Role = "USER")]
        // GET: Voucher
        public ActionResult Index()
        {
            return View();
        }
    }
}