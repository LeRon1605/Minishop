using Models.BLL;
using Models.DAL;
using Models.DTO;
using Models.ViewModel;
using PBL3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBL3.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int id)
        {
            Product product = new ProductBO().find(id);
            if(product == null)
            {
                return RedirectToAction("Index", new
                {
                    controller = "Home"
                });
            }
            else
            {
                List<Comment> comments = new CommentBO().getCommentsOfProduct(id);
                List<Rating> rates = new List<Rating>();
               
                if ( comments.Count() > 0)
                {
                    ViewBag.AvgRating = comments.Sum(x => x.Rate) / comments.Count();
                    for (int i = 1; i <= 5; i++)
                    {
                        rates.Add(new Rating
                        {
                            key = i,
                            quantity = comments.Where(x => x.Rate == i).Count(),
                            percent = (Convert.ToSingle(comments.Where(x => x.Rate == i).Count() * i) / comments.Sum(x => x.Rate)) * 100,
                        });
                    }
                }   
                else if(comments.Count() == 0)
                {
                    ViewBag.AvgRating = 0;
                    for (int i = 1; i <= 5; i++)
                    {
                        rates.Add(new Rating
                        {
                            key = i,
                            quantity = comments.Where(x => x.Rate == i).Count(),
                            percent = 0
                        });
                    }
                } 
                ViewBag.Comments = comments;
                ViewBag.Rates = rates;
                return View(product);
            }
        }
        public ActionResult Search(string keyword = "", string categoryID = "All", string price = "All", int page = 1)
        {
            ProductBO productDAO = new ProductBO();
            int totalPage = 0;
            List<Product> products = productDAO.getPage(page, 20, keyword, categoryID, price, out totalPage);
            ViewBag.categories = new CategoryBO().findAll();
            ViewBag.pagingData = new PagingModel
            {
                CountPages = totalPage,
                CurrentPage = page,
                GenerateURL = (int pageNum) => $"?page={pageNum}&keyword={keyword}&CategoryID={categoryID}&Price={price}"
            };
            return View(products);
        }
    }
}