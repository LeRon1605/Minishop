using Models.DAL;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BLL
{
    public class CommentBUS
    {
        private ShopOnlineDbContext context;
        public CommentBUS()
        {
            context = new ShopOnlineDbContext();
        }
        public Comment find(int id)
        {
            Comment comment = context.Comments.Find(id);
            if (comment == null) return null;
            context.Entry(comment).Reference(x => x.User).Load();
            context.Entry(comment).Reference(x => x.ProductOrder).Load();
            context.Entry(comment.ProductOrder).Reference(x => x.Product).Load();
            context.Entry(comment).Reference(x => x.Reply).Load();
            return comment;
        }
        public bool add(int userID, Comment comment)
        {
            ProductOrder productOrder = context.ProductOrder.Find(comment.ID);
            if (productOrder != null)
            {
                context.Entry(productOrder).Reference(p => p.Order).Load();
                context.Entry(productOrder).Reference(p => p.Comment).Load();
            }    
            if (productOrder != null && productOrder.Comment == null && productOrder.Order.UserID == userID && productOrder.Order.isReceived && productOrder.isComment == false)
            {
                productOrder.Comment = new Comment
                {
                    Content = comment.Content,
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
        public bool reply(int ID, Reply reply)
        {
            Comment comment = context.Comments.Find(ID);
            if (comment == null) return false;
            if (comment.Reply == null)
            {
                comment.Reply = new Reply
                {
                    Content = reply.Content,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };
                comment.isReply = true;
            }
            else
            {
                comment.Reply.Content = reply.Content;
                comment.Reply.UpdatedAt = DateTime.Now;
            }
            context.SaveChanges();
            return true;
        }
        public List<Comment> getCommentsOfProduct(int productID)
        {
            return context.Comments.AsNoTracking().Select(comment => new Comment
            {
                ID = comment.ID,
                Content = comment.Content,
                Reply = comment.Reply,
                isReply = comment.isReply,
                isDeleted = comment.isDeleted,
                UserID = comment.UserID,
                User = comment.User,
                UpdatedAt = comment.UpdatedAt,
                DeletedAt = comment.DeletedAt,
                CreatedAt = comment.CreatedAt,
                ProductOrder = comment.ProductOrder,
                Rate = comment.Rate
            }).Where(comment => comment.ProductOrder.ProductID == productID && comment.isDeleted == false).ToList();
        }
        public List<Comment> getPage(int page, int pageSize, bool? isDeleted, bool? isReply, string keyword, DateTime startDate, DateTime endDate, out int totalRow)
        {
            List<Comment> comments = context.Comments.AsNoTracking().Select(comment => new Comment { 
                ID = comment.ID,
                Content = comment.Content,
                Rate = comment.Rate,
                ProductOrder = new ProductOrder
                {
                    Product = comment.ProductOrder.Product
                },
                isDeleted = comment.isDeleted,
                CreatedAt = comment.CreatedAt,
                User = comment.User,
                UserID = comment.UserID
            }).Where(comment => (((isReply == null || comment.isReply == isReply)) && (isDeleted == null || isDeleted == comment.isDeleted) && (comment.CreatedAt.Date >= startDate.Date && comment.CreatedAt.Date <= endDate.Date) && comment.ID.ToString().Contains(keyword))).ToList();
            totalRow = (int)Math.Ceiling((double)comments.Count() / pageSize);
            return comments.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public bool show(bool isDeleted, int id)
        {
            Comment comment = context.Comments.Find(id);
            if (comment == null) return false;
            comment.isDeleted = isDeleted;
            comment.UpdatedAt = DateTime.Now;
            if (isDeleted)
            {
                comment.DeletedAt = DateTime.Now;
            }
            context.SaveChanges();
            return true;
        }

        public bool update(Comment cmt)
        {
            Comment comment = context.Comments.Find(cmt.ID);
            if (comment == null) return false;
            comment.Content = cmt.Content;
            comment.Rate = cmt.Rate;
            comment.UpdatedAt = DateTime.Now;
            context.SaveChanges();
            return true;
        }
    }
}
