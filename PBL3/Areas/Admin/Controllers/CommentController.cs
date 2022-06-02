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
    [HasLogin(Role = "ADMIN")]
    public class CommentController : Controller
    {
        // GET: Admin/Reply
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Reply(Comment comment)
        {
            if (ModelState.IsValid)
            {
                CommentBLL commentBLL = new CommentBLL();
                if (commentBLL.find(comment.ID) == null) return HttpNotFound();
                commentBLL.update(comment);
            }
            return View();
        }
    }
}