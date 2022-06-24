using PBL3.Helper;
using Models.BLL;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PBL3.Models;

namespace PBL3.Controllers
{
    public class VoucherController : Controller
    {
        // GET: Voucher
        public ActionResult Index(int page = 1, string keyword = "", string value = "All")
        {
            VoucherBUS voucherDAO = new VoucherBUS();
            int totalPage = 0;
            List<Voucher> voucherList = voucherDAO.getPage(page, 12, keyword, value, "valid", out totalPage);
            ViewBag.Total = voucherDAO.Count();
            ViewBag.pagingData = new PagingModel
            {
                CountPages = totalPage,
                CurrentPage = page,
                GenerateURL = (int pageNum) => $"?page={pageNum}&keyword={keyword}&value={value}"
            };
            return View(voucherList);
        }
        [HttpPost]
        public ActionResult Check(string Seri)
        {
            Voucher voucher = new VoucherBUS().check(Seri);
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