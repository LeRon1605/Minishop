using EF.Models;
using EF.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using PBL3.Models;
using PBL3.Helper;
using System.Web.Routing;

namespace PBL3.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["USER"] != null) return RedirectToAction("Index");
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Login(LoginModel data)
        {
            if (Session["USER"] == null)
            {
                User user = new UserDAO().Login(data.Email, Encryptor.MD5Hash(data.Password));
                if (user != null)
                {
                    Session["USER"] = user;
                    TempData["Message"] = "Đăng nhập thành công";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Message"] = "Sai thông tin";
                    return View("Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home"); 
            }  
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(User user)
        {
            if (Session["USER"] == null)
            {
                if (ModelState.IsValid)
                {
                    user.Password = Encryptor.MD5Hash(user.Password);
                    bool result = await new UserDAO().Add(user);
                    if (result)
                    {
                        // TempData["Message"] = "Đăng kí thành viên thành công";
                        Session["User"] = user;
                        return RedirectToAction("ActivateAccount");
                    }
                    else
                    {
                        TempData["Message"] = "Email đã tồn tại";
                        return RedirectToAction("Register");
                    }
                }
                else
                {
                    TempData["Message"] = "Dữ liệu không hợp lệ";
                    return RedirectToAction("Register");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public async Task<ActionResult> ActivateAccount()
        {
            if (Session["User"] == null) return RedirectToAction("Index", "Home");
            else
            {
                User user = (User)Session["User"];
                if (user.isActivated)
                {
                    // Đã kích hoạt rồi
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ActivateAccountModel data = Session["ActivateAccount"] as ActivateAccountModel;
                    if (data == null)
                    {
                        string key = new Random().Next(1000, 9999).ToString();
                        Session["ActivateAccount"] = new ActivateAccountModel
                        {
                            AccountID = user.ID,
                            Key = key,
                            Date = DateTime.Now
                        };
                        await Mail.SendMail(user.Email, "Kích hoạt tài khoản", Mail.GetMailContent(new string[] { key }));
                    }
                    else
                    {
                        if (data.Date.AddMinutes(3) > DateTime.Now)
                        {
                            TempData["Message"] = "Mỗi lần gửi cách nhau 3 phút";
                        }
                        else
                        {
                            string key = new Random().Next(1000, 9999).ToString();
                            Session["ActivateAccount"] = new ActivateAccountModel
                            {
                                AccountID = user.ID,
                                Key = key,
                                Date = DateTime.Now
                            };
                            await Mail.SendMail(user.Email, "Kích hoạt tài khoản", Mail.GetMailContent(new string[] { key }));
                        }
                    }
                    return View();
                }
            }
        }
        [HttpPost]
        public async Task<ActionResult> ActivateAccount(string key)
        {
            ActivateAccountModel data = Session["ActivateAccount"] as ActivateAccountModel;
            Session["ActivateAccount"] = null;
            if (data.Key == key)
            {
                if (DateTime.Now < data.Date.AddMinutes(3))
                {
                    await new UserDAO().Activate(data.AccountID);
                    TempData["Message"] = "Kích hoạt thành công";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Message"] = "Mã hết hạn";
                    return RedirectToAction("ActivateAccount");
                }
            }
            else
            {
                TempData["Message"] = "Mã không đúng";
                return RedirectToAction("ActivateAccount");
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}