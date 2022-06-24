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
    [HasLogin(Role = "ADMIN")]
    public class CommentController : Controller
    {
        // GET: Admin/Reply
        public ActionResult Index(DateTime? startDate, DateTime? endDate, string keyword = "", bool? isDeleted = null, bool? isReply = null, int page = 1)
        {
            int totalPage = 0;
            if (startDate == null) startDate = DateTime.MinValue;
            if (endDate == null) endDate = DateTime.MaxValue;
            List<Comment> comments = new CommentBUS().getPage(page, 10, isDeleted, isReply, keyword, (DateTime)startDate, (DateTime)endDate, out totalPage);
            ViewBag.pagingData = new PagingModel
            {
                CountPages = totalPage,
                CurrentPage = page,
                GenerateURL = (int pageNum) => $"?page={pageNum}&keyword={keyword}&startDate={((DateTime)startDate).ToString("yyyy-MM-dd")}&endDate={((DateTime)endDate).ToString("yyyy-MM-dd")}&isReply={isReply}"
            };
            return View(comments);
        }
        public ActionResult View(int id)
        {
            Comment comment = new CommentBUS().find(id);
            if (comment == null) return HttpNotFound();
            return View(comment);
        }
        public ActionResult Delete(int id)
        {
            if (new CommentBUS().show(true, id))
            {
                TempData["Status"] = true;
                TempData["Message"] = "Ẩn bình luận thành công";
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Bình luận không tồn tại";
            }
            return RedirectToAction("View", new { id = id });
        }
        public ActionResult Show(int id)
        {
            if (new CommentBUS().show(false, id))
            {
                TempData["Status"] = true;
                TempData["Message"] = "Hiện bình luận thành công";
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Bình luận không tồn tại";
            }
            return RedirectToAction("View", new { id = id });
        }
        public ActionResult Reply(Comment cmt)
        {
            if (ModelState["Rate"] != null) ModelState["Rate"].Errors.Clear();
            if (ModelState["Content"] != null) ModelState["Content"].Errors.Clear();
            if (ModelState.IsValid)
            {
                CommentBUS CommentBUS = new CommentBUS();
                Comment comment = CommentBUS.find(cmt.ID);
                if (comment == null) return HttpNotFound();
                CommentBUS.reply(cmt.ID, cmt.Reply);
                TempData["Status"] = true;
                TempData["Message"] = "Phản hồi bình luận thành công";
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Dữ liệu không hợp lệ";
            }
            return RedirectToAction("View", new { id = cmt.ID });
        }
    }
}