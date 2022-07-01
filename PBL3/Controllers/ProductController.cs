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
            Product product = new ProductBUS().find(id);
            if(product == null)
            {
                return HttpNotFound();
            }
            else
            {
                List<Comment> comments = new CommentBUS().getCommentsOfProduct(id);
                List<Rating> rates = new List<Rating>();
                for (int i = 1; i <= 5; i++)
                {
                    rates.Add(new Rating
                    {
                        key = i,
                        quantity = comments.Where(x => x.Rate == i).Count(),
                        percent = (comments.Count() == 0) ? 0 : comments.Where(x => x.Rate == i).Count() * 100.0 / comments.Count(),
                    });
                }
                ViewBag.AvgRating = (comments.Count() == 0) ? 0 : comments.Sum(x => x.Rate) / comments.Count();
                ViewBag.Comments = comments;
                ViewBag.Rates = rates;
                return View(product);
            }
        }
        public ActionResult Search(string minValue, string maxValue, string keyword = "", bool? state = null, string categoryID = "All", int page = 1)
        {
            ProductBUS productBUS = new ProductBUS();
            int totalPage = 0;
            List<Product> products = productBUS.getPage(page, 20, state, keyword, categoryID, minValue, maxValue, out totalPage);
            ViewBag.categories = new CategoryBUS().findAll();
            ViewBag.pagingData = new PagingModel
            {
                CountPages = totalPage,
                CurrentPage = page,
                GenerateURL = (int pageNum) => $"?page={pageNum}&state={state}&keyword={keyword}&CategoryID={categoryID}&minValue={minValue}&maxValue={maxValue}"
            };
            return View(products);
        }
    }
}