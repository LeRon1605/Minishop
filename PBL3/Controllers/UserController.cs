using Models.DTO;
using Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using PBL3.Models;
using PBL3.Helper;
using System.Web.Routing;
using System.IO;

namespace PBL3.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HasLogin(Role = "USER")]
        public ActionResult Index()
        {
             User user = new UserBUS().find((int)Session["USER"]);
             ViewBag.totalOrder = new OrderBUS().getUserOrderCount((int)Session["USER"]);
             return View(user);
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
            if (ModelState.IsValid)
            {
                if (Session["USER"] == null)
                {
                    User user = new UserBUS().Login(data.Email, Encryptor.MD5Hash(data.Password));
                    if (user != null)
                    {
                        Session["USER"] = user.ID;
                        Role role = new RoleBUS().find((int)user.RoleID);
                        if (role.Name == "ADMIN") return RedirectToAction("Index", "Home", new { area = "Admin" });
                        else return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Message"] = "Tài khoản hoặc mật khẩu không đúng";
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["Message"] = ModelState.Values.SelectMany(v => v.Errors).ToList()[0].ErrorMessage;
                return RedirectToAction("Login");
            }
        }
        public ActionResult Logout()
        {
            Session["USER"] = null;
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Register()
        {
            if (Session["USER"] == null) return View();
            else return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<ActionResult> Register(User user)
        {
            if (Session["USER"] == null)
            {
                if (ModelState.IsValid)
                {
                    user.Password = Encryptor.MD5Hash(user.Password);
                    bool result = await new UserBUS().Add(user);
                    if (result)
                    {
                        Session["User"] = user.ID;
                        return RedirectToAction("Index", "Home");
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
        [HasLogin(Role = "USER")]
        [HttpGet]
        public async Task<ActionResult> ActivateAccount()
        {
            if (Session["User"] == null) return RedirectToAction("Index", "Home");
            else
            {
                User user = new UserBUS().find((int)Session["USER"]);
                if (user.isActivated)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    await SendActiveKey(user.ID, user.Email);
                    return View((ActivateAccountModel)Session["ActivateAccount"]);
                }
            }
        }
        [HttpPost]
        [HasLogin(Role = "USER")]
        public async Task<ActionResult> ActivateAccount(string key)
        {
            ActivateAccountModel data = Session["ActivateAccount"] as ActivateAccountModel;
            Session["ActivateAccount"] = null;
            if (data.Key == key)
            {
                if (DateTime.Now < data.Date.AddMinutes(3))
                {
                    UserBUS userDAO = new UserBUS();
                    await userDAO.Activate(data.userID);
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
        [HasLogin(Role = "USER")]
        public async Task<ActionResult> SendActiveKey(int userID, string userEmail)
        {
            ActivateAccountModel data = Session["ActivateAccount"] as ActivateAccountModel;
            if (data == null)
            {
                string key = new Random().Next(1000, 9999).ToString();
                Session["ActivateAccount"] = new ActivateAccountModel
                {
                    userID = userID,
                    Key = key,
                    userEmail = userEmail,
                    Date = DateTime.Now
                };
                await Mail.SendMail(userEmail, "Kích hoạt tài khoản", Mail.GetMailActivateContent(key));
                return new JsonResult { 
                    Data = new
                    {
                        message = "Gửi thành công",
                        success = true
                    }
                };
            }
            else
            {
                if (data.Date.AddSeconds(15) > DateTime.Now)
                {
                    return new JsonResult
                    {
                        Data = new
                        {
                            message = "Mỗi lần gửi cách nhau 15 giây",
                            success = false
                        }
                    };
                }
                else
                {
                    string key = new Random().Next(1000, 9999).ToString();
                    Session["ActivateAccount"] = new ActivateAccountModel
                    {
                        userID = userID,
                        Key = key,
                        userEmail = userEmail,
                        Date = DateTime.Now
                    };
                    await Mail.SendMail(userEmail, "Kích hoạt tài khoản", Mail.GetMailActivateContent(key));
                    return new JsonResult
                    {
                        Data = new
                        {
                            message = "Gửi thành công",
                            success = true
                        }
                    };
                }
            }
        }
        [HttpPost]
        [HasLogin(Role = "USER")]
        public ActionResult Update(User user, HttpPostedFileBase file)
        {
            if (ModelState["Password"] != null) ModelState["Password"].Errors.Clear();
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/public/uploads/users"), fileName);
                    file.SaveAs(path);
                    user.Image = $"/public/uploads/users/{fileName}";
                }
                if (user.ID == (int)Session["USER"])
                {
                    if (new UserBUS().Update(user))
                    {
                        TempData["Status"] = true;
                        TempData["Message"] = "Cập nhật tài khoản công";
                    }
                    else
                    {
                        TempData["Status"] = false;
                        TempData["Message"] = "Cập nhật tài khoản thất bại";
                    }
                }
                else
                {
                    ViewBag.Message = "Bạn không có quyền thực hiện hành động này";
                    return View("~/Views/Shared/UnAuthorize.cshtml");
                } 
            }
            else
            {
                TempData["Status"] = false;
                TempData["Message"] = "Dữ liệu không hợp lệ";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            if (Session["USER"] != null)
            {
                return RedirectToAction("Index", "Home");
            }    
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(string email)
        {
            UserBUS userBUS = new UserBUS();
            User user = userBUS.findByEmail(email);
            if (user != null)
            {
                string password = new Random().Next(10000000, 99999999).ToString();
                userBUS.ChangePassword(password, user.ID);
                await Mail.SendMail(email, "Đặt lại mật khẩu", Mail.GetMailResetPasswordContent(password));
                return new JsonResult
                {
                    Data = new
                    {
                        status = true,
                        message = "Vui lòng kiểm tra email để tiếp tục đăng nhập"
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
                        message = "Đặt lại mật khẩu thất bại",
                        detail = "Không tồn tại người dùng"
                    }
                };
            }
        }
        [HasLogin(Role = "USER")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new UserBUS().find((int)Session["USER"]);
                if (user != null)
                {
                    if (Encryptor.MD5Hash(model.OldPassword) == user.Password)
                    {
                        bool result = new UserBUS().ChangePassword(model.NewPassword, (int)Session["USER"]);
                        return new JsonResult
                        {
                            Data = new
                            {
                                status = true,
                                message = "Thay đổi mật khẩu thành công"
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
                                message = "Thay đổi mật khẩu thất bại",
                                detail = "Mật khẩu không chính xác"
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
                            message = "Thay đổi mật khẩu thất bại",
                            detail = "Người dùng không tồn tại"
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
                        message = "Thay đổi mật khẩu thất bại",
                        detail = ModelState.Values.SelectMany(v => v.Errors).ToList()[0].ErrorMessage
                    }
                };
            }
        }
        [HttpGet]
        [HasLogin(Role = "USER")]
        public ActionResult Orders(int stateID = 0, string keyword = "")
        {
            List<Order> orders = new OrderBUS().getUserOrders((int)Session["USER"], stateID, keyword);
            ViewBag.States = new StateBUS().findAll();
            return View(orders);
        }
    }
}