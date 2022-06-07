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
            // 
            return null;
        }

        public ActionResult Delete(int id)
        {
            // 
            return null;
        }

    }
}