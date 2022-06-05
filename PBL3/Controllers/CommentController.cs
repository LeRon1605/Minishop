using Models.BLL;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index(int id)
        {
            Comment comment = new CommentBLL().find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }    
            return View(comment);
        }
        [HttpPost]
        public ActionResult Comment(Comment cmt)
        {
            if (ModelState.IsValid)
            {
                CommentBLL commentBLL = new CommentBLL();
                Comment comment = commentBLL.find(cmt.ID);
                if (comment == null)
                {
                    if (new CommentBLL().add((int)Session["USER"], cmt))
                    {
                        return new JsonResult
                        {
                            Data = new
                            {
                                status = true,
                                message = "Đánh giá sản phẩm thành công"
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
                                message = "Đánh giá sản phẩm thất bại"
                            }
                        };
                    }
                }
                else
                {
                    if (comment.UserID == (int)Session["USER"])
                    {
                        if (commentBLL.update(cmt))
                        {
                            return new JsonResult
                            {
                                Data = new
                                {
                                    status = true,
                                    message = "Cập nhật thành công"
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
                                    message = "Cập nhật thất bại"
                                }
                            };
                        }
                    }
                    else
                    {
                        return new JsonResult
                        {
                            Data = new
                            {
                                status = false,
                                message = "Cập nhật thất bại",
                                detail = "Bạn không có quyền chỉnh sửa bình luận của người dùng khác"
                            }
                        };
                    }
                }
            }
            else
            {
                return new JsonResult
                {
                    Data = new
                    {
                        status = false,
                        message = "Cập nhật thất bại",
                        detail = "Dữ liệu không hợp lệ"
                    }
                };
            }     
        }
    }
}