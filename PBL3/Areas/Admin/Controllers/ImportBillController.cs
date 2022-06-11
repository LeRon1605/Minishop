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
            ViewBag.bills = new ImportBillBO().getPage(page, 10, keyword, (DateTime)startDate, (DateTime)endDate, out totalPage);
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
            ImportBill bill = new ImportBillBO().find(id);
            if (bill == null) return HttpNotFound();
            return View(bill);
        }

        [HttpPost]
        public ActionResult Add(ImportBill bill)
        {
            if (ModelState.IsValid)
            {
                ProductBO productBO = new ProductBO();
                foreach (ImportBillDetail detail in bill.ImportBillDetails)
                {
                    if (detail.ProductID != null && productBO.exist((int)detail.ProductID) == false)
                    {
                        TempData["Status"] = false;
                        TempData["Message"] = "Sản phẩm không tồn tại";
                        return RedirectToAction("Index");
                    };
                }
                new ImportBillBO().Add(bill);
                TempData["Status"] = true;
                TempData["Message"] = "Nhập hàng thành công";
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Dữ liệu không hợp lệ";
            }
            return RedirectToAction("Index");
        }

        //public ActionResult Update(ImportBill bill)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ImportBillBO importBillBO = new ImportBillBO();
        //        if (importBillBO.Update(bill) == true)
        //        {
        //            TempData["Status"] = true;
        //            TempData["Message"] = "Cập nhật hóa đơn thành công";
        //        }
        //        else
        //        {
        //            TempData["Status"] = true;
        //            TempData["Message"] = "Cập nhật hóa đơn thất bại";
        //        }
        //    }
        //    else
        //    {
        //        TempData["Status"] = false;
        //        TempData["Message"] = "Cập nhật hóa đơn thất bại";

        //    }
        //    return RedirectToAction("View");
        //}

        public ActionResult Delete(int id)
        {
            ImportBillBO importBillBO = new ImportBillBO();
            if (importBillBO.delete(id))
            {
                TempData["Status"] = true;
                TempData["Message"] = "Xóa đơn hàng thành công";
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Xóa đơn hàng thất bại";
            }
            return RedirectToAction("Index");
        }

    }
}