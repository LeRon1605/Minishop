using Models.BLL;
using Models.DTO;
using PBL3.Helper;
using PBL3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Areas.Admin.Controllers
{
    [HasLogin(Role = "Admin")]
    public class ImportBillController : Controller
    {
        // GET: Admin/ImportBill
        public ActionResult Index(DateTime? startDate, DateTime? endDate, int page = 1, string keyword = "")
        {
            int totalPage = 0;
            if (startDate == null) startDate = DateTime.MinValue;
            if (endDate == null) endDate = DateTime.MaxValue;
            ViewBag.bills = new ImportBillBUS().getPage(page, 10, keyword, (DateTime)startDate, (DateTime)endDate, out totalPage);
            ViewBag.pagingData = new PagingModel
            {
                CountPages = totalPage,
                CurrentPage = page,
                GenerateURL = (int pageNum) => $"?page={pageNum}&keyword={keyword}&startDate={((DateTime)startDate).ToString("yyyy-MM-dd")}&endDate={((DateTime)endDate).ToString("yyyy-MM-dd")}"
            };
            return View();
        }
        [HttpGet]
        public ActionResult View(int id)
        {
            ImportBill bill = new ImportBillBUS().find(id);
            if (bill == null) return HttpNotFound();
            return View(bill);
        }

        [HttpPost]
        public ActionResult Add(ImportBill bill)
        {

            if (ModelState.IsValid)
            {
                ProductBUS ProductBUS = new ProductBUS();
                foreach (ImportBillDetail detail in bill.ImportBillDetails)
                {
                    if (detail.ProductID != null && !ProductBUS.exist((int)detail.ProductID))
                    {
                        TempData["Status"] = false;
                        TempData["Message"] = "Sản phẩm không tồn tại";
                        return RedirectToAction("Index");
                    };
                }
                new ImportBillBUS().Add(bill);
                TempData["Status"] = true;
                TempData["Message"] = "Nhập hàng thành công";
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = ModelState.Values.SelectMany(v => v.Errors).ToList()[0].ErrorMessage;
            }
            return RedirectToAction("Index");
        }
    }
}