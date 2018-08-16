using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.Models;
using ZSZ.Common;
using ZSZ.IService;
using ZSZ.WebCommon;

namespace ZSZ.AdminWeb.Controllers
{
    public class MainController : Controller
    {
        public IAdminUserService userService { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg =MVCHelper.GetValidMsg(ModelState) });
            }
            if(model.VerifyCode!=(string)TempData["verifyCode"])
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }
            if(userService.CheckLogin(model.PhoneNum,model.Password))
            {
                Session["AdminUserId"] = userService.GetByPhoneNum(model.PhoneNum).Id;
                return Json(new AjaxResult { Status = "ok"});
            }
            else
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "手机号密码错误" });
            }

        }
        public ActionResult CreateVerifyCode()
        {
            string verifyCode = CommonHelper.GetCaptcha(4);
            TempData["verifyCode"] = verifyCode;
            MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 60, 100, 20, 6);
            return File(ms, "image/jpeg");
        }
    }
}