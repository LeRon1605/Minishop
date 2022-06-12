using Models.DAL;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BLL
{
    public class CommentBO
    {
        private ShopOnlineDbContext context;
        public CommentBO()
        {
            context = new ShopOnlineDbContext();
        }
        public Comment find(int id)
        {
            return context.Comments.Find(id);
        }
        public bool add(int userID, Comment comment)
        {
            ProductOrder productOrder = context.ProductOrder.Find(comment.ID);
            context.Entry(productOrder).Reference(p => p.Order).Load();
            context.Entry(productOrder).Reference(p => p.Comment).Load();
            if (productOrder != null && productOrder.Comment == null && productOrder.Order.UserID == userID && productOrder.Order.isReceived && productOrder.isComment == false)
            {
                productOrder.Comment = new Comment
                {
                    Content = comment.Content,
                    ReplyContent = "",
                    UserID = userID,
                    Rate = comment.Rate,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null,
                    DeletedAt = null,
                    isDeleted = false
                };
                productOrder.isComment = true;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public List<Comment> getCommentsOfProduct(int productID)
        {
            return context.Comments.Select(comment => new Comment
            {
                ID = comment.ID,
                Content = comment.Content,
                ReplyContent = comment.Content,
                isDeleted = comment.isDeleted,
                UserID = comment.UserID,
                User = comment.User,
                UpdatedAt = comment.UpdatedAt,
                DeletedAt = comment.DeletedAt,
                CreatedAt = comment.CreatedAt,
                ProductOrder = comment.ProductOrder,
                Rate = comment.Rate
            }).Where(comment => comment.ProductOrder.ProductID == productID).OrderBy(comment => comment.CreatedAt).ToList();
        }
        public List<Comment> getPage(int page, int pageSize, string isReply, DateTime startDate, DateTime endDate, out int totalRow)
        {
            List<Comment> comments = context.Comments.Where(comment => (isReply == "All" || !string.IsNullOrEmpty(comment.ReplyContent) == Convert.ToBoolean(isReply)) && (comment.CreatedAt.Date >= startDate.Date && comment.CreatedAt.Date <= endDate.Date)).ToList();
            totalRow = (int)Math.Ceiling((double)comments.Count() / pageSize);
            return comments.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public bool delete(int id)
        {
            Comment comment = context.Comments.Find(id);
            if (comment == null) return false;
            comment.isDeleted = true;
            comment.UpdatedAt = DateTime.Now;
            comment.DeletedAt = DateTime.Now;
            context.SaveChanges();
            return true;
        }

        public bool update(Comment cmt)
        {
            Comment comment = context.Comments.Find(cmt.ID);
            if (comment == null) return false;
            comment.Content = cmt.Content;
            comment.Rate = cmt.Rate;
            comment.ReplyContent = cmt.ReplyContent;
            comment.UpdatedAt = DateTime.Now;
            context.SaveChanges();
            return true;
        }
    }
}
