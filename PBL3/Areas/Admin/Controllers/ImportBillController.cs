using Models.BLL;
using Models.DTO;
using PBL3.Helper;
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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Update(ImportBill bill)
        {
            if(ModelState.IsValid)
            {
                ImportBillBO importBillBO = new ImportBillBO();
                if(importBillBO.Update(bill) == true)
                {
                    TempData["Status"] = true;
                    TempData["Message"] = "Cập nhật hóa đơn thành công";
                }    
                else
                {
                    TempData["Status"] = true;
                    TempData["Message"] = "Cập nhật hóa đơn thất bại";
                }
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Cập nhật hóa đơn thất bại";
               
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            ImportBillBO importBillBO = new ImportBillBO();
            if (importBillBO.Delete(id))
            {
                return new JsonResult
                {
                    Data = new
                    {
                        message = "Xóa thành công hóa hàng",
                        status = true
                    }
                };
            }
            else
            {
                return new JsonResult
                {
                    Data = new
                    {
                        message = "Xóa hóa đơn thất bại",
                        detail = "Hóa đơn không tồn tại",
                        status = true
                    }
                };
            }
        }

    }
}