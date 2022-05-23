using Models.BLL;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Controllers
{
    public class VoucherController : Controller
    {
        // GET: Voucher
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Check(string Seri)
        {
            Voucher voucher = new VoucherBLL().check(Seri);
            if (voucher != null)
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = true,
                        message = "Mã hợp lệ",
                        value = voucher
                    }
                };
            }
            else
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = false,
                        message = "Mã không hợp lệ hoặc hết hạn"
                    }
                };
            }
        }
    }
}